using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class DepartmentsRanking : MonoBehaviour
{
    [Serializable]
    public class DepartmentRow : IComparable<DepartmentRow>
    {
        public string name;
        public string niceName;
        public RectTransform row;
        public Text scoreCounter;
        public Sprite iconSprite;
        public int currentScore = 0;

        [HideInInspector]
        public int lastScore = 0;

        [HideInInspector]
        public Coroutine posCoro = null;
        [HideInInspector]
        public Coroutine lengthCoro = null;
        [HideInInspector]
        public Coroutine scoreCoro = null;


        private bool configuredFirebase = false;

        public int CompareTo(DepartmentRow other)
        {
            return other.currentScore.CompareTo(currentScore);
        }

        public void ConfigureFirebase()
        {
            if (configuredFirebase)
                return;

            FirebaseStartup.DatabaseReference.Child("departmentsRanking").Child(name).ValueChanged += DepartmentsRanking_ValueChanged;

            configuredFirebase = true;
        }
        private void DepartmentsRanking_ValueChanged(object sender, Firebase.Database.ValueChangedEventArgs e)
        {
            if (e.DatabaseError != null)
            {
                Debug.LogError(e.DatabaseError.Message);
                return;
            }
            //Debug.Log("DepartmentsRanking_ValueChanged " + name + " = " + e.Snapshot.Value.ToString());
            currentScore = int.Parse(e.Snapshot.Value.ToString());
        }
    }

    public float startPosY = -235.0f;
    public float fullRowHeight = 138.0f;
    public float minLength = 240.0f;
    public float maxLength = 1040.0f;

    public float animTime = 0.5f;

    public DepartmentRow[] departments;

    public List<GameObject> noDepartmentControls;
    public List<GameObject> yesDepartmentControls;
    public Image currentDepartmentBackground;
    public Image currentDepartmentLogo;
    public Text currentDepartmentName;

    public float noDepartmentRowAlpha = 0.75f;
    public float myDepartmentRowAlpha = 1.0f;
    public float theirDepartmentRowAlpha = 0.5f;

    private void Start()
    {
        foreach (var dep in departments)
        {
            //Debug.Log("setup " + dep.name + "    " + FirebaseStartup.DatabaseReference.Child("departmentsRanking").Child(dep.name).ToString());
            dep.ConfigureFirebase();
        }
    }

    public void AnimUpdateBars()
    {
        List<DepartmentRow> tmp = new List<DepartmentRow>(departments);
        tmp.Sort();

        float maxScore = tmp[0].currentScore;
        float minScore = tmp[tmp.Count-1].currentScore;

        for (int i=0; i<tmp.Count; i++)
        {
            float fscore = tmp[i].currentScore;
            float targetLength = minLength + (maxLength - minLength) * ((fscore - minScore) / (maxScore - minScore));

            if (tmp[i].posCoro != null)
                StopCoroutine(tmp[i].posCoro);
            if (tmp[i].lengthCoro != null)
                StopCoroutine(tmp[i].lengthCoro);
            if (tmp[i].scoreCoro != null)
                StopCoroutine(tmp[i].scoreCoro);

            tmp[i].posCoro = StartCoroutine(animYPos(tmp[i], tmp[i].row.localPosition.y, startPosY + fullRowHeight * i));
            tmp[i].lengthCoro = StartCoroutine(animLength(tmp[i], tmp[i].row.sizeDelta.x, targetLength));
            tmp[i].scoreCoro = StartCoroutine(animScore(tmp[i], tmp[i].lastScore, tmp[i].currentScore));
        }
    }
    public void AnimUpdateBarsDelayed(float delay)
    {
        Invoke("AnimUpdateBars", delay);
    }

    public void ChooseDepartment(string name)
    {
        PlayerPrefs.SetString("ChoosenDepartment", name);
        PlayerPrefs.Save();
        UpdateCurrentDepartment();
    }

    public void UpdateCurrentDepartment()
    {
        if (PlayerPrefs.HasKey("ChoosenDepartment"))
        {
            string depName = PlayerPrefs.GetString("ChoosenDepartment");
            noDepartmentControls.ForEach(c => c.SetActive(false));
            foreach (var dep in departments)
            {
                if (dep.name == depName)
                {
                    yesDepartmentControls.ForEach(c => c.SetActive(true));

                    Color rc = dep.row.GetComponent<Image>().color;
                    rc.a = myDepartmentRowAlpha;
                    dep.row.GetComponent<Image>().color = rc;

                    currentDepartmentBackground.color = rc;
                    currentDepartmentLogo.sprite = dep.iconSprite;
                    currentDepartmentName.text = dep.niceName;
                }
                else
                {
                    Color rc = dep.row.GetComponent<Image>().color;
                    rc.a = theirDepartmentRowAlpha;
                    dep.row.GetComponent<Image>().color = rc;
                }
            }
        }
        else
        {
            noDepartmentControls.ForEach(c => c.SetActive(true));
            yesDepartmentControls.ForEach(c => c.SetActive(false));
            foreach (var dep in departments)
            {
                Color rc = dep.row.GetComponent<Image>().color;
                rc.a = noDepartmentRowAlpha;
                dep.row.GetComponent<Image>().color = rc;
            }
        }
    }

    public void AddScoreToMyDepartmentIfCan(int score)
    {
        if (FirebaseStartup.DatabaseReference == null)
            return;

        if (PlayerPrefs.HasKey("ChoosenDepartment"))
        {
            string depName = PlayerPrefs.GetString("ChoosenDepartment");
            FirebaseStartup.DatabaseReference.Child("departmentsRanking").Child(depName).RunTransaction(mutableData => {
                int currentScore = int.Parse(mutableData.Value.ToString());
                mutableData.Value = currentScore + score;
                return Firebase.Database.TransactionResult.Success(mutableData);
            });
        }
    }

    IEnumerator animYPos(DepartmentRow dep, float from, float to)
    {
        Vector3 position = dep.row.localPosition;
        position.y = from;
        dep.row.localPosition = position;
        float dt = 0.0f;
        while (dt < animTime)
        {
            position.y = Mathf.SmoothStep(from, to, dt / animTime);
            dep.row.localPosition = position;
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        position.y = to;
        dep.row.localPosition = position;
        dep.posCoro = null;
    }

    IEnumerator animLength(DepartmentRow dep, float from, float to)
    {
        Vector2 size = dep.row.sizeDelta;
        size.x = from;
        dep.row.sizeDelta = size;
        float dt = 0.0f;
        while (dt < animTime)
        {
            size.x = Mathf.SmoothStep(from, to, dt / animTime);
            dep.row.sizeDelta = size;
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        size.x = to;
        dep.row.sizeDelta = size;
        dep.lengthCoro = null;
    }

    IEnumerator animScore(DepartmentRow dep, int from, int to)
    {

        dep.scoreCounter.text = String.Format("{0:N0}", from);
        float dt = 0.0f;
        while (dt < animTime)
        {
            dep.scoreCounter.text = String.Format("{0:N0}", Mathf.RoundToInt(Mathf.SmoothStep((float)from, (float)to, dt / animTime)));
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        dep.scoreCounter.text = String.Format("{0:N0}", to);
        dep.lastScore = to;
        dep.scoreCoro = null;
    }
}

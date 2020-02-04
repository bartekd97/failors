using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skin : MonoBehaviour
{
    [SerializeField]
    private Sprite skin;

    [SerializeField]
    private int pointCondition;

    [SerializeField]
    private SKINCONDITIONTYPE conditionType;

    [SerializeField]
    private string name;

    [SerializeField]
    private bool isUnlocked = false;

    public string Name { get => name; }
    public Sprite SkinSprite { get => skin; }
    public Text Description { get => description; set => description = value; }
    public GameObject IsActive { get => isActive; set => isActive = value; }

    private void Start()
    {
        TryToUnlockSkin();
    }

    public void TryToUnlockSkin()
    {
        if(conditionType == SKINCONDITIONTYPE.pointsInTotal)
        {
            if (PlayerPrefs.HasKey("TotalScore"))
            {
                if (PlayerPrefs.GetInt("TotalScore") >= pointCondition)
                {
                    Unlock();

                    PlayerPrefs.SetInt("Skin" + name, 1);
                    PlayerPrefs.Save();
                }
                else
                    description.text = "Get " + pointCondition.ToString("N0") + " points\nin total";
            }
            else
                description.text = "Get " + pointCondition.ToString("N0") + " points\nin total";
        }
        else if(conditionType == SKINCONDITIONTYPE.pointsRecord)
        {
            if (PlayerPrefs.HasKey("HighestScore"))
            {
                if (PlayerPrefs.GetInt("HighestScore") >= pointCondition)
                {
                    Unlock();

                    PlayerPrefs.SetInt("Skin" + name, 1);
                    PlayerPrefs.Save();
                }
                else
                    description.text = "Get " + pointCondition.ToString("N0") + " points\nin one game"; 
            }
            else
                description.text = "Get " + pointCondition.ToString("N0") + " points\nin one game";
        }
    }

    public void CheckIfIsUnlock()
    {
        if (PlayerPrefs.HasKey("Skin" + skin.name))
        {
            if (PlayerPrefs.GetInt("Skin" + skin.name) == 1)
            {
                Unlock();
            }
        }
    }

    [SerializeField]
    private Text description;
    [SerializeField]
    private Image spritePreview;
    [SerializeField]
    private GameObject isActive;

    private void Unlock()
    {
        isUnlocked = true;

        description.gameObject.SetActive(false);

        spritePreview.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public void SetAsActive()
    {
        if(isUnlocked)
        {
            SkinsManager.instance.SetActiveSkin(this);

            isActive.SetActive(true);
        }
    }
}

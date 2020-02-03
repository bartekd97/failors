using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinsManager : MonoBehaviour
{
    public static SkinsManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

    }

    [SerializeField]
    private List<Skin> skins = new List<Skin>();

    [SerializeField]
    private Skin activeSkin;

    [SerializeField]
    private SpriteRenderer skinRenderer;

    public Skin ActiveSkin { get => activeSkin; }

    private void Start()
    {
        SetActiveSkin();
        CheckSkinUnlocks();
    }

    private void CheckSkinUnlocks()
    {
        foreach(Skin skin in skins)
        {
            skin.CheckIfIsUnlock();

            if(PlayerPrefs.HasKey("activeskin") && PlayerPrefs.GetString("activeskin") == skin.name)
            {
                skin.IsActive.gameObject.SetActive(true);
            }
            else if(!PlayerPrefs.HasKey("activeskin") && skin.name == "default")
            {
                skin.IsActive.gameObject.SetActive(true);
            }
            else
                skin.IsActive.gameObject.SetActive(false);
        }
    }

    public void UpdateSkinsStatus()
    {
        foreach (Skin skin in skins)
        {
            skin.TryToUnlockSkin();
        }
    }

    private void SetActiveSkin()
    {
        if(PlayerPrefs.HasKey("activeskin"))
        {
            foreach(Skin skin in skins)
            {
                if(PlayerPrefs.GetString("activeskin") == skin.name)
                {
                    activeSkin = skin;

                    skinRenderer.sprite = activeSkin.SkinSprite;
                }
            }
        }
    }

    public void SetActiveSkin(Skin skin)
    {
        activeSkin.IsActive.gameObject.SetActive(false);

        activeSkin = skin;

        skinRenderer.sprite = activeSkin.SkinSprite;

        PlayerPrefs.SetString("activeskin", skin.name);
        PlayerPrefs.Save();
    }
}

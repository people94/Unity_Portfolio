using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public int prevEffectSound { get; set; }
    public int prevBgSound { get; set; }
    private int effectSound;
    private int bgSound;
    public static SystemManager instance;

    public int EffectSound
    {
        get
        {
            return effectSound;
        }

        set
        {
            effectSound = value;
        }
    }

    public int BgSound
    {
        get
        {
            return bgSound;
        }

        set
        {
            bgSound = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveSound()
    {
        PlayerPrefs.SetInt("bgSound", bgSound);
        PlayerPrefs.SetInt("effectSound", effectSound);
    }

    public void LoadSound()
    {
        bgSound = PlayerPrefs.GetInt("bgSound", 50);
        effectSound = PlayerPrefs.GetInt("effectSound", 50);
    }

    public void InitPrev()
    {
        prevBgSound = bgSound;
        prevEffectSound = effectSound;
    }
}

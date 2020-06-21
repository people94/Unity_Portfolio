using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel = null;
    [SerializeField] private GameObject optionPanel = null;

    public void OnSaveButton()
    {
        SystemManager.instance.SaveSound();
        optionPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OnCancelButton()
    {
        SystemManager.instance.BgSound = SystemManager.instance.prevBgSound;
        SystemManager.instance.EffectSound = SystemManager.instance.prevEffectSound;
        optionPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}

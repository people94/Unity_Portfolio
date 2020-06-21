using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionCtrl : MonoBehaviour
{
    public Slider bgSlider;
    public Slider effectSlider;
    public TextMeshProUGUI bgTxt;
    public TextMeshProUGUI effectTxt;

    private void OnEnable()
    {
        SystemManager.instance.LoadSound();
        SystemManager.instance.InitPrev();
        bgTxt.text = SystemManager.instance.BgSound.ToString();
        effectTxt.text = SystemManager.instance.EffectSound.ToString();
        bgSlider.value = (float)SystemManager.instance.BgSound;
        effectSlider.value = (float)SystemManager.instance.EffectSound;
    }

    public void ChangeBgSlider()
    {
        SystemManager.instance.BgSound = (int)bgSlider.value;
        bgTxt.text = ((int)bgSlider.value).ToString();
    }

    public void ChangeEffectSlider()
    {
        SystemManager.instance.EffectSound = (int)effectSlider.value;
        effectTxt.text = ((int)effectSlider.value).ToString();
    }
}

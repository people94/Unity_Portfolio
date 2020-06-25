using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanelButton : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject gamePanel;
    public GameObject infoPanel;

    public void OnGameButtonClick()
    {
        gamePanel.SetActive(true);
        infoPanel.SetActive(false);
    }

    public void OnInfoButtonClick()
    {
        gamePanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    public void OnExitButtonClick()
    {
        optionPanel.SetActive(false);
    }
}

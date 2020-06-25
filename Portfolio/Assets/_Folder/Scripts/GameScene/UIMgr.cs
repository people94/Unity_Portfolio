using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public GameObject menuGroup;
    public GameObject basicPanel;
    public GameObject statPanel;
    public GameObject skillPanel;
    public GameObject inventoryPanel;
    public GameObject optionPanel;
    private bool menuOpen = false;

    public void OnMenuButtonClick()
    {
        menuGroup.SetActive(!menuOpen);
        menuOpen = !menuOpen;
    }

    public void OnStatButtonClick()
    {
        menuGroup.SetActive(false);
        basicPanel.SetActive(false);
        statPanel.SetActive(true);
        skillPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void OnSKillButtonClick()
    {
        menuGroup.SetActive(false);
        basicPanel.SetActive(false);
        statPanel.SetActive(false);
        skillPanel.SetActive(true);
        inventoryPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    public void OnInventoryButtonClick()
    {
        menuGroup.SetActive(false);
        basicPanel.SetActive(false);
        statPanel.SetActive(false);
        skillPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        optionPanel.SetActive(false);
    }

    public void OnOptionButtonClick()
    {
        menuGroup.SetActive(false);
        basicPanel.SetActive(false);
        statPanel.SetActive(false);
        skillPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        optionPanel.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    [TextArea]
    public string dialogue;
    public Sprite cg;
}

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanel = null;
    [SerializeField] private Text dialogueText;
    [HideInInspector] public bool isDialogue = false;
    [HideInInspector] public int dialogueIdx = 0;
    public Dialogue[] dialogue;

    public void StartDialogue()
    {
        Time.timeScale = 0;
        DialogueOnOff(true);
        dialogueIdx = 0;
        NextDialogue();
    }

    private void DialogueOnOff(bool onoff)
    {
        dialoguePanel.SetActive(onoff);
        isDialogue = onoff;        
    }

    public void NextDialogue()
    {
        if (dialogueIdx == dialogue.Length) HideDialogue();
        dialogueText.text = dialogue[dialogueIdx++].dialogue;
    }

    public void HideDialogue()
    {
        Time.timeScale = 1;
        DialogueOnOff(false);
    }
    
}

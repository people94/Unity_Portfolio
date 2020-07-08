using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMap : MonoBehaviour
{
    #region "private"
    //대화 시스템에 필요
    private DialogueSystem ds;                                        //대화시스템객체
    #endregion

    #region "public"
    public GameObject nextBt;                                         //튜토리얼에서 다음 버튼
    public GameObject joystick;                                       //조이스틱
    public GameObject jump;                                           //점프 버튼
    public GameObject teleport;                                       //텔레포트 버튼
    public GameObject orbital;                                        //오비탈 버튼
    public GameObject blaze;                                          //블레이즈 버튼
    public GameObject cataclysm;                                      //카타클리즘 버튼
    [HideInInspector] public bool joystickGuide;                      //조이스틱 가이드 받았는지
    [HideInInspector] public bool jumpGuide = true;                          //점프 가이드 받았는지
    [HideInInspector] public bool teleportGuide;                      //텔레포트 가이드 받았는지
    [HideInInspector] public bool orbitalGuide;                       //Orbital 가이드 받았는지
    [HideInInspector] public bool blazeGuide;                         //blaze 가이드 받았는지
    [HideInInspector] public bool cataclysmGuide;                     //cataclysm 가이드 받았는지
    #endregion

    private void Start()
    {
        ds = GetComponent<DialogueSystem>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            joystick.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            joystick.transform.GetChild(0).GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
        }
    }

    public void PlayerEnter()
    {
        ds.StartDialogue();
    }

    public void ButtonAllStop()
    {
        //nextBt.GetComponent<Image>().raycastTarget = false;
        nextBt.SetActive(false);
        joystick.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
        joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
        jump.GetComponent<Image>().raycastTarget = false;
        teleport.GetComponent<Image>().raycastTarget = false;
        orbital.GetComponent<Image>().raycastTarget = false;
        blaze.GetComponent<Image>().raycastTarget = false;
        cataclysm.GetComponent<Image>().raycastTarget = false;
    }

    public void ButtonAllActive()
    {
        joystick.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        jump.GetComponent<Image>().raycastTarget = true;
        teleport.GetComponent<Image>().raycastTarget = true;
        orbital.GetComponent<Image>().raycastTarget = true;
        blaze.GetComponent<Image>().raycastTarget = true;
        cataclysm.GetComponent<Image>().raycastTarget = true;
    }

    public void OnClickNext()
    {
        if(SystemManager.instance.stageNum == SystemManager.Stage.Tutorial)
            ds.NextDialogue();
        if(ds.dialogueIdx == 5)
        {
            JoystickGuide();
        }
        else if (ds.dialogueIdx == 8)
        {
            //JumpGuide();
        }
        else if (ds.dialogueIdx == 10)
        {
            TeleportGuide();
        }
        else if (ds.dialogueIdx == 15)
        {
            OrbitalGuide();
        }
        else if (ds.dialogueIdx == 20)
        {
            BlazeGuide();
        }
        else if (ds.dialogueIdx == 23)
        {
            CataclysmGuide();
        }
        else if (ds.dialogueIdx == 26)
        {
            ButtonAllActive();
            ds.HideDialogue();
        }
    }

    IEnumerator GuideClear()
    {
        yield return new WaitForSeconds(0.8f);
        nextBt.SetActive(true);
        ds.NextDialogue();
    }

    public void JoystickGuide()
    {
        ButtonAllStop();
        joystick.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        StartCoroutine(JoystickFlick());
    }

    IEnumerator JoystickFlick()
    {
        while(true)
        {
            joystick.transform.GetChild(0).GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.2f);
            joystick.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            if(joystickGuide)
            {
                JoyStickClear();
                break;
            }
        }
    }

    public void JoyStickClear()
    {
        joystick.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
        joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
        joystick.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        StartCoroutine(GuideClear());
    }

    public void JumpGuide()
    {
        ButtonAllStop();
        jump.GetComponent<Image>().raycastTarget = true;
        StartCoroutine(JumpFlick());
    }

    IEnumerator JumpFlick()
    {
        while (true)
        {
            jump.transform.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.2f);
            jump.transform.GetComponent<Image>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            if(jumpGuide)
            {
                JumpClear();
                break;
            }
        }
    }

    public void JumpClear()
    {
        jump.transform.GetComponent<Image>().color = new Color(1, 1, 1);
        jump.GetComponent<Image>().raycastTarget = false;
        StartCoroutine(GuideClear());
    }

    public void TeleportGuide()
    {
        ButtonAllStop();
        teleport.GetComponent<Image>().raycastTarget = true;
        StartCoroutine(TeleportFlick());
    }

    IEnumerator TeleportFlick()
    {
        while (true)
        {
            teleport.transform.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.2f);
            teleport.transform.GetComponent<Image>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            if(teleportGuide)
            {
                TeleportClear();
                break;
            }
        }
    }

    public void TeleportClear()
    {
        teleport.transform.GetComponent<Image>().color = new Color(1, 1, 1);
        teleport.GetComponent<Image>().raycastTarget = false;
        StartCoroutine(GuideClear());
    }
    
    public void OrbitalGuide()
    {
        ButtonAllStop();
        orbital.GetComponent<Image>().raycastTarget = true;
        StartCoroutine(OrbitalFlick());
    }

    IEnumerator OrbitalFlick()
    {
        while (true)
        {
            orbital.transform.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.2f);
            orbital.transform.GetComponent<Image>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            if(orbitalGuide)
            {
                OrbitalClear();
                break;
            }
        }
    }

    public void OrbitalClear()
    {
        orbital.transform.GetComponent<Image>().color = new Color(1, 1, 1);
        orbital.GetComponent<Image>().raycastTarget = false;
        StartCoroutine(GuideClear());
    }

    public void BlazeGuide()
    {
        ButtonAllStop();
        blaze.GetComponent<Image>().raycastTarget = true;
        StartCoroutine(BlazeFlick());
    }

    IEnumerator BlazeFlick()
    {
        while (true)
        {
            blaze.transform.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.2f);
            blaze.transform.GetComponent<Image>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            if(blazeGuide)
            {
                BlazeClear();
                break;
            }
        }
    }

    public void BlazeClear()
    {
        blaze.transform.GetComponent<Image>().color = new Color(1, 1, 1);
        blaze.GetComponent<Image>().raycastTarget = false;
        StartCoroutine(GuideClear());
    }

    public void CataclysmGuide()
    {
        ButtonAllStop();
        cataclysm.GetComponent<Image>().raycastTarget = true;
        StartCoroutine(CataclysmFlick());
    }

    IEnumerator CataclysmFlick()
    {
        while (true)
        {
            cataclysm.transform.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            yield return new WaitForSeconds(0.2f);
            cataclysm.transform.GetComponent<Image>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            if(cataclysmGuide)
            {
                CataclysmClear();
                break;
            }
        }
    }

    public void CataclysmClear()
    {
        cataclysm.transform.GetComponent<Image>().color = new Color(1, 1, 1);
        cataclysm.GetComponent<Image>().raycastTarget = false;
        StartCoroutine(GuideClear());
    }
}

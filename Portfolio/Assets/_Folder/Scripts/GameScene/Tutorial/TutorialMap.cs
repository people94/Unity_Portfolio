using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMap : MonoBehaviour
{
    #region "대화시스템"
    //대화 시스템에 필요
    private DialogueSystem ds;                                        //대화시스템객체
    #endregion

    #region "튜토리얼"
    public GameObject player;                                         //플레이어
    public GameObject nextBt;                                         //튜토리얼에서 다음 버튼
    public GameObject joystick;                                       //조이스틱
    public GameObject jump;                                           //점프 버튼
    public GameObject teleport;                                       //텔레포트 버튼
    public GameObject orbital;                                        //오비탈 버튼
    public GameObject blaze;                                          //블레이즈 버튼
    public GameObject cataclysm;                                      //카타클리즘 버튼
    [HideInInspector] public bool joystickGuide;                      //조이스틱 가이드 받았는지
    [HideInInspector] public bool jumpGuide = true;                   //점프 가이드 받았는지
    [HideInInspector] public bool teleportGuide;                      //텔레포트 가이드 받았는지
    [HideInInspector] public bool orbitalGuide;                       //Orbital 가이드 받았는지
    [HideInInspector] public bool blazeGuide;                         //blaze 가이드 받았는지
    [HideInInspector] public bool cataclysmGuide;                     //cataclysm 가이드 받았는지
    #endregion

    #region "카메라이동 및 포탈"
    public GameObject portalPref;                                     //맵클리어시 생성할 포탈프리팹
    public GameObject portalPos;                                      //포탈 생성위치
    public GameObject cameraPos;                                      //카메라 연출시 카메라 위치
    private Vector3 prevCameraPos;                                    //원래 카메라 위치
    private Quaternion prevCameraRot;                                 //원래 카메라 각도
    #endregion

    private void Start()
    {
        //대화 시스템 컴포넌트
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

    //플레이어 들어오면 실행
    public void PlayerEnter()
    {
        //대화시작
        ds.StartDialogue();
    }

    //모든 버튼 비활성화
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

    //모든 버튼 활성화
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

    //다음버튼 누르면 다음 대화실행 인덱스에 따라 맞는 버튼 점멸기능
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
            JumpGuide();
        }
        else if (ds.dialogueIdx == 11)
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
            CreatePortal();
        }
    }

    //상황에 맞는 버튼 누르면 다음버튼 활성화
    IEnumerator GuideClear()
    {
        yield return new WaitForSeconds(0.8f);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        nextBt.SetActive(true);
        ds.NextDialogue();
    }

    //조이스틱 버튼 누르기 전까지 조이스틱 버튼을 제외한 모든 버튼 비활성화 이후 조이스틱 버튼 점멸기능하는 코루틴함수 실행
    public void JoystickGuide()
    {
        ButtonAllStop();
        joystick.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        joystick.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        StartCoroutine(JoystickFlick());
    }

    //조이스틱 누를때 까지 버튼 점멸하는 기능
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

    //조이스틱 누르면 버튼 원래대로 돌리고 다음버튼 활성화시키는 코루틴함수 실행
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

    //포탈 생성
    public void CreatePortal()
    {
        //포탈 프리팹 생성
        GameObject portal = Instantiate(portalPref, portalPos.transform.position, Quaternion.Euler(0, 90, 0), this.transform);
        //플레이어 카메라 이동하는 연출
        prevCameraPos = Camera.main.transform.position;
        prevCameraRot = Camera.main.transform.rotation;
        Camera.main.GetComponent<CameraMove>().enabled = false;
        Camera.main.transform.position = cameraPos.transform.position;
        Camera.main.transform.LookAt(portal.transform);
        ReturnCamera();
    }

    public void ReturnCamera()
    {
        StartCoroutine(ReturnCameraProc());
    }

    //카메라 정상위치로 돌리는 함수(포탈에서 실행)
    IEnumerator ReturnCameraProc()
    {
        yield return new WaitForSeconds(3.0f);
        Camera.main.transform.position = prevCameraPos;
        Camera.main.transform.rotation = prevCameraRot;
        Camera.main.GetComponent<CameraMove>().enabled = true;
    }
}

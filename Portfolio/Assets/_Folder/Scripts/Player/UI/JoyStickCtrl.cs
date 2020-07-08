using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickCtrl : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    #region "조이스틱"
    public RectTransform backGround;        //조이스틱 배경
    public RectTransform handle;            //조이스틱 핸들
    private float radius;                   //조이스틱 배경 반지름
    #endregion

    #region "플레이어 이동 및 회전"
    public GameObject player;               //플레이어 게임오브젝트
    private PlayerMove pc;                  //플레이어 무브하는 클래스
    private float distance;                 //조이스틱 배경과 핸들사이의 거리차
    private float angle;                    //조이스틱 배경과 마우스 사이의 각도
    #endregion

    //튜토리얼에서 조이스틱 작동 했는지
    public GameObject tutorialMap;
    private bool tutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        radius = backGround.rect.width / 2;
        pc = player.GetComponent<PlayerMove>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pc.isTouch = false;
        //터치떼면 핸들 원점으로 돌아오도록
        handle.localPosition = Vector3.zero;
        //터치떼면 이전 속도 초기화
        pc.movePos = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!tutorial)
        {
            tutorialMap.GetComponent<TutorialMap>().joystickGuide = true;
            tutorial = true;
        }
        pc.movePos = Vector3.zero;
        pc.rot = player.transform.rotation;
        pc.isTouch = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //eventData.position => 마우스 위치
        //value => 이동방향을 구하기 위해
        //마우스 위치 - 백그라운드 위치 = 백그라운드에서 마우스를 바라보는 벡터
        Vector2 value = eventData.position - (Vector2)backGround.position;

        //value 값을 -radius ~ radius 만큼으로 제한한다.
        value = Vector2.ClampMagnitude(value, radius);

        handle.localPosition = value;

        value = value.normalized;

        //조이스틱이 중심에 가까우면 천천히 멀면 빠르게 움직이도록 하기 위해
        //배경과 핸들 사이의 거리를 이용하여 보간해줌
        //distacne 값은 0~1 사이의 값이 나옴
        distance = Vector2.Distance(backGround.position, handle.position) / radius;

        //플레이어가 움직이는 방향
        //pc.movePos = new Vector3(value.x * distance, 0.0f, value.y * distance);
        pc.vertical = value.y * distance;
        pc.horizontal = value.x * distance;

        //플레이어의 회전 값
        //atan을 이용해서 각도를 구함
        //value.x = 벡터의 x 방향
        //value.y = 벡터의 y 방향
        //atan의 결과값은 라디안 이므로 라디안->디그리로 변경 해줘야함.
        //mathf.Rad2Deg = > 상수를 반환하므로 곱하기 해서 디그리로 변경
        //pc.rot.eulerAngles = new Vector3(0, Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg, 0);

        //LookRotation - 플레이어가 움직이는 방향의 회전 값을 구해준다
        //rot = Quaternion.LookRotation(movePos.normalized);
    }
}

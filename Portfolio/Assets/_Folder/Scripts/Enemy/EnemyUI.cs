using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas uiCanvas;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    // Start is called before the first frame update
    void Start()
    {
        uiCanvas = GetComponentInParent<Canvas>();
        uiCamera = uiCanvas.worldCamera;
        rectParent = uiCanvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        //월드좌표계를 스크린에서 사용할 수 있도록 변환
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        if(screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
            //WorldToScreenPoint의 반환값의 z값은 메인 카메라에서 XY평면까지의 거리
            //뒤를 돌았을 때 hpbar가 안보이게 하도록 하는 장치
            //뒤를 돌면 카메라랑 거리가 음수로 나온다
            //z값이랑 상관없이 x, y는 양수기 때문에 뒤돌아 있는 상태에서도 ui가 보이게 된다.
            //따라서 뒤를 돌아 있는 상태에선 screenPos에 -1을 곱해 x, y를 음수값으로 하여 안보이게 하는것이다.
        }

        var localPos = Vector2.zero;
        //스크린좌표로 되어 있는 것을 UI캔버스 좌표로 옮겨준다.
        //매개변수는 부모의 렉트 트랜스폼, 스크린좌표, 렌더링카메라, 변환된 좌표를 저장할 변수
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        rectHp.localPosition = localPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject target = null;               //카메라가 따라다닐 타겟
    //public float angle;                            //카메라가 타겟 바라볼때 얼만큼 회전할지
    public float height;                           //카메라가 타겟보다 얼마나 위에 있을지
    public float distance;                         //카메라가 타겟이랑 얼마나 떨어져 있을지
    public float moveDamping;                      //카메라가 타겟 따라다니는 속도
    public float rotateDamping = 10.0f;             //회전 속도 계수
    public float targetOffset = 2.0f;       //추적 좌표의 오프셋

    private void LateUpdate()
    {
        if (!target)
            return;

        //transform.rotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        //Vector3 dir = target.transform.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(dir);
        ////var position = transform.position;
        ////position.z = target.transform.position.z - distance;
        ////position.y = Mathf.Lerp(position.y, target.transform.position.y + height, moveDamping * Time.deltaTime);
        ////position.x = Mathf.Lerp(position.x, target.transform.position.x, moveDamping * Time.deltaTime);
        ////this.transform.position = position;
        //
        //this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + height, target.transform.position.z) + target.transform.forward * -1 * distance;

        //카메라의 높이와 거리를 계산
        var camPos = target.transform.position - (target.transform.forward * distance) + (target.transform.up * height);

        //이동할 때의 속도 계수를 적용
        transform.position = Vector3.Slerp(transform.position, camPos, Time.deltaTime * moveDamping);
        //회전할 때의 속도 계수를 적용
        transform.rotation = Quaternion.Slerp(transform.rotation, target.transform.rotation, Time.deltaTime * rotateDamping);
        //카메라를 추적 대상으로 Z축을 회전시킴
        transform.LookAt(target.transform.position + (target.transform.up * targetOffset));

        //transform.LookAt(target.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject target = null;               //카메라가 따라다닐 타겟
    public float angle;                            //카메라가 타겟 바라볼때 얼만큼 회전할지
    public float height;                           //카메라가 타겟보다 얼마나 위에 있을지
    public float distance;                         //카메라가 타겟이랑 얼마나 떨어져 있을지
    public float moveDamping;                      //카메라가 타겟 따라다니는 속도

    private void LateUpdate()
    {
        if (!target)
            return;

        transform.rotation = Quaternion.Euler(angle, 0.0f, 0.0f);
        var position = transform.position;
        position.z = target.transform.position.z - distance;
        position.y = Mathf.Lerp(position.y, target.transform.position.y + height, moveDamping * Time.deltaTime);
        position.x = Mathf.Lerp(position.x, target.transform.position.x, moveDamping * Time.deltaTime);
        this.transform.position = position;

        //transform.LookAt(target.transform);
    }
}

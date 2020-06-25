using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController cc;
    public float speed = 0.0f;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        speed = 5.0f;
    }

    public void Move(Vector3 dir)
    {
        cc.SimpleMove(dir * speed);
        //애니메이션 플레이
    }
}

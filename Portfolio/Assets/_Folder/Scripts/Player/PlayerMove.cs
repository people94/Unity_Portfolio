using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Animator anim;
    private CharacterController cc;
    public float speed = 5.0f;                               //플레이어 이동속도
    public float teleportDis = 20.0f;
    [HideInInspector] public bool isTouch = false;           //터치 중인지
    public float rotSpeed = 5.0f;
    [HideInInspector] public Vector3 movePos;                //플레이어 이동 방향(포지션)
    [HideInInspector] public Quaternion rot;                 //플레이어 회전값(회전)
    public GameObject teleportPref;                         //텔레포트 프리팹

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(isTouch)
        {
            Move();
            Rotate();
        }
        else
        {
            Idle();
        }
    }

    public void Move()
    {
        anim.SetBool("Idle", false);
        cc.SimpleMove(movePos * speed);
        //애니메이션 플레이
        if (movePos.magnitude <= 0.7)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetTrigger("Run");
        }
    }

    public void Rotate()
    {
        Quaternion curRot = Quaternion.LookRotation(transform.forward);
        transform.rotation = rot;
        //transform.rotation = dir;
    }

    public void Idle()
    {
        anim.SetBool("Idle", true);
        anim.SetBool("Walk", false);
    }

    public void UsePortal(Vector3 destPos)
    {
        this.transform.position = destPos;
    }

    public void Teleport()
    {
        //transform.Translate(transform.forward * teleportDis, Space.World);
        StartCoroutine(DoTeleport());
        anim.SetTrigger("Teleport");
        
    }

    IEnumerator DoTeleport()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject startTeleport = Instantiate(teleportPref, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), transform.rotation);
        transform.Translate(Vector3.forward * teleportDis);
        Destroy(startTeleport, 1.0f);
        GameObject destTeleport = Instantiate(teleportPref, new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z), transform.rotation);
        Destroy(destTeleport, 1.0f);
    }
}

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
    public GameObject teleportPref2;
    private bool isMove = false;                                    //이동중인지

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
       
        Move();
        Rotate();
        
        if(!isMove)
            Idle();       
    }

    public void Move()
    {        
        if (isTouch)
        {
            isMove = true;
            anim.SetBool("Idle", false);
            //transform.rotation =Quaternion.LookRotation( movePos);
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
        else
        {
            isMove = false;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (h != 0 || v != 0)
            {
                isMove = true;
                movePos = (Vector3.forward * v) + (Vector3.right * h);
                transform.Translate(movePos.normalized * speed * Time.deltaTime, Space.Self);

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
                //movePos.Normalize();
                //cc.SimpleMove(movePos * speed);
                anim.SetBool("Idle", false);
                transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(movePos), Time.deltaTime * 5.0f);                           
            }
            else
            {
                isMove = false;
            }
        }
    }

    public void Rotate()
    {
        if (isTouch)
        {
            //Quaternion curRot = Quaternion.LookRotation(transform.forward);

            transform.rotation = rot;
            //transform.rotation = dir;
        }
        else
        {
            
        }
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
        GameObject startTeleport2 = Instantiate(teleportPref2, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
        transform.Translate(Vector3.forward * teleportDis);
        Destroy(startTeleport2, 1.0f);
        GameObject destTeleport2 = Instantiate(teleportPref2, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);        
        Destroy(destTeleport2, 1.0f);
    }
}

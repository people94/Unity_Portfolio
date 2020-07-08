using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region "애니메이션"
    private Animator anim;                                   //애니메이션 컴포넌트
    private bool isMove = false;                                    //이동중인지
    #endregion

    #region "이동"
    [HideInInspector] public bool isTouch = false;           //터치 중인지
    [HideInInspector] public Vector3 movePos;                //플레이어 이동 방향(포지션)
    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    public float speed = 5.0f;                               //플레이어 이동속도
    #endregion

    #region "회전"
    [HideInInspector] public Quaternion rot;                 //플레이어 회전값(회전)
    public float rotSpeed = 10.0f;
    #endregion

    #region "텔레포트"
    public float teleportDis = 20.0f;                        //텔레포트 거리
    public GameObject teleportPref2;                         //텔레포트 이펙트
    //튜토리얼에서 텔레포트 작동 했는지
    public GameObject tutorialMap;
    private bool teleport = false;
    #endregion

    #region "점프"
    Rigidbody rb;                                            //리지드바디 컴포넌트
    Ray jumpRay;                                            //캐릭터가 바닥에 붙어있는지 체크하기 위함
    RaycastHit hitInfo;
    public float rayDist = 0.15f;               
    private float jumpHeight = 10;                                //점프 높이
    private int maxJump = 2;                                     //최대로 수행할 수 있는 점프횟수
    private int jumpCnt = 0;                                     //점프 몇번 했는지(더블점프)
    private bool isJump = false;                             //현재 점프중인지
    private bool jump = false;                               //튜토리얼 점프 수행했는지
    private float curJumpTime = 0.0f;                        //현재 몇초동안 점프중인지
    private float jumpTime = 0.8f;                           //몇초만큼 점프할 수 있는지
    private bool isGround = false;
    #endregion

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();        
    }

    private void Update()
    {
        jumpRay.origin = this.transform.position + new Vector3(0,1f,0);        
        jumpRay.direction = Vector3.down;
        if (jumpCnt < 1 && Input.GetKeyDown(KeyCode.Space))
        {
            jumpCnt++;
           // anim.SetBool("Jump", true);
            isJump = true;
            curJumpTime = 0.0f;
            //rb.useGravity = false;
            jumpHeight = 10.0f;
            Debug.Log("점프!");
        }

        if (Physics.Raycast(jumpRay.origin, jumpRay.direction, out hitInfo, rayDist))
        {
            isGround = true;
            jumpCnt = 0; 
        }
        else
        {
            isGround = false;
        }

        if (isJump)
        {
            Jump();
        }
        if(!isJump && !isGround)
        { 
            this.transform.Translate(Vector3.down * 5 * Time.deltaTime);
        }

        Debug.DrawRay(jumpRay.origin , Vector3.down * 1.2f, Color.red);

        Debug.Log(isGround);

        Move();
        Rotate();        
        
        if(vertical == 0)
            Idle();       
    }

    public void Jump()
    {
        if(!jump)
        {
            tutorialMap.GetComponent<TutorialMap>().jumpGuide = true;
            jump = true;
        }
        this.transform.Translate(Vector3.up * jumpHeight * Time.deltaTime);
        curJumpTime += Time.deltaTime;
        if (curJumpTime >= jumpTime)
        {
            isJump = false;
            //rb.useGravity = true;
        }

        //jumpHeight -= 3 * Time.deltaTime;
        
    }

    public void Move()
    {        
        if (!isTouch)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        movePos = new Vector3(0, 0, vertical);
        //내 기준(로컬좌표계)로 되어 있는 것을 월드 좌표기준으로 바꿔준다
        //예를 들어 나의 회전이 y축을 기준으로 90도이면 forward방향(0,0,1)을 월드기준(1,0,0)으로 바꿔준다.
        //movePos = transform.TransformDirection(movePos);
        transform.Translate(movePos * speed * Time.deltaTime);

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
    }

    public void Rotate()
    {
        if (horizontal != 0)
        {
            float temp = horizontal;
            if (vertical < 0)
                temp = -horizontal;
            transform.Rotate(0, temp * rotSpeed, 0);
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
        if (!teleport)
        {
            tutorialMap.GetComponent<TutorialMap>().teleportGuide = true;
            teleport = true;
        }
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
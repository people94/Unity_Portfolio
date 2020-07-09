using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region "애니메이션"
    private Animator anim;                                   //애니메이션 컴포넌트
    private bool isMove = false;                                    //이동중인지
    private bool isAttack = false;                           //공격 중인지
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
    Ray jumpRay;                                             //캐릭터가 바닥에 붙어있는지 체크하기 위함
    RaycastHit hitInfo;
    public float rayDist = 0.15f;               
    private float jumpHeight = 10;                           //점프 높이
    private float gravity = 0.0f;                            //중력(리지드바디 중력 + 스크립트 중력)
    private int maxJump = 2;                                 //최대로 수행할 수 있는 점프횟수
    private int jumpCnt = 0;                                 //점프 몇번 했는지(더블점프)
    private bool isJump = false;                             //현재 점프중인지
    private float curJumpTime = 0.0f;                        //현재 몇초동안 점프중인지
    private float jumpTime = 0.5f;                           //몇초만큼 점프할 수 있는지
    private bool isGround = false;                           //땅에 붙었는지
    private bool jump = false;                               //튜토리얼 점프 수행했는지
    private bool doubleJump = false;                               //튜토리얼 점프 수행했는지    
    #endregion

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        jumpRay.direction = Vector3.down;
        isAttack = GetComponent<PlayerAttack>().isAttack;
    }

    private void Update()
    {
        //이동 및 회전을 위한 입력 받기
        if (!isTouch)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        //캐릭터 기준으로 바닥레이저 시작위치 갱신
        jumpRay.origin = this.transform.position + new Vector3(0,1f,0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            Jump();
        }

        //점프상태면 점프시간 계산하고 위쪽 방향으로 이동
        if (isJump)
            DoJump();

        //바닥으로 레이저쏴서 땅에 붙어있는지 검출 & 점프 애니메이션 꺼준다.
        if (Physics.Raycast(jumpRay.origin, jumpRay.direction, out hitInfo, rayDist,1<<10))
        {
            isGround = true;
            anim.SetBool("Jump", false);
            anim.SetBool("DoubleJump", false);
            //anim.SetBool("Idle", true);
            jumpCnt = 0; 
        }
        else
        {
            isGround = false;
        }        
        
        //땅에 붙어있지않으면 밑으로 중력계속 적용
        if(!isJump && !isGround)
        {
            this.transform.Translate(Vector3.down * 3 * Time.deltaTime);
        }

        //디버깅용
        Debug.DrawRay(jumpRay.origin , Vector3.down * 1.2f, Color.red);
        Debug.Log(isGround);             
        
        //앞뒤 입력 없으면 기본상태
        if(vertical == 0)
            Idle();
        //입력 있을때만 이동
        else
        {
            Move();
        }

        //회전 입력있으면 회전
        if(horizontal != 0)
            Rotate();
    }

    //스페이스누르면 점프
    public void Jump()
    {
        if (!jump)
        {
            tutorialMap.GetComponent<TutorialMap>().jumpGuide = true;
            jump = true;
        }

        if (jumpCnt < maxJump - 1)
        {
            jumpCnt++;

            if (!isAttack)
            {
                anim.SetBool("Jump", true);
                anim.SetBool("Idle", false);
                anim.SetBool("Walk", false);
            }

            isJump = true;
            curJumpTime = 0.0f;
            //rb.useGravity = false;
            jumpHeight = 10.0f;
            this.transform.Translate(Vector3.up * 0.5f);
            Debug.Log("점프!");
            if (isJump && jumpCnt == 1 && !isAttack)
            {
                Debug.Log("더블 점프!");
                anim.SetBool("DoubleJump", true);
                anim.SetBool("Jump", false);
            }
        }
    }

    //점프상태면 점프시간 계산하고 위쪽 방향으로 이동
    public void DoJump()
    {
        this.transform.Translate(Vector3.up * jumpHeight * Time.deltaTime);
        Debug.Log("JumpCnt : " + jumpCnt);
        curJumpTime += Time.deltaTime;
        if (curJumpTime >= jumpTime)
        {
            isJump = false;
        }
    }

    public void Move()
    {
        movePos = new Vector3(0, 0, vertical);
        //내 기준(로컬좌표계)로 되어 있는 것을 월드 좌표기준으로 바꿔준다
        //예를 들어 나의 회전이 y축을 기준으로 90도이면 forward방향(0,0,1)을 월드기준(1,0,0)으로 바꿔준다.
        //movePos = transform.TransformDirection(movePos);
        transform.Translate(movePos * speed * Time.deltaTime);

        if (isGround && !isAttack)
        {
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
    }

    public void Rotate()
    {
        float temp = horizontal;
        if (vertical < 0)
            temp = -horizontal;
        transform.Rotate(0, temp * rotSpeed, 0);
    }

    public void Idle()
    {
        if (isGround && !isAttack)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Walk", false);
        }
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
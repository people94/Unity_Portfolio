using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//몬스터 유한상태머신
public class EnemyFSM : MonoBehaviour
{
    //몬스터 상태 이넘문
    protected enum EnemyState
    {
        Idle, Patrol, Move, Attack, Return, Damaged, Die
    }

    #region "private"
    [SerializeField] private GameObject hpBarPref;    //hp바 프리팹
    private Canvas uiCanvas;
    private Image hpBarImage;
    
    //공격 딜레이
    float attTime = 2.0f;           //2초에 한번 공격
    float timer = 0.0f;             //타이머
    IEnumerator decreaseHP;         //에너미 피깎는 코루틴변수
    #endregion

    #region "public"
    public Vector3 hpBarOffset = new Vector3(0, 2.2f, 0);
    public List<Transform> wayPoints;                   //순찰 하는 지점
    #endregion

    #region "proteced"
    [HideInInspector] protected float findRange;   //플레이어를 찾는 범위
    [HideInInspector] protected float moveRange;   //시작지점에서 최대 이동가능한 번위
    [HideInInspector] protected float attackRange;  //공격 가능 범위
    protected int nextIdx;                                 //다음 순찰지점 인덱스
    protected NavMeshAgent nav;                         //네비게이션 위해
    protected EnemyState state;
    protected Vector3 startPoint;             //몬스터 시작위치
    protected Quaternion startRotation;       //몬스터 시작회전값
    protected Vector3 destPos;                //도착 지점
    protected GameObject player;              //플레이어 찾기 위해
    protected EnemyCounter enemyCnt;
    protected float maxHp = 100;              //전체 체력
    protected float hp = 100;                 //체력
    protected int att = 5;                    //공격력
    protected float speed = 5.0f;             //이동속도
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        //몬스터 상태 초기화
        state = EnemyState.Idle;

        //시작지점 저장
        startPoint = transform.position;
        startRotation = transform.rotation;
        //플레이어 위치 찾기
        player = GameObject.Find("Player");
       
        //애니메이터 컴포넌트 찾기
        //anim = GetComponentInChildren<Animator>();

        nav = GetComponent<NavMeshAgent>();

        SetHpBar();

        enemyCnt = GetComponentInParent<EnemyCounter>();

        //하이러키 뷰의 WayPointGroup 게임오브젝트를 추출
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            //WayPointGroup 하위에 있는 모든 Transform 컴포넌트를 추출한 후
            //List 타입의 wayPints 배열에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);
            Debug.Log("WayPoint : " + wayPoints.Count);
            //이것도 가능
            //Transform[] ways = group.GetComponentsInChildren<Transform>();
            //wayPoints.AddRange(ways);
            //배열의 첫 번째 항목 삭제
            wayPoints.RemoveAt(0);

            //첫 번째로 이동할 위치를 불규칙하게 추출
            nextIdx = Random.Range(0, wayPoints.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //상태에 따른 행동처리
        switch (state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:
                break;
            default:
                break;
        }//end of switch
    }//end of void Update()

    private void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPref, uiCanvas.transform);
        //0번째에는 자기 자신이 저장되어 있기 때문에 1번째를 가져옴
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpbar = hpBar.GetComponent<EnemyUI>();
        _hpbar.targetTr = this.gameObject.transform;
        _hpbar.offset = hpBarOffset;
    }

    //대기상태
    public virtual void Idle()
    {
        Debug.Log("EnemyFSM : Idle");
        
    }

    public virtual void Patrol()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < findRange)
        {
            state = EnemyState.Move;
            //print("상태전환 : Idle -> Move");

            //애니메이션 상태 변환하기
            //anim.SetTrigger("Move");
        }
        else
        {
            state = EnemyState.Patrol;
            nav.destination = wayPoints[nextIdx].position;

            if (Vector3.Distance(gameObject.transform.position, wayPoints[nextIdx].position) < 0.1f)
            {
                //다음 목적지의 배열 첨자를 계산
                //nextIdx = ++nextIdx % wayPoints.Count;
                nextIdx = Random.Range(0, wayPoints.Count);
            }
        }
    }

    //플레이어 추격 상태
    public virtual void Move()
    {
        //이동중 이동할 수 있는 최대범위에 들어왔을때
        if (Vector3.Distance(transform.position, startPoint) > moveRange)
        {
            state = EnemyState.Return;
            //print("상태전환 : Move -> Return");

            //애니메이션
            //anim.SetTrigger("Return");
        }
        //리턴상태가 아니면 플레이어를 추격해야 한다
        else if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            nav.destination = player.transform.transform.position;
        }
        else//공격범위 안에 들어옴
        {
            state = EnemyState.Attack;
            print("상태전환 : Move -> Attack");

            //애니메이션
            //anim.SetTrigger("Attack");
        }
    }

    //공격 상태
    public virtual void Attack()
    {
        //공격범위안에 들어옴
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            //일정 시간마다 플레이어를 공격하기
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                print("공격");
                //플레이어의 필요한 스크립트 컴포넌트를 가져와서 데미지를 주면 된다.
                //player.GetComponent<PlayerMove>().hitDamage(att);

                //타이머 초기화
                timer = 0.0f;

                //애니메이션
                //anim.SetTrigger("Attack");
            }
        }
        else//현재 상태를 무브로 전환하기 (재추격)
        {
            state = EnemyState.Move;
            print("상태전환 : Attack -> Move");
            timer = 0.0f;

            //애니메이션
            //anim.SetTrigger("Move");
        }
    }

    //복귀상태
    public virtual void Return()
    {
        //시작위치까지 도달하지 않을때는 이동
        //도착하면 대기상태로 변경
        if (Vector3.Distance(transform.position, startPoint) > 0.1f)
        {
            //Vector3 dir = (startPoint - transform.position).normalized;
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
            //cc.SimpleMove(dir * speed);
            nav.destination = startPoint;
        }
        else
        {
            //위치값,회전값을 초기값으로
            transform.position = startPoint;
            //transform.rotation = startRotation;
            transform.rotation = Quaternion.identity;   //시작 회전값 0으로 초기화
            state = EnemyState.Idle;
            print("상태전환 : Return -> Idle");

            //애니메이션
            //anim.SetTrigger("Idle");
        }
    }

    //플레이어쪽에서 충돌감지를 할 수 있으니 이 함수는 퍼블릭으로 만들자
    public void HitDamage(int value)
    {
        //예외처리
        //피격상태이거나, 죽은 상태일때는 데미지 중/첩으로 주지 않는다.
        if (state == EnemyState.Damaged || state == EnemyState.Die) return;

        //에너미 피깎기
        decreaseHP = DecreaseHP(value);
        StartCoroutine(decreaseHP);

        //몬스터의 체력이 1이상이면 피격상태
        if (hp > 0)
        {
            state = EnemyState.Damaged;
            print("상태전환 : Any State -> Damaged");
            print("HP : " + hp);

            Damaged();

            //anim.SetTrigger("Damaged");
        }
        else//0이하이면 죽음상태
        {
            state = EnemyState.Die;
            print("상태전환 : Any State -> Die");

            hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;

            Die();

            //anim.SetTrigger("Die");
        }
    }

    //피격상태 (Any State)
    public virtual void Damaged()
    {
        //피격 상태를 처리하기 위한 코루틴을 실행한다
        StartCoroutine(DamageProc());
    }

    //피격상태 처리용 코루틴
    IEnumerator DamageProc()
    {
        //피격모션 시간만큼 기다리기
        yield return new WaitForSeconds(1.0f);
        //현재상태를 이동으로 전환
        state = EnemyState.Move;
        print("상태전환 : Damaged -> Move");
        //anim.SetTrigger("Move");
    }

    //에너미 hp바 줄어들이기용
    IEnumerator DecreaseHP(float value)
    {
        float targetHp = hp - value;
        while (true)
        {
            hp -= value * Time.deltaTime * 2;
            hpBarImage.fillAmount = hp / maxHp;
            yield return null;
            if(targetHp > hp)
            {
                hp = targetHp;
                hpBarImage.fillAmount = hp / maxHp;
                StopCoroutine(decreaseHP);
                break;
            }            
        }
    }

    //죽음상태 (Any State)
    public virtual void Die()
    {
        //진행중인 모든 코루틴은 정지한다
        StopAllCoroutines();

        //죽음상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DieProc());

        enemyCnt.enemyCnt--;
        if(enemyCnt.enemyCnt == 0)
        {
            GetComponentInParent<EnemyCounter>().PortalOpen();
        }
    }

    IEnumerator DieProc()
    {
        //2초후에 자기자신을 제거한다
        yield return new WaitForSeconds(2.0f);
        print("죽음");
        Destroy(this.gameObject);
    }
    
    private void OnDrawGizmos()
    {
        //공격 가능 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //플레이어 찾을 수 있는 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);

        //이동가능한 최대 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }
}

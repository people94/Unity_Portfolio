using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    private enum NormalAttackState
    {
        NoAttack, NormalRAttack, NormalLAttack, Normal2HAttack
    }
    private GameObject target = null;           //타겟
    private RaycastHit hitInfo;                 //레이캐스트 히트 정보
    public LayerMask enemyMask;                 //타겟 설정할 때 타겟이 될 수있는 오브젝트 설정하기 위해

    public GameObject firePos = null;           //기본 공격이 시작되는 위치
    public float normalAttackSpeed = 5.0f;      //기본 공격 속도
    public float bounceAttackSpeed = 10.0f;     //튕기는 공격 속도
    private Animator anim = null;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
       
;   }

    public void SetTarget()
    {
        //현재 마우스 클릭이 UI이면 true를 아니면 false를 반환하는 함수
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //0번 - 왼쪽 1번 - 오른쪽 2번 - 가운데?
        if (Input.GetMouseButton(0))
        {
            //마우스로 클릭한 물체가 무엇인지 판별하는 코드
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 999f, enemyMask))
            {
                if (target != null)
                    target.GetComponent<Outline>().enabled = false;
                target = hitInfo.collider.gameObject;
                target.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if (target != null)
                {
                    target.GetComponent<Outline>().enabled = false;
                    target = null;
                }            
            }
        }
    }

    public void OnClickNormalButton()
    {
        
    }
    
    //NormalR -> NormalL -> 2HAttack
    public void NormalRAttack()
    {
        if (target == null)
        {
            NormalAttack normal = NormalObjectPool.instance.PopNormal();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.gameObject.SetActive(true);
            normal.speed = normalAttackSpeed;
            normal.dir = transform.forward;
        }
        else
        {
            this.transform.LookAt(target.transform);
            NormalAttack normal = NormalObjectPool.instance.PopNormal();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.gameObject.SetActive(true);
            normal.target = target;
            normal.speed = normalAttackSpeed;            
        }
    }

    //NormalR -> NormalL -> 2HAttack
    public void NormalLAttack()
    {
        if (target == null)
        {
            NormalAttack normal = NormalObjectPool.instance.PopNormal();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.gameObject.SetActive(true);
            normal.speed = normalAttackSpeed;
            normal.dir = transform.forward;
        }
        else
        {
            this.transform.LookAt(target.transform);
            NormalAttack normal = NormalObjectPool.instance.PopNormal();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.gameObject.SetActive(true);
            normal.target = target;
            normal.speed = normalAttackSpeed;
        }
    }

    //NormalR -> NormalL -> 2HAttack
    public void Normal2HAttack()
    {
        if (target == null)
        {
            NormalAttack normal = NormalObjectPool.instance.PopNormal();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.gameObject.SetActive(true);
            normal.speed = normalAttackSpeed;
            normal.dir = transform.forward;
        }
        else
        {
            this.transform.LookAt(target.transform);
            NormalAttack normal = NormalObjectPool.instance.PopNormal();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.gameObject.SetActive(true);
            normal.target = target;
            normal.speed = normalAttackSpeed;
        }
    }

    public void Skill1()
    {
        if(target != null)
        {
            this.transform.LookAt(target.transform);
            BouncingAttack bounce = BouncingObjectPool.instance.PopBouncing();
            bounce.gameObject.transform.position = firePos.transform.position;
            bounce.start = this.transform;
            bounce.dest = target.transform;
            bounce.speed = bounceAttackSpeed;
        }
            //애니메이션 플레이
    }

    public void Skill2()
    {
        Debug.Log("Skill2");
        //애니메이션 플레이
    }

    public void Skill3()
    {
        Debug.Log("Skill3");
        //애니메이션 플레이
    }
}

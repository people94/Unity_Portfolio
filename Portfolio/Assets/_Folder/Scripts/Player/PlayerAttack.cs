using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    private GameObject target = null;           //타겟
    private RaycastHit hitInfo;                 //레이캐스트 히트 정보
    public LayerMask enemyMask;                 //타겟 설정할 때 타겟이 될 수있는 오브젝트 설정하기 위해

    public GameObject firePos = null;           //기본 공격이 시작되는 위치
    public float normalAttackSpeed = 5.0f;      //기본 공격 총알 속도

    // Update is called once per frame
    void Update()
    {
        SetTarget();
    }

    public void SetTarget()
    {
        /*마우스로 클릭한 위치로 플레이어 이동시킬 때 사용하는 코드
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float distance = 0.0f;
        //Camera.main.ScreenPointToRay - 카메라를 레이의 원점으로 하고 매개변수로 받은 위치를 방향으로 하는 레이를 반환한다.
        //카메라를 origin으로 삼고, 인자로 받을 픽셀로부터 자동으로 방향을 결졍해주는 함수입니다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos;

        //특정 평면과 Ray와의 충돌을 체크하여  Ray의 Origin으로부터 충돌지점까지의 거리를 _f에 저장하여 줍니다.
        if(plane.Raycast(ray, out distance))
        {
            pos = ray.origin + ray.direction * distance;
        }
        */
        
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
       
    public void Attack()
    {
        Debug.Log("Attack");
        if(target == null)
        {
            NormalAttack normal = ObjectPool.instance.PopPool();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.speed = normalAttackSpeed;
        }
        else
        {
            NormalAttack normal = ObjectPool.instance.PopPool();
            normal.gameObject.transform.position = firePos.transform.position;
            normal.target = target;
            normal.speed = normalAttackSpeed;
        }
    }

    public void Skill1()
    {
        Debug.Log("Skill1");
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

    public void Skill4()
    {
        Debug.Log("Skill4");
        //애니메이션 플레이
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : EnemyFSM
{
    public float _findRange = 15f;
    public float _moveRange = 30f;
    public float _attackRange = 15f;

    private GameObject firePos;                 //화살 발사 위치
    public GameObject arrowPref;                //화살 프리팹
    public float arrowSpeed = 10.0f;                    //화살 속도
    private GameObject[] arrowPool;             //화살 오브젝트풀
    private int arrowIdx = 0;                       //화살 인덱스
    private int maxArrow = 10;                  //화살 최대개수

    private void OnEnable()
    {
        findRange = _findRange;
        moveRange = _moveRange;
        attackRange = _attackRange;
        firePos = this.transform.GetChild(0).gameObject;
        arrowPool = new GameObject[maxArrow];
        for (int i = 0; i < maxArrow; i++)
        {
            arrowPool[i] = Instantiate(arrowPref, this.transform);
            arrowPool[i].SetActive(false);
        }
    }

    public override void Idle()
    {
        Debug.Log("Archer : Idle");
        base.Idle();
    }


    public override void Trace()
    {
        print("Archer : Trace");
        base.Trace();
    }

    public override void Attack()
    {
        //일정 시간마다 플레이어를 공격하기
        timer += Time.deltaTime;
        anim.SetFloat("Attack", timer);
        anim.SetBool("Run", false);
        if (timer > attTime)
        {
            gameObject.transform.LookAt(player.transform);
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("BowAttack"));

            arrowPool[arrowIdx].transform.position = firePos.transform.position;
            arrowPool[arrowIdx].transform.forward = this.transform.forward;
            arrowPool[arrowIdx].SetActive(true);
            arrowPool[arrowIdx].GetComponent<Rigidbody>().velocity = arrowPool[arrowIdx].transform.forward * arrowSpeed;
            
            arrowIdx++;
            if (arrowIdx == maxArrow)
            {
                arrowIdx = 0;
            }

            //타이머 초기화
            timer = 0.0f;
        }
    }

    public override void Return()
    {
        print("Archer : Return");
        base.Return();
    }

    public override void Damaged()
    {
        print("Archer : Damaged");
        base.Damaged();
    }

    public override void Die()
    {
        print("Archer : Die");
        base.Die();
    }
}

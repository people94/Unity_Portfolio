using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cataclysm : MonoBehaviour
{
    public float fallingSpeed;
    public float addSpeed;
    public float time;
    public int damage = 100;                                   //몬스터랑 적중시 줄 데미지
    public Vector3 startPos;                                   //스킬 시작 지점
    public GameObject spawnPref;                               //스킬이 시전될때 나오는 프리팹
    public GameObject hitPref;                                 //스킬이 어딘가에 닿았을 때 나오는 프리팹
    private Collider[] enemys;                                 //스킬이 바닥에 닿았을 때 주변에 있는 에너미들
    public float attackRange;                                  //공격 범위

    private void OnEnable()
    {
        //스폰 이펙트 만들고 1초 뒤에 삭제
        //GameObject spawnEffect = Instantiate(spawnPref);
        //if(spawnEffect != null)
        //    Destroy(spawnEffect, 3.0f);
        StartCoroutine(AddSpeed());
    }

    private void Update()
    {
        //fallingSpeed += 2 * Time.deltaTime;
        //계속 밑으로 떨어지기만 한다.
        this.transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("GROUND"))
        {
            enemys = Physics.OverlapSphere(this.transform.position, attackRange, 1 << LayerMask.NameToLayer("ENEMY"));
            foreach( Collider enemy in enemys)
            {
                enemy.gameObject.GetComponent<EnemyFSM>().HitDamage(damage);
            }
            //GameObject hitEffect = Instantiate(hitPref);
            //if(hitEffect != null)
            //    Destroy(hitEffect, 3.0f);
            CataclysmPool.instance.ReturnCataclysm(this);
        }
    }

    IEnumerator AddSpeed()
    {
        yield return new WaitForSeconds(time);
        fallingSpeed = addSpeed;
    }
}

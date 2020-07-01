using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBlaze : MonoBehaviour
{
    public GameObject hitPreb;                                 //적을 맞췄을 때 터뜨리는 효과
    [HideInInspector] public Vector3 startPos;                 //공격이 시작되는 위치 - 여기서 시작 이펙트 생성할것
    public Vector3 dir;                                        //날아갈 방향
    [HideInInspector] public float speed = 10.0f;              //발사 속도
    private float distance = 0.0f;                             //시작 위치와 나 사이의 거리
    public float maxDistance = 15.0f;                          //최대 발사 거리
    public int damage = 100;                              //몬스터랑 적중시 줄 데미지


    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(dir * speed * Time.deltaTime);

        distance = Vector3.Distance(startPos, this.transform.position);
        if (distance >= maxDistance)
        {
            DragonBlazePool.instance.ReturnBlaze(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
        {
            Debug.Log("에너미 적중");
            other.gameObject.GetComponent<EnemyFSM>().HitDamage(damage);
            //GameObject hit = Instantiate(hitPreb);
            //hit.transform.position = other.transform.position;
            //hit.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y - 180, this.transform.rotation.z);
            //if(hit != null)
            //    Destroy(hit, 3.0f);
            DragonBlazePool.instance.ReturnBlaze(this);
        }
    }
}

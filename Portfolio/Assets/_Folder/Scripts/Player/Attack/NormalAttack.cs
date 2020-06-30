using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    [HideInInspector] public float speed = 5.0f;
    public Vector3 dir;
    [HideInInspector] public GameObject target = null;

    private void OnEnable()
    {
        StartCoroutine(Return());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            this.transform.Translate(dir * speed * Time.deltaTime);
        }
        else
        {
            dir = target.transform.position - transform.position;
            dir.Normalize();
            this.transform.Translate(dir * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("ENEMY"))
        {
            collision.collider.transform.GetComponent<EnemyFSM>().HitDamage(100);
            NormalObjectPool.instance.ReturnNormal(this);
        }
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(5.0f);
        NormalObjectPool.instance.ReturnNormal(this);
    }
}

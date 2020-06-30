using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingAttack : MonoBehaviour
{
    [HideInInspector] public Transform start;
    [HideInInspector] public Transform dest;
    private Collider[] enemys;
    private LayerMask enemyMask;
    public int bouncingCnt = 5;
    private int curBouncingCnt = 0;
    public float speed = 5.0f;
    public Vector3 dir;

    private void OnEnable()
    {
        enemyMask = 1 << LayerMask.NameToLayer("ENEMY");
        curBouncingCnt = 0;
        start = null;
        dest = null;
    }

    private void Update()
    {
        if (dest != null)
        {
            if (curBouncingCnt < bouncingCnt)
            {
                dir = dest.position - start.position;
                dir.Normalize();
                this.transform.Translate(dir * speed * Time.deltaTime);
            }
            else
            {
                BouncingObjectPool.instance.ReturnBouncing(this);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("ENEMY"))
        {
            start = collider.gameObject.transform;
            enemys = Physics.OverlapSphere(collider.gameObject.transform.position, 5.0f, enemyMask);
            if (enemys.Length < 2)
            {
                BouncingObjectPool.instance.ReturnBouncing(this);
                return;
            }
            dest = enemys[Random.Range(0, enemys.Length)].transform;
            
            while (start == dest)
            {
                dest = enemys[Random.Range(0, enemys.Length)].transform;
            }
            curBouncingCnt++;
        }
    }
}

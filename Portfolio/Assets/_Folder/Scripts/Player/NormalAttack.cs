using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    [HideInInspector] public float speed = 5.0f;
    private Vector3 dir;
    [HideInInspector] public GameObject target = null;

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        else
        {
            dir = target.transform.position - transform.position;
            dir.Normalize();
            this.transform.Translate(dir * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}

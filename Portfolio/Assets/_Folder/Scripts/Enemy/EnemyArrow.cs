using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            other.GetComponent<PlayerCtrl>().HitDamage(10);
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.gameObject.SetActive(false);
        }
    }
}

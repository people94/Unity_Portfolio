using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClub : MonoBehaviour
{
    public bool isAttack;
    private void OnTriggerEnter(Collider other)
    {
        if (isAttack && other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            other.GetComponent<PlayerCtrl>().HitDamage(10);
            isAttack = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance1 : MonoBehaviour
{
    private bool playerEnter = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!playerEnter && other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            Debug.Log("플레이어입장");
            playerEnter = true;
            GetComponentInParent<Map1>().EnterPlayer();
        }
    }
}

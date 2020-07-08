using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance1 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            Debug.Log("플레이어입장");
            this.GetComponentInParent<Map1>().PlayerEnter();
            Destroy(this.gameObject);
        }
    }
}

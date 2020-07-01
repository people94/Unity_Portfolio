using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject destPos;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("PLAYER"))
        {
            other.GetComponent<PlayerMove>().isTouch = false;
            other.GetComponent<PlayerMove>().UsePortal(destPos.transform.position);
        }
    }
}

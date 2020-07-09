using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject destPos;

    private void Start()
    {
        destPos = GameObject.Find("TutorialDest");
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

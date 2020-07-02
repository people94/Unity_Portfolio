using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject destPos;
    private GameObject player;
    private int enemyCnt;

    private void Start()
    {
        enemyCnt = GetComponentInParent<EnemyCounter>().enemyCnt;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if(enemyCnt == 0 && other.gameObject.CompareTag("PLAYER"))
        {
            other.GetComponent<PlayerMove>().isTouch = false;
            other.GetComponent<PlayerMove>().UsePortal(destPos.transform.position);
        }
    }
}

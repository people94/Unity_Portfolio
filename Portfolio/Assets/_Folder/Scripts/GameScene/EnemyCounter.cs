using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public int enemyCnt = 5;

    public GameObject portal;   //에너미 다 죽으면 열릴 포탈

    public void PortalOpen()
    {
        portal.SetActive(true);
    }
}

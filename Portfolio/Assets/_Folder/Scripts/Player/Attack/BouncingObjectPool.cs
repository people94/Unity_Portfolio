using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObjectPool : MonoBehaviour
{
    public static BouncingObjectPool instance = null;             //싱글톤패턴을 위한 인스턴스
    private Queue<BouncingAttack> bouncingPool = null;            //기본공격 프리팹담을 풀
    public GameObject bouncingAttack = null;                      //기본공격 프리팹

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        InitBouncing(3);

        //DontDestroyOnLoad(this.gameObject);
    }

    public void InitBouncing(int poolSize)
    {
        bouncingPool = new Queue<BouncingAttack>();
        for (int i = 0; i < poolSize; i++)
        {
            bouncingPool.Enqueue(PushBouncing());
        }
    }

    public BouncingAttack PushBouncing()
    {
        BouncingAttack bounce = Instantiate<GameObject>(bouncingAttack).GetComponent<BouncingAttack>();
        bounce.gameObject.SetActive(false);
        bounce.transform.SetParent(this.transform);
        return bounce;
    }

    public BouncingAttack PopBouncing()
    {
        if (bouncingPool.Count > 0)
        {
            Debug.Log("1번");
            BouncingAttack bounce = bouncingPool.Dequeue();
            bounce.gameObject.SetActive(true);
            bounce.transform.SetParent(null);
            return bounce;
        }
        else
        {
            Debug.Log("2번");
            BouncingAttack bounce = Instantiate<GameObject>(bouncingAttack).GetComponent<BouncingAttack>();
            bounce.gameObject.SetActive(true);
            bounce.transform.SetParent(null);
            return bounce;
        }
    }

    public void ReturnBouncing(BouncingAttack bounce)
    {
        Debug.Log("리턴");
        bounce.gameObject.SetActive(false);
        bounce.transform.SetParent(this.transform);
        bouncingPool.Enqueue(bounce);
    }
}

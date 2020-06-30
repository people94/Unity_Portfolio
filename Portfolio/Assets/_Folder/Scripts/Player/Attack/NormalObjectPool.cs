using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalObjectPool : MonoBehaviour
{
    public static NormalObjectPool instance = null;                   //싱글톤패턴을 위한 인스턴스
    private Queue<NormalAttack> normalPool = null;              //기본공격 프리팹담을 풀
    public GameObject normalAttack = null;                      //기본공격 프리팹

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        InitNormal(10);

        DontDestroyOnLoad(this.gameObject);
    }

    public void InitNormal(int poolSize)
    {
        normalPool = new Queue<NormalAttack>();
        for(int i = 0; i < poolSize; i++)
        {
            normalPool.Enqueue(PushNormal());
        }
    }

    public NormalAttack PushNormal()
    {
        NormalAttack normal = Instantiate<GameObject>(normalAttack).GetComponent<NormalAttack>();
        normal.gameObject.SetActive(false);
        normal.transform.SetParent(this.transform);
        return normal;
    }

    public NormalAttack PopNormal()
    {
        if(normalPool.Count > 0)
        {
            NormalAttack normal = normalPool.Dequeue();
            normal.transform.SetParent(null);
            return normal;
        }
        else
        {
            NormalAttack normal = Instantiate<GameObject>(normalAttack).GetComponent<NormalAttack>();
            normal.transform.SetParent(null);
            return normal;
        }
    }

    public void ReturnNormal(NormalAttack normal)
    {
        normal.gameObject.SetActive(false);
        normal.transform.SetParent(this.transform);
        normalPool.Enqueue(normal);
    }
}

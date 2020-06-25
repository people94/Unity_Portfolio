using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance = null;                   //싱글톤패턴을 위한 인스턴스
    private Queue<NormalAttack> normalPool = null;              //프리팹담을 풀
    public GameObject normalAttack = null;                      //기본공격 프리팹

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        InitPool(10);

        DontDestroyOnLoad(this.gameObject);
    }

    public void InitPool(int poolSize)
    {
        normalPool = new Queue<NormalAttack>();
        for(int i = 0; i < poolSize; i++)
        {
            normalPool.Enqueue(PushPool());
        }
    }

    public NormalAttack PushPool()
    {
        NormalAttack normal = Instantiate<GameObject>(normalAttack).GetComponent<NormalAttack>();
        normal.gameObject.SetActive(false);
        normal.transform.SetParent(this.transform);
        return normal;
    }

    public NormalAttack PopPool()
    {
        if(normalPool.Count > 0)
        {
            NormalAttack normal = normalPool.Dequeue();
            normal.gameObject.SetActive(true);
            normal.transform.SetParent(null);
            return normal;
        }
        else
        {
            NormalAttack normal = Instantiate<GameObject>(normalAttack).GetComponent<NormalAttack>();
            normal.gameObject.SetActive(true);
            normal.transform.SetParent(null);
            return normal;
        }
    }

    public void ReturnPool(NormalAttack normal)
    {
        normal.gameObject.SetActive(false);
        normal.transform.SetParent(this.transform);
        normalPool.Enqueue(normal);
    }
}

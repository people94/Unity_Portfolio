using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CataclysmPool : MonoBehaviour
{
    public static CataclysmPool instance = null;             //싱글톤패턴을 위한 인스턴스
    private Queue<Cataclysm> cataclysmPool = null;            //기본공격 프리팹담을 풀
    public GameObject cataclysmPref = null;                      //기본공격 프리팹

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        InitCataclysm(3);
    }

    public void InitCataclysm(int poolSize)
    {
        cataclysmPool = new Queue<Cataclysm>();
        for (int i = 0; i < poolSize; i++)
        {
            cataclysmPool.Enqueue(PushCataclysm());
        }
    }

    public Cataclysm PushCataclysm()
    {
        Cataclysm cataclysm = Instantiate<GameObject>(cataclysmPref).GetComponent<Cataclysm>();
        cataclysm.gameObject.SetActive(false);
        cataclysm.transform.SetParent(this.transform);
        return cataclysm;
    }

    public Cataclysm PopCataclysm()
    {
        if (cataclysmPool.Count > 0)
        {
            Cataclysm cataclysm = cataclysmPool.Dequeue();
            cataclysm.gameObject.SetActive(true);
            cataclysm.transform.SetParent(null);
            return cataclysm;
        }
        else
        {
            Cataclysm cataclysm = Instantiate<GameObject>(cataclysmPref).GetComponent<Cataclysm>();
            cataclysm.gameObject.SetActive(true);
            cataclysm.transform.SetParent(null);
            return cataclysm;
        }
    }

    public void ReturnCataclysm(Cataclysm cataclysm)
    {
        cataclysm.gameObject.SetActive(false);
        cataclysm.transform.SetParent(this.transform);
        cataclysmPool.Enqueue(cataclysm);
    }
}

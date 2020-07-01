using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBlazePool : MonoBehaviour
{
    public static DragonBlazePool instance = null;             //싱글톤패턴을 위한 인스턴스
    private Queue<DragonBlaze> blazePool = null;            //기본공격 프리팹담을 풀
    public GameObject blazePref = null;                      //기본공격 프리팹

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        InitBlaze(20);

        //DontDestroyOnLoad(this.gameObject);
    }

    public void InitBlaze(int poolSize)
    {
        blazePool = new Queue<DragonBlaze>();
        for (int i = 0; i < poolSize; i++)
        {
            blazePool.Enqueue(PushBlaze());
        }
    }

    public DragonBlaze PushBlaze()
    {
        DragonBlaze blaze = Instantiate<GameObject>(blazePref).GetComponent<DragonBlaze>();
        blaze.gameObject.SetActive(false);
        blaze.transform.SetParent(this.transform);
        return blaze;
    }

    public DragonBlaze PopBlaze()
    {
        if (blazePool.Count > 0)
        {
            DragonBlaze blaze = blazePool.Dequeue();
            blaze.gameObject.SetActive(true);
            blaze.transform.SetParent(null);
            return blaze;
        }
        else
        {
            DragonBlaze blaze = Instantiate<GameObject>(blazePref).GetComponent<DragonBlaze>();
            blaze.gameObject.SetActive(true);
            blaze.transform.SetParent(null);
            return blaze;
        }
    }

    public void ReturnBlaze(DragonBlaze blaze)
    {
        blaze.gameObject.SetActive(false);
        blaze.transform.SetParent(this.transform);
        blazePool.Enqueue(blaze);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalPool : MonoBehaviour
{
    public static OrbitalPool instance = null;                   //싱글톤패턴을 위한 인스턴스
    private Queue<OrbitalFlame> orbitalPool = null;              //기본공격 프리팹담을 풀
    public GameObject orbitalPref = null;                      //기본공격 프리팹

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        InitOrbital(10);

        DontDestroyOnLoad(this.gameObject);
    }

    public void InitOrbital(int poolSize)
    {
        orbitalPool = new Queue<OrbitalFlame>();
        for(int i = 0; i < poolSize; i++)
        {
            orbitalPool.Enqueue(PushOrbital());
        }
    }

    public OrbitalFlame PushOrbital()
    {
        OrbitalFlame orbital = Instantiate<GameObject>(orbitalPref).GetComponent<OrbitalFlame>();
        orbital.gameObject.SetActive(false);
        orbital.transform.SetParent(this.transform);
        return orbital;
    }

    public OrbitalFlame PopOrbital()
    {
        if(orbitalPool.Count > 0)
        {
            OrbitalFlame orbital = orbitalPool.Dequeue();
            orbital.transform.SetParent(null);
            return orbital;
        }
        else
        {
            OrbitalFlame orbital = Instantiate<GameObject>(orbitalPref).GetComponent<OrbitalFlame>();
            orbital.transform.SetParent(null);
            return orbital;
        }
    }

    public void ReturnOrbital(OrbitalFlame orbital)
    {
        orbital.gameObject.SetActive(false);
        orbital.transform.SetParent(this.transform);
        orbitalPool.Enqueue(orbital);
    }
}

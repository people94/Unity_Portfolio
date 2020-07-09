﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    private GameObject target = null;           //타겟
    private RaycastHit hitInfo;                 //레이캐스트 히트 정보
    public LayerMask enemyMask;                 //타겟 설정할 때 타겟이 될 수있는 오브젝트 설정하기 위해

    public GameObject firePos = null;           //공격이 시작되는 위치

    public GameObject returnPos = null;         //공격 했다가 돌아오는 위치
    public float orbitalSpeed = 10.0f;          //Orbital 공격 발사 속도
    private bool onOrbital = false;             //현재 Orbital 공격을 하고있는지

    public float blazeSpeed = 10.0f;            //DragonBlaze 공격 발사 속도
    private bool onBlaze = false;               //현재 Blaze 공격을 하고있는지(Blaze공격 사이의 텀을 두기 위함)

    public float cataclysmDis = 20.0f;          //Cataclysm 공격 사거리
    public float cataclysmSpeed = 20.0f;        //Cataclysm 떨어지는 속도
    public float cataclysmHeight = 10.0f;       //Cataclysm 얼마나 위에서 떨어질건지
    public float cataclysmRange = 20.0f;        //Cataclysm 공격 범위

    public bool isAttack = false;               //현재 공격중인지

    private Animator anim = null;

    //튜토리얼에서 Orbital 작동 했는지
    public GameObject tutorialMap;
    private bool orbital = false;
    //튜토리얼에서 Blaze 작동 했는지
    private bool blaze = false;
    //튜토리얼에서 cataclysm 작동 했는지
    private bool cataclysm = false;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();       
;   }

    public void SetTarget()
    {
        //현재 마우스 클릭이 UI이면 true를 아니면 false를 반환하는 함수
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //0번 - 왼쪽 1번 - 오른쪽 2번 - 가운데?
        if (Input.GetMouseButton(0))
        {
            //마우스로 클릭한 물체가 무엇인지 판별하는 코드
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 999f, enemyMask))
            {
                if (target != null)
                    target.GetComponent<Outline>().enabled = false;
                target = hitInfo.collider.gameObject;
                target.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if (target != null)
                {
                    target.GetComponent<Outline>().enabled = false;
                    target = null;
                }            
            }
        }
    }
    
    //OrbitalR -> OrbitalL -> 2HAttack
    public void OrbitalAttack()
    {
        if(!orbital)
        {
            tutorialMap.GetComponent<TutorialMap>().orbitalGuide = true;
            orbital = true;
        }
        if (!onOrbital)
        {
            anim.SetTrigger("Orbital");
            //애니메이션 플레이
            StartCoroutine(OnOrbital());
            if (target == null)
            {
                OrbitalFlame orbital = OrbitalPool.instance.PopOrbital();
                orbital.gameObject.transform.position = firePos.transform.position;
                orbital.gameObject.SetActive(true);
                orbital.speed = orbitalSpeed;
                orbital.returnTarget = returnPos;
                orbital.dir = transform.forward;
            }
            else
            {
                this.transform.LookAt(target.transform);
                OrbitalFlame orbital = OrbitalPool.instance.PopOrbital();
                orbital.gameObject.transform.position = firePos.transform.position;
                orbital.gameObject.SetActive(true);
                orbital.returnTarget = returnPos;
                orbital.speed = orbitalSpeed;
                orbital.dir = (target.transform.position - firePos.transform.position).normalized;
            }
        }
    }

    IEnumerator OnOrbital()
    {
        onOrbital = true;
        isAttack = true;
        yield return new WaitForSeconds(0.2f);
        onOrbital = false;
        isAttack = false;
    }
    
    public void DragonBlaze()
    {
        if (!blaze)
        {
            tutorialMap.GetComponent<TutorialMap>().blazeGuide = true;
            blaze = true;
        }
        //애니메이션 플레이
        if (!onBlaze)
        {
            anim.SetBool("Blaze", true);
            StartCoroutine(OnBlaze());
            if (target == null)
            {
                DragonBlaze blaze = DragonBlazePool.instance.PopBlaze();
                blaze.gameObject.transform.position = new Vector3(Random.Range(firePos.transform.position.x - 1, firePos.transform.position.x + 1),
                                                                  firePos.transform.position.y,
                                                                  Random.Range(firePos.transform.position.z - 1, firePos.transform.position.z + 1));
                blaze.startPos = this.transform.position;
                blaze.dir = this.transform.forward;
                blaze.speed = blazeSpeed;
            }
            else
            {
                this.transform.LookAt(target.transform);
                DragonBlaze blaze = DragonBlazePool.instance.PopBlaze();
                blaze.gameObject.transform.position = new Vector3(Random.Range(firePos.transform.position.x - 1, firePos.transform.position.x + 1),
                                                                  firePos.transform.position.y,
                                                                  Random.Range(firePos.transform.position.z - 1, firePos.transform.position.z + 1));
                //new Vector3(Random.Range(firePos.transform.position.x - 1, firePos.transform.position.x + 1), Random.Range(firePos.transform.position.y - 1, firePos.transform.position.y - 1), firePos.transform.position.z - 1);
                blaze.startPos = this.transform.position;
                blaze.dir = this.transform.forward;
                blaze.speed = blazeSpeed;
            }            
        }
    }

    IEnumerator OnBlaze()
    {
        onBlaze = true;
        yield return new WaitForSeconds(0.1f);
        onBlaze = false;
    }

    IEnumerator CataProc()
    {
        yield return new WaitForSeconds(1.2f);
        DoCataclysm();
        isAttack = false;
    }

    public void DoCataclysm()
    {
        if (!cataclysm)
        {
            tutorialMap.GetComponent<TutorialMap>().cataclysmGuide = true;
            cataclysm = true;
        }
        if (target == null)
        {
            Cataclysm cataclysm = CataclysmPool.instance.PopCataclysm();
            cataclysm.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + cataclysmHeight, transform.position.z) + transform.forward * cataclysmDis;
            cataclysm.startPos = new Vector3(transform.position.x, transform.position.y + cataclysmHeight, transform.position.z) + transform.forward * cataclysmDis;
            //cataclysm.fallingSpeed = cataclysmSpeed;
            cataclysm.GetComponent<Cataclysm>().Spawn();
            cataclysm.attackRange = cataclysmRange;
        }
        else
        {
            this.transform.LookAt(target.transform);
            Cataclysm cataclysm = CataclysmPool.instance.PopCataclysm();
            cataclysm.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + cataclysmHeight, target.transform.position.z);
            cataclysm.startPos = new Vector3(target.transform.position.x, target.transform.position.y + cataclysmHeight, target.transform.position.z);
            //cataclysm.fallingSpeed = cataclysmSpeed;
            cataclysm.GetComponent<Cataclysm>().Spawn();
            cataclysm.attackRange = cataclysmRange;
        }
    }

    public void Cataclysm()
    {
        Debug.Log("카타클리즘");
        isAttack = true;
        anim.SetTrigger("Cataclysm");
        StartCoroutine(CataProc());
    }
    
    public void Skill3()
    {
        Debug.Log("Skill3");
        //애니메이션 플레이
    }
}

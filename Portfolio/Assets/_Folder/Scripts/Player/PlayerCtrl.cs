﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    #region "플레이어 hp/mp"
    public Image hpBar = null;             //hp바   
    private Image mpBar = null;             //mp바
    private float maxHp = 100.0f;           //플레이어 전체 체력
    private float curHp;                    //플레이어 현재 체력
    private float maxMp = 100.0f;           //플레이어 전체 마나
    private float curMp;                    //플레이어 현재 마나
    private float curTime = 0.0f;           //몇초 지났는지 체크
    private float chargeTime = 0.5f;        //충전되는 시간
    IEnumerator coroutine;
    #endregion

    private void OnEnable()
    {
        curHp = maxHp;
        curMp = maxMp;
    }

    private void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("1번");
            HitDamage(10);
        }
    }

    //플레이어에게 데미지 줄때 호출할 함수
    public void HitDamage(float damage)
    {
        ChangeHp(-damage);
        //애니메이션 플레이
    }

    //hp 변경할 일이 있을 때 호출하는 함수
    public void ChangeHp(float value)
    {
        curHp += value;
        coroutine = SlowChangeHp(value);
        StartCoroutine(coroutine);
    }

    //mp 변경할 일이 있을 때 호출하는 함수
    public void ChangeMp(float value)
    {
        curMp += value;
        coroutine = SlowChangeMp(value);
        StartCoroutine(coroutine);
    }

    //hp 천천히 줄어들거나 늘리게 하는 함수
    IEnumerator SlowChangeHp(float value)
    {
        float saveHp = hpBar.fillAmount * 100 + value;
        while (true)
        {
            if (curTime >= chargeTime)
            {
                StopCoroutine(coroutine);
                coroutine = null;
                curTime = 0.0f;
                hpBar.fillAmount = saveHp / 100.0f;
                break;
            }
            curTime += Time.deltaTime;
            hpBar.fillAmount += value * Time.deltaTime / chargeTime / 100;
            yield return null;
        }
    }

    //mp 천천히 줄어들거나 늘리게 하는 함수
    IEnumerator SlowChangeMp(float value)
    {
        float saveMp = mpBar.fillAmount * 100+ value;
        while (true)
        {
            if (curTime >= chargeTime)
            {
                StopCoroutine(coroutine);
                coroutine = null;
                curTime = 0.0f;
                mpBar.fillAmount = saveMp / 100.0f;
                break;
            }
            curTime += Time.deltaTime;
            mpBar.fillAmount += value * Time.deltaTime / chargeTime / 100;
            yield return null;
        }
    }
}
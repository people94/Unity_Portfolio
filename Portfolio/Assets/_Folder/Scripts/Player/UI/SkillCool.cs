using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCool : MonoBehaviour
{
    public float coolDown;  //쿨다운 시간
    private IEnumerator coroutine;

    public void CoolDown()
    {
        GetComponent<Image>().fillAmount = 0;
        coroutine = IsCool();
        this.GetComponent<Button>().enabled = false;
        StartCoroutine(coroutine);
    }

    IEnumerator IsCool()
    {
        while(true)
        {
            GetComponent<Image>().fillAmount += Time.deltaTime / coolDown;
            yield return null;
            if(GetComponent<Image>().fillAmount >= 1)
            {
                this.GetComponent<Button>().enabled = true;
                StopCoroutine(coroutine);
                break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyDown : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private PlayerAttack player = null;
    private bool isTouch = false;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if(isTouch)
            player.DragonBlaze();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!this.GetComponent<SkillCool>().isCool)
        {
            isTouch = true;
            player.isAttack = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isTouch)
        {
            isTouch = false;
            player.isAttack = false;
            player.gameObject.GetComponentInChildren<Animator>().SetBool("Blaze", false);
            this.GetComponent<SkillCool>().CoolDown();
        }
    }    
}

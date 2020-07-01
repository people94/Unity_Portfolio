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
        if(!this.GetComponent<SkillCool>().isCool)
            isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isTouch)
        {
            isTouch = false;
            this.GetComponent<SkillCool>().CoolDown();
        }
    }    
}

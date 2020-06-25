using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDrop : MonoBehaviour, IDropHandler
{
    public int ItemNum;

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0 && ItemNum == SlotDrag.draggingItem.GetComponent<SlotDrag>().ItemNum)
        {
            SlotDrag.draggingItem.transform.SetParent(this.transform);
        }
    }
    
}

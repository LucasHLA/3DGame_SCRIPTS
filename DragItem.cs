using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeginDragged;
    Vector3 startPOS;
    Transform startParent;
    public Itens item;

    void Start()
    {
        GetComponent<Image>().sprite = item.icon;
    }

    
    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeginDragged = gameObject;
        startPOS = transform.position;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == startParent)
        {
            transform.position = startPOS;
        }
    }

    public void setParent(Transform slotTransform, Slot slot)
    {
        if (item.slotsType.ToString() == slot.slotsType.ToString())
        {
            transform.SetParent(slotTransform);
            item.getAction();
        }
        else if (slot.slotsType.ToString() == "inventory")
        {
            transform.SetParent(slotTransform);
        }
    }
}

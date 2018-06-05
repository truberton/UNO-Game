﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform placeholderParent = null;
    public Vector3 HandPosition { get; set; }

    GameObject placeholder = null;

    public enum Slot { TableCard, HandCard };
    public Slot typeOfItem = Slot.TableCard;

    public string HandColor { get; private set; }
    public GameObject MainCard { get; private set; }
    public string StackColor = "Blue";

    public void OnBeginDrag(PointerEventData eventData)
    {
        HandPosition = Input.mousePosition;
        MainCard = eventData.pointerDrag;
        placeholder = new GameObject();
        placeholder.transform.SetParent(transform.parent);
        LayoutElement layoutElement = placeholder.AddComponent<LayoutElement>();
        layoutElement.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        layoutElement.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        layoutElement.flexibleWidth = 0;
        layoutElement.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        parentToReturnTo = transform.parent;
        placeholderParent = parentToReturnTo;
        transform.SetParent(transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        if (placeholder.transform.parent != placeholderParent)
        {
            placeholder.transform.SetParent(placeholderParent);
        }

        int NewSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++)
        {
            if (transform.position.x < placeholderParent.GetChild(i).position.x)
            {
                NewSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < NewSiblingIndex)
                {
                    NewSiblingIndex--;
                }

                break;
            }
        }
        placeholder.transform.SetSiblingIndex(NewSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentToReturnTo);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);

        Card_deck c = new Card_deck();
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        /*TODO
        Add color, specialfunction, first card allowed always*/
        if (MainCard != null)
        {
            if (typeOfItem == d.typeOfItem)
            {
                //d.parentToReturnTo = transform;
            }
            HandColor = MainCard.GetComponent<CardValues>().Color;
            Debug.Log(HandColor.ToString());
            if (HandColor != StackColor)
            {
                MainCard.transform.position = HandPosition;
                Debug.Log("rgftrjyusjs");
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField]
    private CollectableData collectableData;

    private bool dragged = false;
    private bool over = false;
    private RectTransform rectT;

    private void Start()
    {
        rectT = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (dragged)
        {
            rectT.position = Input.mousePosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragged = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (over)
            dragged = true;
    }
}

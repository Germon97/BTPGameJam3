using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CollectableData collectableData;

    private bool dragged = false;
    private bool over = false;
    private RectTransform rectT;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
        inventoryManager = GetComponentInParent<InventoryManager>();
    }

    public void Init(CollectableData colData)
    {
        GetComponent<Image>().sprite = collectableData.sprite;
    }
    public void Init(CollectableData colData, InventoryManager manager)
    {
        collectableData = colData;
        GetComponent<Image>().sprite = colData.sprite;
        inventoryManager = manager;
    }

    private void Update()
    {
        if (dragged)
        {
            rectT.position = Input.mousePosition;
        }
    }

    public CollectableData GetData()
    {
        return collectableData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryManager != null)
        {
            inventoryManager.NewSelectedItem(this);
        }
    }
}

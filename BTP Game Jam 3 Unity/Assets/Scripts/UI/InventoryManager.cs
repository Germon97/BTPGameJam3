using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField]
    private GameObject inventoryItem;

    private List<InventoryItem> inventory;

    [SerializeField]
    private Transform inventoryPanel;

    [SerializeField]
    private PlayerCollector collector;

    private InventoryItem selectedItem;

    private void Start()
    {
        inventory = new List<InventoryItem>();
    }

    public void Add(CollectableData col)
    {
        GameObject item = Instantiate(inventoryItem, Vector3.zero, Quaternion.identity);
        item.transform.SetParent(inventoryPanel, false);

        item.GetComponent<InventoryItem>().Init(col, this);
    }

    public void Remove()
    {
        if (selectedItem == null)
            return;

        inventory.Remove(selectedItem);
        collector.RemoveCollectable(selectedItem.GetData());
        Destroy(selectedItem.gameObject);
    }

    public void NewSelectedItem(InventoryItem item)
    {
        selectedItem = item;

        ChangeFocusedCollectable(item.GetData());
    }
    
    private void ChangeFocusedCollectable(CollectableData data)
    {
        string description = "";
        description += "Value: " + data.value.ToString() + "\n";
        description += "Weight: " + (data.weight * 10).ToString() + "\n";
        description += data.text;

        descriptionText.text = description;
    }
}

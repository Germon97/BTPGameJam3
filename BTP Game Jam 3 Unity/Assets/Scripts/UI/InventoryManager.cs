using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private GameObject inventoryItem;

    private InventoryItem[] inventory;




    private void Add(CollectableData collectable)
    {
            
    }


    private void ChangeFocusedCollectable(CollectableData data)
    {
        string description = "";
        description += "Value: " + data.value.ToString() + "\n";
        description += "Weight: " + data.weight.ToString() + "\n";
        description += data.text;

        descriptionText.text = description;
    }
}

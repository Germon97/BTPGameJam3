using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    // TODO: refactor.
    [SerializeField]
    private LayerMask collectableLayer;
    [SerializeField]
    private float collectAbleRadius;

    [SerializeField]
    private int currentGold = 0;
    [SerializeField]
    private int maxWeight = 0;
    [SerializeField]
    private float currentWeight = 0;

    [SerializeField]
    private WeightBarScript weightBar;
    [SerializeField]
    private GoldScript goldScript;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private GameObject collectAblePrefab;

    private void Start()
    {
        goldScript.UpdateGold(0);
        weightBar.ChangeMaxWeight(maxWeight);
        weightBar.ChangeWeight(currentWeight);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            Collect();
    }

    public void Collect()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, collectAbleRadius, Vector2.up, 0.2f, collectableLayer);

        if (hit.collider != null)
        {
            CollectAble col = hit.collider.gameObject.GetComponent<CollectAble>();

            // TODO add responce for to much loot.
            if (col == null || currentWeight + col.data.weight > maxWeight)
                return;

            Collect(col);
            Destroy(col.gameObject);
        }
    }

    private void Collect(CollectAble col)
    {
        currentGold += col.data.value;
        currentWeight += col.data.weight;

        weightBar.ChangeWeight(currentWeight);
        goldScript.UpdateGold(currentGold);

        inventoryManager.Add(col.data);
    }

    public void RemoveCollectable(CollectableData col)
    {
        currentGold -= col.value;
        currentWeight -= col.weight;

        weightBar.ChangeWeight(currentWeight);
        goldScript.UpdateGold(currentGold);

        SpawnCollectable(col);
    }

    public float GetWeightSlowMultiplier()
    {
        return 1 - (currentWeight / maxWeight);
    }

    public void SpawnCollectable(CollectableData col)
    {
        GameObject collectable = Instantiate(collectAblePrefab, transform.position, Quaternion.identity);
        collectable.GetComponent<CollectAble>().Init(col);
    }
}

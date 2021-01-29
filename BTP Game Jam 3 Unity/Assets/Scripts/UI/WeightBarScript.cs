using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeightBarScript : MonoBehaviour
{
    [SerializeField]
    private Image[] fullWeights;
    [SerializeField]
    private Image[] emptyWeights;
    [SerializeField]
    private RectTransform mask;

    private int maxWeight = 3;
    private float currentWeight = 1.5f;

    private void UpdateUI()
    {
        for (int i = 0; i < fullWeights.Length; i++)
        {
            fullWeights[i].enabled = i < maxWeight;
        }

        for (int i = 0; i < emptyWeights.Length; i++)
        {
            emptyWeights[i].enabled = i < maxWeight;
        }

        Vector2 size = mask.sizeDelta;
        size.x = currentWeight * 63;
        mask.sizeDelta = size;
    }

    public void ChangeWeight(float weight)
    {
        currentWeight = weight;
        UpdateUI();
    }

    public void ChangeMaxWeight(int weight)
    {
        maxWeight = weight;
        UpdateUI();
    }
}

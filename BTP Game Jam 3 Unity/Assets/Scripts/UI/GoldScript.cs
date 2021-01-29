using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldScript : MonoBehaviour
{
    [SerializeField]
    private TMP_Text goldText;

    public void UpdateGold(int value)
    {
        goldText.text = value.ToString() + "$";
    }

}

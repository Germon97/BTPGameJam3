using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectable", menuName = "Collectable")]
[System.Serializable]
public class CollectableData : ScriptableObject
{
    public Sprite sprite;
    public int value;
    public float weight;
    [TextArea]
    public string text;
}

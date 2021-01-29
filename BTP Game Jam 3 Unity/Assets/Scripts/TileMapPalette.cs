using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(fileName = "TileMapTemplate", menuName = "TileMapTemplate")]
public class TileMapPalette : ScriptableObject
{
    public TileBase raum;
    public TileBase tunnel;
}

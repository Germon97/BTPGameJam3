using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileBaseComposer : MonoBehaviour
{
    public abstract void Init();
    public abstract TileBase GetWallTileBase(int x, int y, bool tunnel, bool horizontal, bool corner);
    public abstract TileBase GetFloorTileBase(int x, int y, bool tunnel);
}

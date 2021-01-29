using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The parent class for a primitve room layouts.
/// </summary>
[System.Serializable]
public abstract class Primitives
{
    public abstract int GetSize();
    public abstract bool DoesCollide(Primitives other);

    public abstract Primitives Copy();

    public abstract List<Vector2Int> GetCoordinates();

    public abstract int GetDistance(Primitives other);
    public abstract int GetXDistance(Primitives other);
    public abstract int GetYDistance(Primitives other);

    public abstract Vector2Int[] GetConnectionPoints(Primitives other);

    public abstract string ToString();

    public abstract Vector3Int GetCentrer();
}

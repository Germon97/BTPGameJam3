using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TunnelDirection { Verical, Horizontal }

public class Tunnel : MonoBehaviour
{

    private TunnelDirection direction;
    private Primitives[] primitives;

    public Tunnel(Primitives[] primitives, TunnelDirection direction)
    {
        this.primitives = primitives;
        this.direction = direction;
    }

    public HashSet<Vector2Int> GetCoordinates()
    {
        HashSet<Vector2Int> coordinates = new HashSet<Vector2Int>();

        foreach (Primitives p in primitives)
        {
            coordinates.UnionWith(p.GetCoordinates());
        }
        return coordinates;
    }
}

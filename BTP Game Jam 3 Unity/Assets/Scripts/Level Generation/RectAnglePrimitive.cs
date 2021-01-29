using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class RectAnglePrimitive : Primitives
{
    public Vector2Int origin;
    public Vector2Int size;

    public List<Vector2Int> coordinates;

    public RectAnglePrimitive(Vector2Int origin, Vector2Int size)
    {
        this.origin = origin;
        this.size = size;
        CalculateCoordinates();
    }

    public override Primitives Copy()
    {
        RectAnglePrimitive newRect = new RectAnglePrimitive(origin, size);
        return newRect;
    }

    public override bool DoesCollide(Primitives other)
    {
        if (other is RectAnglePrimitive)
            return RectangleCollision((RectAnglePrimitive)other);
        return false;
    }

    /// <summary>
    /// Puts all the coordinates from this room in a list.
    /// </summary>
    private void CalculateCoordinates()
    {
        coordinates = new List<Vector2Int>();

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                coordinates.Add(new Vector2Int((int)(origin.x + x), (int)(origin.y + y)));
            }
        }
    }

    public bool RectangleCollision(RectAnglePrimitive other)
    {
        // Checks the collision between to rectancles by checking each axis seperatately.
        // If there is one axis in which the rectangles don't overlap, the rectalgles themself do therefore not collider.
        
        // The walls around the rectangles are still considered to be part of the room and
        // are therefore also calculated in this method.

        // Check x axis.
        if (origin.x + size.x + 1 <= other.origin.x - 1 || other.origin.x + other.size.x + 1 <= origin.x - 1)
            return false;
        // Check y axis.
        if (origin.y + size.y + 1 <= other.origin.y - 1 || other.origin.y + other.size.y + 1 <= origin.y - 1)
            return false;
        
        // Rectangles overlap in both axis and therefore they overlap as a whole.
        return true;
    }
    public override List<Vector2Int> GetCoordinates()
    {
        return coordinates;
    }

    public override int GetSize()
    {
        return (int)(size.x * size.y);
    }

    // TODO: that can be better.
    public override int GetDistance(Primitives other)
    {
        int xDis = GetXDistance(other);
        int yDis = GetYDistance(other);

        if (xDis < 0)
            return yDis;
        else if (yDis < 0)
            return xDis;
        else
            return xDis + yDis;
    }

    public override int GetXDistance(Primitives other)
    {
        if (other is RectAnglePrimitive)
        {
            RectAnglePrimitive otherRect = (RectAnglePrimitive)other;
            if (origin.x < otherRect.origin.x)
            {
                return otherRect.origin.x - origin.x - size.x;
            }
            else
            {
                return origin.x - otherRect.origin.x - otherRect.size.x;
            }
        }
        return int.MaxValue;
    }

    public override int GetYDistance(Primitives other)
    {
        if (other is RectAnglePrimitive)
        {
            RectAnglePrimitive otherRect = (RectAnglePrimitive)other;
            if (origin.y < otherRect.origin.y)
            {
                return otherRect.origin.y - origin.y - size.y;
            }
            else
            {
                return origin.y - otherRect.origin.y - otherRect.size.y;
            }
        }
        return int.MaxValue;
    }

    public override Vector2Int[] GetConnectionPoints(Primitives other)
    {
        int xDis = GetXDistance(other);
        int yDis = GetYDistance(other);

        if (other is RectAnglePrimitive)
        {
            RectAnglePrimitive otherRect = (RectAnglePrimitive)other;
            if (xDis < 0)
            {
                // Primitivs overlap in the x axis.
                int xTunnel = 0;
                if (origin.x < otherRect.origin.x)
                {
                    xTunnel = (int)((otherRect.origin.x + origin.x + size.x) / 2);
                }
                else
                {
                    xTunnel = (int)((origin.x + otherRect.origin.x + otherRect.size.x) / 2);
                }

                if (origin.y < otherRect.origin.y)
                {
                    return new Vector2Int[] { new Vector2Int(xTunnel, origin.y + size.y), new Vector2Int(xTunnel, otherRect.origin.y) };
                }
                else
                {
                    return new Vector2Int[] { new Vector2Int(xTunnel, otherRect.origin.y + otherRect.size.y), new Vector2Int(xTunnel, origin.y) };
                }
            }
            else if (yDis < 0)
            {
                // Primitivs overlap in the x axis.
                int yTunnel = 0;
                if (origin.y < otherRect.origin.y)
                {
                    yTunnel = (int)((otherRect.origin.y + origin.y + size.y) / 2);
                }
                else
                {
                    yTunnel = (int)((origin.y + otherRect.origin.y + otherRect.size.y) / 2);
                }

                if (origin.x < otherRect.origin.x)
                {
                    return new Vector2Int[] { new Vector2Int(origin.x + size.x, yTunnel), new Vector2Int(otherRect.origin.x, yTunnel) };
                }
                else
                {
                    return new Vector2Int[] { new Vector2Int(otherRect.origin.x + otherRect.size.x, yTunnel), new Vector2Int(origin.x, yTunnel) };
                }
            }

        }
        return null;
    }

    public override string ToString()
    {
        return "RectPrim: origin: " + origin.x.ToString() + ", " + origin.y.ToString() + " size: " + size.x.ToString() + ", " + size.y.ToString();
    }

    public override Vector3Int GetCentrer()
    {
        return new Vector3Int(origin.x + (int)(size.x / 2), origin.y + (int)(size.y / 2), 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType { Start, End, Normal }

[System.Serializable]
public class Room
{

    [SerializeField]
    private List<Room> connectedRooms;

    public int size;
    public List<Primitives> primitives;

    /// <summary>
    /// A hashset for all coordinates in this room.
    /// Gets together all coordinates from its primitives.
    /// </summary>
    private HashSet<Vector2Int> coordinates;

    /// <summary>
    /// Allows the room to be merges with others during creation.
    /// Set it to false for rooms that should not change size/ form.
    /// </summary>
    public bool canMerge = true;

    /// <summary>
    /// Stores the room that is closest to this one.
    /// Uses during room generation.
    /// </summary>
    public Room closestRoom;
    public int closestDistance;

    /// <summary>
    /// Determines if new door connections can be created.
    /// </summary>
    public bool canAddDoors = true;

    public bool connectedToStart = false;

    private RoomType currentType;


    public Room(Primitives primitive)
    {
        Init(primitive);
        connectedRooms = new List<Room>();
    }

    public Room(Primitives primitive, bool canMerge, bool canAddDoors)
    {
        Init(primitive);
        this.canAddDoors = canAddDoors;
        this.canMerge = canMerge;
    }

    private void Init(Primitives primitive)
    {
        primitives = new List<Primitives>();
        primitives.Add(primitive);
        UpdateCoordinates();
        currentType = RoomType.Normal;
    }

    /// <summary>
    /// Checks if this room overlaps with an other room,
    /// by checking each primitive against each other.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool DoesRoomOverlap(Room other)
    {
        foreach (Primitives p1 in primitives)
        {
            foreach (Primitives p2 in other.primitives)
            {
                if (p1.DoesCollide(p2))
                    return true;
            }
        }

        return false;
    }

    public void MergeRooms(Room other)
    {
        foreach (Primitives p in other.primitives)
        {
            primitives.Add(p.Copy());
            coordinates.UnionWith(p.GetCoordinates());
        }
        size = coordinates.Count;
    }

    private int GetSize()
    {
        return size;
    }

    private void UpdateCoordinates()
    {
        coordinates = new HashSet<Vector2Int>();
        foreach (Primitives p in primitives)
        {
            coordinates.UnionWith(p.GetCoordinates());
        }
        size = coordinates.Count;
    }

    public HashSet<Vector2Int> GetCoordinates()
    {
        return coordinates;
    }

    public bool IsConnectedTo(Room other)
    {
        return connectedRooms.Contains(other);
    }

    public void ConnectNewRoom(Room other)
    {
        if(connectedRooms.Contains(other))
            return;

        connectedRooms.Add(other);
        if (other.connectedToStart)
        {
            ConnectToStart();
        }
    }

    /// <summary>
    /// Goes recusively through all connected room and
    /// set the once that weren't connecte be.
    /// </summary>
    private void ConnectToStart()
    {
        connectedToStart = true;
        foreach (Room r in connectedRooms)
        {
            if (!r.connectedToStart)
                r.ConnectToStart();
        }
    }

    public List<Room> GetConnectedRoom()
    {
        return connectedRooms;
    }

    public RoomType GetRoomType()
    {
        return currentType;
    }

    public void ChangeRoomType(RoomType type)
    {
        currentType = type;
    }

    public bool CanAddTunnel()
    {
        switch (currentType)
        {
            case RoomType.Normal:
                return true;
            case RoomType.Start:
            case RoomType.End:
                if (connectedRooms.Count == 0)
                    return true;
                else
                    return false;
            default:
                return true;
        }
    }
}

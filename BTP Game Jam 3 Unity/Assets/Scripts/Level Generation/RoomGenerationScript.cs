using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RoomGenerationScript : MonoBehaviour
{
    [SerializeField]
    private bool spawnInExtras = true;

    [SerializeField]
    private TileBase wallTile;
    [SerializeField]
    private TileBase floorTile;
    [SerializeField]
    private TileBase doorRightTile;
    [SerializeField]
    private TileBase doorLeftTile;

    [SerializeField]
    private TileBaseComposer tileComposer;

    [SerializeField]
    private Tilemap wallTileMap;
    [SerializeField]
    private Tilemap doorTileMap;
    [SerializeField]
    private Tilemap floorTileMap;
    [SerializeField]
    private GameObject shootingEnemy;
    [SerializeField]
    private GameObject broomEnemy;
    [SerializeField]
    private GameObject slimeEnemy;
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject roomLight;
    [SerializeField]
    private ShadowMap shadowMap;

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    [SerializeField]
    private float roomCount;
    [SerializeField]
    private int roomWidth;
    [SerializeField]
    private int roomHeight;
    [SerializeField]
    private int maxRoomPrimitives;
    [SerializeField]
    private int maxRoomSize;
    [SerializeField]
    private int minRoomSize;
    [SerializeField]
    private int maxEnemiesPerRoom;
    [SerializeField]
    private int straightTunnelMax;

    [Range(0, 100)]
    [SerializeField]
    private float tunnelRate;
    
    private List<Room> rooms;
    private List<Tunnel> floors;


    /* General map guide:
     * 0 Floor
     * 1 Wall
     * 2 Door
     */

    private int[,] currentMap;
    private int startRoomIndex;
    private int endRoomIndex;

    private int roomCreationAttempts = 0;

    private void ResetMap()
    {
        // Initialize with 1.
        for (int x = 0; x < currentMap.GetLength(0); x++)
        {
            for (int y = 0; y < currentMap.GetLength(1); y++)
            {
                currentMap[x, y] = -1;
            }
        }
    }

    /// <summary>
    /// This method generates the rooms.
    /// </summary>
    [ContextMenu("Generate")]
    public void GenerateMap()
    {
        roomCreationAttempts++;
        if (roomCreationAttempts > 10)
        {
            SceneManager.LoadScene("MainMenu");
        }

        rooms = new List<Room>();
        floors = new List<Tunnel>();
        currentMap = new int[width, height];

        ResetMap();

        // Make sure the while loop can't be endless.
        int rounds = 0;
        while (rooms.Count < roomCount)
        {
            rounds++;
            if (rounds > 300)
                break;
            AddRoom();
        }

        // Clear all room that are to small.
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].size < minRoomSize)
            {
                rooms.RemoveAt(i);
                i--;
            }
        }

        rooms[0].connectedToStart = true;

        ColorInRoom();

        // CreateTunnels();

        CreateDoubleTunnel();
        RemoveNotConnectedRoom();
        ResetMap();
        ColorInRoom();

        SetStartAndEndRoom();
        SetRooms();
        SetTunnels();
        Visualize(currentMap);

        if (spawnInExtras)
        {
            SpawnInEnemies();
            SpawnInLoot();
            SpawnInLights();
            SpawnShadows();
        }
    }

    private void SetStartAndEndRoom()
    {
        if (rooms.Count < 2)
        {
            // This map would work create a new one.
            GenerateMap();
            // throw new Exception("Not enough rooms");
        }


        // TODO have a minimum distance between start and end room.
        startRoomIndex = UnityEngine.Random.Range(0, rooms.Count);
        rooms[startRoomIndex].ChangeRoomType(RoomType.Start);
        endRoomIndex = UnityEngine.Random.Range(0, rooms.Count);
        while (endRoomIndex == startRoomIndex)
        {
            startRoomIndex = UnityEngine.Random.Range(0, rooms.Count);
        }
        rooms[endRoomIndex].ChangeRoomType(RoomType.End);
    }

    private void AddRoom()
    {
        int width = UnityEngine.Random.Range(roomWidth / 2, roomWidth);
        int height = UnityEngine.Random.Range(roomHeight / 2, roomHeight);

        int xOrigin = UnityEngine.Random.Range(1, currentMap.GetLength(0) - width - 1);
        int yOrigin = UnityEngine.Random.Range(1, currentMap.GetLength(1) - height - 1);

        RectAnglePrimitive newRect = new RectAnglePrimitive(new Vector2Int(xOrigin, yOrigin), new Vector2Int(width, height));

        Room newRoom = new Room(newRect);

        // Check if this room poses any problems.
        List<Room> overlappinRooms = new List<Room>();
        // Adds up all primitives that the new potential room would have.
        int primitiveCountOfNewRoom = 1;
        int sizeOfNewRoom = newRect.GetSize();

        foreach (Room r in rooms)
        {
            if (newRoom.DoesRoomOverlap(r))
            {
                // Found an overlapping room that can't be merged.
                if (!r.canMerge)
                    return;

                primitiveCountOfNewRoom += r.primitives.Count;
                overlappinRooms.Add(r);
                sizeOfNewRoom += r.size;
            }
        }

        // Check if the new room would be to big.
        if (sizeOfNewRoom > maxRoomSize)
            return;
        // Check if the new room would have more primitives that allowed.
        if (primitiveCountOfNewRoom > maxRoomPrimitives)
            return;


        foreach (Room r in overlappinRooms)
        {
            rooms.Remove(r);
            newRoom.MergeRooms(r);
        }

        rooms.Add(newRoom);
    }

    /// <summary>
    /// To connect all room, the map gets first colored in by the room.
    /// Eg. 2 = this tile belongs to the room at rooms[2]
    /// </summary>
    private void ColorInRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            foreach (Vector2Int c in rooms[i].GetCoordinates())
            {
                currentMap[c.x, c.y] = i;
            }
        }
    }

    /// <summary>
    /// Creates tunnels with a given width.
    /// Will only create tunnels if to rooms are facing each other and their distance is smaller than straightTunnelMax.
    /// Both walls were the tunnels connect need to be straight.
    /// </summary>
    private void CreateDoubleTunnel()
    {
        // TODO: refactor.
        int tunnelWidth = 2;

        // Go throught the map.
        for (int x = 0; x < currentMap.GetLength(0) -  1; x++)
        {
            for (int y = 0; y < currentMap.GetLength(1); y++)
            {
                if (currentMap[x, y] == -1)
                    continue;

                CheckHorizonzalDoubleTunnel(x, y, tunnelWidth);

                CheckVerticalDoubleTunnel(x, y, tunnelWidth);
            }
        }
    }

    private void CheckHorizonzalDoubleTunnel(int x, int y, int tunnelWidth)
    {
        int endX = 0;
        bool failedToConnect = false;

        int startRoomIndex = currentMap[x, y];

        if (startRoomIndex < 0 || startRoomIndex >= rooms.Count)
            return;

        Room startRoom = rooms[startRoomIndex];

        Room endRoom = null;
        int endRoomIndex = 0;


        // Check in x direction.
        // Go throught for the whole width of the tunnel.
        for (int i = 0; i < tunnelWidth && y + i < currentMap.GetLength(1) && !failedToConnect; i++)
        {
            // Check if this a tile next to a wall and a potential connecting point for a tunnel.
            if (currentMap[x + 1, y + i] != -1)
                break;

            bool canConnectInThisRow = false;

            // Try to find an other room within the maximum tunnel legth.
            for (int j = 0; j < straightTunnelMax && j + x < currentMap.GetLength(0) && !failedToConnect; j++)
            {
                int value = currentMap[x + j, y + i];

                if (value == -1 || value == startRoomIndex)
                    continue;

                // First to hit an other room.
                if (endRoom == null)
                {
                    if (startRoom.IsConnectedTo(rooms[value]))
                    {
                        failedToConnect = true;
                        break;
                    }

                    endRoomIndex = value;
                    endRoom = rooms[endRoomIndex];
                    endX = x + j;
                    canConnectInThisRow = true;
                    break;
                }
                else
                {
                    // Check if the connection point has the same y as the previous one.
                    if (endX != x + j)
                        failedToConnect = true;
                    else
                        canConnectInThisRow = true;
                    break;
                }
            }

            if (!canConnectInThisRow)
                failedToConnect = true;
        }

        if (!failedToConnect && startRoom != null && endRoom != null)
        {
            float ran = UnityEngine.Random.Range(0, 100);

            if (ran < tunnelRate)
            {
                rooms[startRoomIndex].ConnectNewRoom(rooms[endRoomIndex]);
                rooms[endRoomIndex].ConnectNewRoom(rooms[startRoomIndex]);

                RectAnglePrimitive rext = new RectAnglePrimitive(new Vector2Int(x + 1, y), new Vector2Int(endX - x - 1, tunnelWidth));
                floors.Add(new Tunnel(new Primitives[] { rext }, TunnelDirection.Horizontal));
            }
        }
    }

    private void CheckVerticalDoubleTunnel(int x, int y, int tunnelWidth)
    {
        int endY = 0;
        bool failedToConnect = false;

        int startRoomIndex = currentMap[x, y];

        if (startRoomIndex < 0 || startRoomIndex >= rooms.Count)
            return;

        Room startRoom = rooms[startRoomIndex];

        Room endRoom = null;
        int endRoomIndex = 0;


        // Check in x direction.
        // Go throught for the whole width of the tunnel.
        for (int i = 0; i < tunnelWidth && x + i < currentMap.GetLength(0) && !failedToConnect; i++)
        {
            // Check if this a tile next to a wall and a potential connecting point for a tunnel.
            if (currentMap[x + i, y + 1] != -1)
                break;

            bool canConnectInThisRow = false;

            // Try to find an other room within the maximum tunnel legth.
            for (int j = 0; j < straightTunnelMax && j + y < currentMap.GetLength(1) && !failedToConnect; j++)
            {
                int value = currentMap[x + i, y + j];

                if (value == -1 || value == startRoomIndex)
                    continue;

                // First to hit an other room.
                if (endRoom == null)
                {
                    if (startRoom.IsConnectedTo(rooms[value]))
                    {
                        failedToConnect = true;
                        break;
                    }

                    endRoomIndex = value;
                    endRoom = rooms[endRoomIndex];
                    endY = y + j;
                    canConnectInThisRow = true;
                    break;
                }
                else
                {
                    // Check if the connection point has the same y as the previous one.
                    if (endY != y + j)
                        failedToConnect = true;
                    else
                        canConnectInThisRow = true;
                    break;
                }
            }

            if (!canConnectInThisRow)
                failedToConnect = true;
        }

        if (!failedToConnect && startRoom != null && endRoom != null)
        {
            float ran = UnityEngine.Random.Range(0, 100);

            if (ran < tunnelRate)
            {
                rooms[startRoomIndex].ConnectNewRoom(rooms[endRoomIndex]);
                rooms[endRoomIndex].ConnectNewRoom(rooms[startRoomIndex]);

                RectAnglePrimitive rext = new RectAnglePrimitive(new Vector2Int(x, y + 1), new Vector2Int(tunnelWidth, endY - y - 1));
                floors.Add(new Tunnel(new Primitives[] { rext }, TunnelDirection.Horizontal));
            }
        }
    }

    private void CreateTunnels()
    {
        rooms[0].connectedToStart = true;

        foreach (Room r1 in rooms)
        {
            foreach (Primitives p1 in r1.primitives)
            {
                foreach (Room r2 in rooms)
                {
                    // A variable to break out of the loops once a connection is established.
                    bool didConnect = false;
                    // Check if it is the same room or if there already is a direct connection.
                    if (r1.Equals(r2) || r1.IsConnectedTo(r2))
                        break;
                    if (!r1.CanAddTunnel() || !r2.CanAddTunnel())
                        break;

                    foreach (Primitives p2 in r2.primitives)
                    {
                        int xDistance = p1.GetXDistance(p2);
                        int yDistance = p1.GetYDistance(p2);

                        Vector2Int[] ConnectionPoints = null;

                        if (xDistance < 0 && yDistance <= straightTunnelMax)
                        {
                            ConnectionPoints = p1.GetConnectionPoints(p2);
                        }
                        else if (yDistance < 0 && xDistance <= straightTunnelMax)
                        {
                            ConnectionPoints = p1.GetConnectionPoints(p2);
                        }

                        if (ConnectionPoints != null)
                        {
                            Vector2Int start = ConnectionPoints[0];
                            for (int i = 1; i < ConnectionPoints.Length; i++)
                            {
                                Vector2Int dir = (ConnectionPoints[i] - start);
                                float mag = dir.magnitude;
                                dir.x = (int)(dir.x / mag);
                                dir.y = (int)(dir.y / mag);

                                Vector2Int current = start;
                                while (current.x != ConnectionPoints[i].x || current.y != ConnectionPoints[i].y)
                                {
                                    currentMap[current.x, current.y] = 0;
                                    current += dir;
                                }
                                start = ConnectionPoints[i];
                            }

                            currentMap[ConnectionPoints[0].x, ConnectionPoints[0].y] = 2;
                            currentMap[ConnectionPoints[ConnectionPoints.Length - 1].x, ConnectionPoints[ConnectionPoints.Length - 1].y] = 2;

                            r1.ConnectNewRoom(r2);
                            r2.ConnectNewRoom(r1);
                            didConnect = true;
                            break;
                        }
                    }
                }
            }
        }
    }

    private void DebugPrintRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            string roomLog = "";
            roomLog += "id: " + i.ToString();
            roomLog += "\nconnected to: ";

            foreach (Room r in rooms[i].GetConnectedRoom())
            {
                roomLog += " " + rooms.IndexOf(r).ToString() + ",";

            }

            roomLog += "\nPrimitives:\n";
            foreach (Primitives p in rooms[i].primitives)
            {
                roomLog += p.ToString() + "; ";
            }
            Debug.Log(roomLog);
        }
    }

    private void SpawnInEnemies()
    {
        foreach (Room r in rooms)
        {
            if (r.GetRoomType() != RoomType.Normal)
                continue;

            int enemiesInThisRoom = 0;

            foreach (Vector2Int c in r.GetCoordinates())
            {
                bool nextToWall = false;

                for (int xLocal = -1; xLocal < 2 && !nextToWall; xLocal++)
                {
                    for (int yLocal = -1; yLocal < 2 && !nextToWall; yLocal++)
                    {
                        if (currentMap[c.x + xLocal, c.y + yLocal] < 0)
                        {
                            nextToWall = true;
                        }
                    }
                }

                if (nextToWall)
                    continue;

                float ran = UnityEngine.Random.Range(0, 100);

                if (ran <= 0.05f)
                {
                    Instantiate(shootingEnemy, floorTileMap.CellToWorld(new Vector3Int(c.x, c.y, 0)), Quaternion.identity);
                    enemiesInThisRoom++;
                    if (enemiesInThisRoom >= maxEnemiesPerRoom)
                        break;
                    continue;
                }
                ran = UnityEngine.Random.Range(0, 100);

                if (ran <= 0.03f)
                {
                    Instantiate(slimeEnemy, floorTileMap.CellToWorld(new Vector3Int(c.x, c.y, 0)), Quaternion.identity);
                    enemiesInThisRoom++;
                    if (enemiesInThisRoom >= maxEnemiesPerRoom)
                        break;
                    continue;
                }
                ran = UnityEngine.Random.Range(0, 100);

                if (ran <= 0.02f)
                {
                    Instantiate(broomEnemy, floorTileMap.CellToWorld(new Vector3Int(c.x, c.y, 0)), Quaternion.identity);
                    enemiesInThisRoom++;
                    if (enemiesInThisRoom >= maxEnemiesPerRoom)
                        break;
                    continue;
                }
            }
        }
    }

    private void SpawnInLights()
    {
        foreach (Room r in rooms)
        {
            if (r.GetRoomType() != RoomType.Normal)
                continue;

            foreach (Vector2Int c in r.GetCoordinates())
            {
                float ran = UnityEngine.Random.Range(0, 100);
                if (ran <= 0.02f)
                {
                    Instantiate(roomLight, floorTileMap.CellToWorld(new Vector3Int(c.x, c.y, 0)), Quaternion.identity);
                }
            }
        }
    }

    private void SpawnInLoot()
    {
        foreach (Room r in rooms)
        {
            if (r.GetRoomType() != RoomType.Normal)
                continue;

            foreach (Vector2Int c in r.GetCoordinates())
            {
                float ran = UnityEngine.Random.Range(0, 100);
                if (ran <= 0.05f)
                {
                    Instantiate(coin, floorTileMap.CellToWorld(new Vector3Int(c.x, c.y, 0)), Quaternion.identity);
                }
            }
        }
    }

    private void SpawnShadows()
    {
        for (int x = 0; x < currentMap.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < currentMap.GetLength(1); y++)
            {
                if (currentMap[x, y] != -1)
                    continue;

                bool closeToFloor = false;
                int shadowIndex = 0;


                for (int xLocal = -1; xLocal < 2; xLocal++)
                {
                    for (int yLocal = -1; yLocal < 2; yLocal++)
                    {
                        if (x + xLocal < 0)
                        {
                            if(yLocal == 0)
                                shadowIndex += 1;
                            continue;
                        }

                        if (x + xLocal >= currentMap.GetLength(0))
                        {
                            if (yLocal == 0)
                                shadowIndex += 8;
                            continue;
                        }

                        if (y + yLocal < 0)
                        {
                            if (xLocal == 0)
                                shadowIndex += 4;
                            continue;
                        }

                        if (y + yLocal >= currentMap.GetLength(1))
                        {
                            if (xLocal == 0)
                                shadowIndex += 2;
                            continue;
                        }

                        if (currentMap[x + xLocal, y + yLocal] != -1)
                            closeToFloor = true;
                        else
                        {
                            // Is wall look at what diretion.
                            if (xLocal == -1 && yLocal == 0)
                                shadowIndex += 1;
                            if (xLocal == 0 && yLocal == 1)
                                shadowIndex += 2;
                            if (xLocal == 0 && yLocal == -1)
                                shadowIndex += 4;
                            if (xLocal == 1 && yLocal == 0)
                                shadowIndex += 8;
                        }
                    }
                }

                if (closeToFloor)
                {
                    GameObject shadowInstance = Instantiate(shadowMap.GetShadow(shadowIndex), wallTileMap.CellToWorld(new Vector3Int(x, y, 0)) + Vector3.up * 0.5f, Quaternion.identity);
                    shadowInstance.transform.localScale = wallTileMap.transform.parent.localScale;
                    shadowInstance.transform.SetParent(transform);
                }
            }
        }
    }

    private void SetRooms()
    {
        foreach (Room r in rooms)
        {
            foreach (Primitives p in r.primitives)
            {
                foreach (Vector2Int v in p.GetCoordinates())
                {
                    currentMap[v.x, v.y] = 0;
                }
                /*
                if (p is RectAnglePrimitive)
                {
                    RectAnglePrimitive rectP = (RectAnglePrimitive)p;
                    for (int x = 0; x < rectP.size.x; x++)
                    {
                        for (int y = 0; y < rectP.size.y; y++)
                        {
                            currentMap[rectP.origin.x + x, rectP.origin.y + y] = 0;
                        }
                    }
                }
                */
            }
        }
    }

    private void RemoveNotConnectedRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (!rooms[i].connectedToStart)
            {
                rooms.RemoveAt(i);
                i--;
            }
        }
    }

    private void SetTunnels()
    {
        foreach (Tunnel f in floors)
        {
            foreach (Vector2Int v in f.GetCoordinates())
            {
                currentMap[v.x, v.y] = 1;
                // wallTileMap.SetTile(new Vector3Int(v.x, v.y, 0), doorLeftTile);
            }
        }
    }

    /// <summary>
    /// Method that translates the given map to the actuall tilemaps.
    /// </summary>
    /// <param name="map"></param>
    private void Visualize(int[,] map)
    {
        tileComposer.Init();

        wallTileMap.ClearAllTiles();
        floorTileMap.ClearAllTiles();
        doorTileMap.ClearAllTiles();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int mapValue = map[x, y];

                if (mapValue == -1)
                {
                    // Is wall, check if floor is nearby in a n 3>3 field around.
                    bool nextToFloor = false;
                    bool nextToTunnel = false;
                    bool vertical = true;
                    bool horizontal = true;


                    for (int xLocal = -1; xLocal < 2; xLocal++)
                    {
                        for (int yLocal = -1; yLocal < 2; yLocal++)
                        {
                            if (x + xLocal < 0 || x + xLocal >= map.GetLength(0))
                                continue;
                            if (y + yLocal < 0 || y + yLocal >= map.GetLength(1))
                                continue;

                            if (map[x + xLocal, y + yLocal] > -1)
                            {
                                if (yLocal == 0)
                                    horizontal = false;
                                if (xLocal == 0)
                                    vertical = false;

                                if (map[x + xLocal, y + yLocal] == 0)
                                    nextToFloor = true;
                                if (map[x + xLocal, y + yLocal] == 1)
                                    nextToTunnel = true;
                            }
                        }
                    }

                    /*
                    if ((x > 0  && map[x - 1, y] == -1) && (y > 0 && map[x, y - 1] == -1))
                        continue;
                    */

                    if (!nextToFloor && !nextToTunnel)
                        continue;

                    TileBase tile = tileComposer.GetWallTileBase(x, y, (nextToTunnel), horizontal, (!horizontal && !vertical));
                    wallTileMap.SetTile(new Vector3Int(x, y, 0), tile);
                }
                else
                {
                    TileBase tile = tileComposer.GetFloorTileBase(x, y, (mapValue == 1));
                    floorTileMap.SetTile(new Vector3Int(x, y, 0), tile);
                }

                /*
                else if (mapValue == 0)
                {
                    floorTileMap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
                else if (mapValue == 2)
                {
                    floorTileMap.SetTile(new Vector3Int(x, y, 0), floorTile);
                    // TODO
                    continue;

                    if (map[x + 1, y] == 1)
                        doorTileMap.SetTile(new Vector3Int(x, y, 0), doorLeftTile);
                    else
                        doorTileMap.SetTile(new Vector3Int(x, y, 0), doorRightTile);
                }
                */
            }
        }
    }

    public Vector3 GetStartPosition()
    {
        return wallTileMap.CellToWorld(rooms[startRoomIndex].primitives[0].GetCentrer());
    }
    public Vector3 GetExitPosition()
    {
        return wallTileMap.CellToWorld(rooms[endRoomIndex].primitives[0].GetCentrer());
    }
}

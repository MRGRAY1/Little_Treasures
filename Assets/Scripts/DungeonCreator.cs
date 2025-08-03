using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public static DungeonCreator Instance;

    public enum Direction { North = 0, East = 1, South = 2, West = 3 }

    public class Cell
    {
        public bool visited;
        public bool[] status = new bool[4]; // 0=N, 1=E, 2=S, 3=W
        public bool isBossRoom;
        public bool isStartRoom;
    }

    [Header("Dungeon Settings")]
    public Vector2Int size = new Vector2Int(9, 9);
    public int totalRooms = 12;
    public int mainPathLength = 6;
    [Range(0f, 1f)] public float branchChance = 0.4f;

    [Header("Room Prefab & Positioning")]
    public GameObject room;
    public Vector2 offset = new Vector2(12, 12);

    private Cell[,] grid;
    private int roomCount = 0;
    private Vector2Int startPos;
    private List<Vector2Int> mainPathRooms = new();

    private Transform spawnPoint;

    public bool IsGenerationComplete { get; private set; }
    private void Awake()
    {
        Instance = this;  // Always update instance on load
        IsGenerationComplete = false;
    }
    public void StartGenerateDungeon()
    {
        Logger.Log("Dungeon generation started");

        // Reset before generating
        IsGenerationComplete = false;
        roomCount = 0;
        mainPathRooms.Clear();

        GenerateDungeon();
        spawnPoint = transform;
    }

    private void GenerateDungeon()
    {
        grid = new Cell[size.x, size.y];
        for (int x = 0; x < size.x; x++)
            for (int y = 0; y < size.y; y++)
                grid[x, y] = new Cell();

        startPos = new Vector2Int(size.x / 2, size.y / 2);
        GenerateMainPath();
        GenerateBranches();
        InstantiateDungeonFromGrid();

        Logger.Log($"Dungeon generated with {roomCount} rooms");
        IsGenerationComplete = true;
    }

    private void GenerateMainPath()
    {
        Vector2Int current = startPos;
        grid[current.x, current.y].visited = true;
        grid[current.x, current.y].isStartRoom = true;
        roomCount++;
        mainPathRooms.Add(current);

        for (int i = 0; i < mainPathLength - 1; i++)
        {
            List<Direction> validDirs = GetAvailableDirections(current);

            if (validDirs.Count == 0)
                break;

            Direction dir = validDirs[Random.Range(0, validDirs.Count)];
            Vector2Int next = GetOffset(current, dir);

            grid[next.x, next.y].visited = true;
            roomCount++;
            mainPathRooms.Add(next);

            grid[current.x, current.y].status[(int)dir] = true;
            grid[next.x, next.y].status[(int)Opposite(dir)] = true;

            current = next;
        }

        grid[current.x, current.y].isBossRoom = true;
    }

    private void GenerateBranches()
    {
        foreach (Vector2Int roomPos in mainPathRooms)
        {
            if (roomCount >= totalRooms)
                return;

            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                if (Random.value > branchChance)
                    continue;

                Vector2Int branch = GetOffset(roomPos, dir);

                if (!IsInBounds(branch.x, branch.y) || grid[branch.x, branch.y].visited)
                    continue;

                // Prevent rooms from touching others without connection
                bool invalidNeighbor = false;

                foreach (Direction checkDir in System.Enum.GetValues(typeof(Direction)))
                {
                    Vector2Int neighbor = GetOffset(branch, checkDir);
                    if (!IsInBounds(neighbor.x, neighbor.y))
                        continue;

                    if (grid[neighbor.x, neighbor.y].visited && neighbor != roomPos)
                    {
                        // If the neighbor has no door facing this new branch, it's invalid
                        if (!grid[neighbor.x, neighbor.y].status[(int)Opposite(checkDir)])
                        {
                            invalidNeighbor = true;
                            break;
                        }
                    }
                }

                if (invalidNeighbor)
                    continue;

                // Create the connection
                grid[roomPos.x, roomPos.y].status[(int)dir] = true;
                grid[branch.x, branch.y].status[(int)Opposite(dir)] = true;

                grid[branch.x, branch.y].visited = true;
                roomCount++;

                if (roomCount >= totalRooms)
                    return;
            }
        }
    }

    private List<Direction> GetAvailableDirections(Vector2Int pos)
    {
        List<Direction> dirs = new();
        foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
        {
            Vector2Int next = GetOffset(pos, dir);
            if (IsInBounds(next.x, next.y) && !grid[next.x, next.y].visited)
                dirs.Add(dir);
        }
        return dirs;
    }

    private Vector2Int GetOffset(Vector2Int pos, Direction dir)
    {
        return dir switch
        {
            Direction.North => new Vector2Int(pos.x, pos.y + 1),
            Direction.East => new Vector2Int(pos.x + 1, pos.y),
            Direction.South => new Vector2Int(pos.x, pos.y - 1),
            Direction.West => new Vector2Int(pos.x - 1, pos.y),
            _ => pos
        };
    }

    private Direction Opposite(Direction dir)
    {
        return (Direction)(((int)dir + 2) % 4);
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < size.x && y < size.y;
    }

    private void InstantiateDungeonFromGrid()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (!grid[x, y].visited)
                    continue;

                Vector3 position = new Vector3(x * offset.x, 0, y * offset.y);
                RoomBehavior newRoom = Instantiate(room, position, Quaternion.identity, transform).GetComponent<RoomBehavior>();

                newRoom.UpdateRoom(grid[x, y].status);
                newRoom.name = $"Room {x} - {y}";

                if (grid[x, y].isStartRoom)
                {
                    newRoom.SetType(RoomType.Start);

                    // Set the player’s spawn point from this room’s marker
                    if (newRoom.publicSpawnPoint != null)
                    {
                        spawnPoint = newRoom.publicSpawnPoint.transform;
                        if (PlayerManager.Instance != null)
                            PlayerManager.Instance.SetSpawnPoint(spawnPoint);
                    }
                }
                else if (grid[x, y].isBossRoom)
                {
                    newRoom.SetType(RoomType.Boss);
                }
                else
                {
                    newRoom.SetType(RoomType.Normal);
                }
            }
        }

        Debug.Log($"Dungeon generated with {roomCount} rooms");

        IsGenerationComplete = true;
    }
}

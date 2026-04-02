using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dungeon Creation Class that creates the dungeon with randomly generated rooms/directions for a unique level each time
/// </summary>
/// TODOS:
/// Fix ending boss room. Still doesnt spawn sometimes. 
/// Create Spawn points for items in rooms
/// Update for difficulty with game progression
public class DungeonCreator : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// 4 directions of a room
    /// </summary>
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    /// <summary>
    /// Represents a dungeon room in the grid
    /// </summary>
    public class Cell
    {
        public bool visited;
        public bool[] status = new bool[4]; // doors: 0=N,1=E,2=S,3=W
        public bool isStartRoom;
        public bool isBossRoom;
    }

    /// <summary>
    /// set base values for random generation
    /// </summary>
    [Header("Dungeon Settings")] public Vector2Int size = new Vector2Int(9, 9); // grid dimensions

    public int mainPathLength = 8; // how long the main path is
    public int minTotalRooms = 1; // minimum number of rooms 
    public int totalRooms = 15; // maximum number of rooms
    [Range(0f, 1f)] public float branchChance = 0.5f; // chance to add branches
    [Range(0f, 1f)] public float loopChance = 0.3f; // chance to add loops

    [Header("Room Prefab & Positioning")] public GameObject room;
    public Vector2 offset = new Vector2(12, 12); // spacing between rooms

    private Cell[,] grid;
    public List<GameObject> rooms;
    private Vector2Int startPos;
    public Vector3Variable playerSpawnPoint;

    private int visitedCount = 0;

    public BoolVariable CompleteCheck;

    #endregion

    #region Initialization Functions

    private void OnEnable()
    {
        GameEvents.RoomGenerationStart += Initialize;
    }

    private void OnDisable()
    {
        GameEvents.RoomGenerationStart -= Initialize;
    }

    /// <summary>
    /// Initialize the dungeon Creation
    /// </summary>
    public void Initialize(object sender)
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        // Initialize grid
        visitedCount = 0; // reset before generation
        grid = new Cell[size.x, size.y];
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
            grid[x, y] = new Cell();
        rooms = new List<GameObject>();

        startPos = new Vector2Int(0, 0);

        // 1. Generate main path
        GenerateMainPath();
        // 2. Generate branches
        GenerateBranches();
        // 3. Add loops for a grid-like network
        GenerateLoops();
        // 4. Find and place boss room
        FinalizeBossRoom();
        // 5. Instantiate room prefabs into the world
        InstantiateDungeonFromGrid();
        // 6. Send Event when dungeon is generated
        CompleteInit();
    }

    public void CompleteInit()
    {
        CompleteCheck.setValue(true);
        GameEvents.DungeonGenerationComplete?.Invoke(this);
    }

    #endregion

    #region Main Functions

    /// <summary>
    /// Generates the main path from the start position.
    /// </summary>
    private void GenerateMainPath()
    {
        Vector2Int current = startPos;
        MarkVisited(current.x, current.y);
        grid[current.x, current.y].isStartRoom = true;

        for (int i = 1; i < mainPathLength; i++)
        {
            List<Direction> validDirs = PickDirection(current);
            if (validDirs.Count == 0) break;

            Direction dir = validDirs[Random.Range(0, validDirs.Count)];
            Vector2Int next = GetOffset(current, dir);

            if (HasExtraNeighbors(next, current)) continue;

            // Connect rooms
            grid[current.x, current.y].status[(int)dir] = true;
            grid[next.x, next.y].status[(int)Opposite(dir)] = true;

            current = next;
            MarkVisited(current.x, current.y);
        }
    }

    /// <summary>
    /// Instantiate room prefabs for all visited cells.
    /// </summary>
    private void InstantiateDungeonFromGrid()
    {
        rooms.Clear();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (!grid[x, y].visited) continue;

                Vector3 position = new Vector3(x * offset.x, 0, y * offset.y);
                GameObject newRoom = Instantiate(room, position, Quaternion.identity, transform);

                rooms.Add(newRoom);
                RoomBehavior rb = newRoom.GetComponent<RoomBehavior>();
                rb.UpdateRoom(grid[x, y].status);

                if (grid[x, y].isStartRoom) rb.SetType(RoomType.Start);
                else if (grid[x, y].isBossRoom) rb.SetType(RoomType.Boss);
                else rb.SetType(RoomType.Normal);
            }
        }
    }

    /// <summary>
    /// Adds random branches from the main path
    /// until the total room count is reached.
    /// </summary>
    private void GenerateBranches()
    {
        List<Vector2Int> visitedRooms = new();
        for (int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
            if (grid[x, y].visited)
                visitedRooms.Add(new Vector2Int(x, y));

        foreach (Vector2Int pos in visitedRooms)
        {
            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                if (visitedCount >= totalRooms) return;

                // If we're under minTotalRooms, force expansion.
                // Otherwise, rely on branchChance.
                if (visitedCount < minTotalRooms || Random.value <= branchChance)
                {
                    Vector2Int branch = GetOffset(pos, dir);
                    if (!IsInBounds(branch.x, branch.y) || grid[branch.x, branch.y].visited) continue;
                    if (HasExtraNeighbors(branch, pos)) continue;

                    grid[pos.x, pos.y].status[(int)dir] = true;
                    grid[branch.x, branch.y].status[(int)Opposite(dir)] = true;
                    grid[branch.x, branch.y].visited = true;
                }
            }
        }
    }

    /// <summary>
    /// Randomly connects nearby rooms to create loops,
    /// which prevents the dungeon from being a simple tree.
    /// </summary>
    private void GenerateLoops()
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (!grid[x, y].visited) continue;

                foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
                {
                    Vector2Int neighbor = GetOffset(new Vector2Int(x, y), dir);
                    if (!IsInBounds(neighbor.x, neighbor.y)) continue;
                    if (!grid[neighbor.x, neighbor.y].visited) continue;

                    // Already connected?
                    if (grid[x, y].status[(int)dir]) continue;

                    // Chance to connect
                    if (Random.value < loopChance)
                    {
                        grid[x, y].status[(int)dir] = true;
                        grid[neighbor.x, neighbor.y].status[(int)Opposite(dir)] = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Finds the farthest room from the start,
    /// and ensures it becomes a dead-end boss room.
    /// </summary>
    private void FinalizeBossRoom()
    {
        // Step 1: BFS to find farthest room from start
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, int> dist = new Dictionary<Vector2Int, int>();

        queue.Enqueue(startPos);
        dist[startPos] = 0;

        Vector2Int farthest = startPos;
        int maxDist = 0;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            int currentDist = dist[current];

            if (currentDist > maxDist)
            {
                maxDist = currentDist;
                farthest = current;
            }

            for (int d = 0; d < 4; d++)
            {
                if (!grid[current.x, current.y].status[d]) continue;

                Vector2Int neighbor = GetOffset(current, (Direction)d);
                if (!IsInBounds(neighbor.x, neighbor.y)) continue;
                if (!grid[neighbor.x, neighbor.y].visited) continue;

                if (!dist.ContainsKey(neighbor))
                {
                    dist[neighbor] = currentDist + 1;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // Step 2: ensure farthest is a dead-end
        int connections = CountConnections(farthest);

        if (connections == 1)
        {
            // Already a dead-end
            grid[farthest.x, farthest.y].isBossRoom = true;
            return;
        }

        // Step 3: if possible, add a new room as a boss
        if (visitedCount < totalRooms)
        {
            List<Direction> availableDirs = PickDirection(farthest);
            if (availableDirs.Count > 0)
            {
                Direction dir = availableDirs[Random.Range(0, availableDirs.Count)];
                Vector2Int newRoomPos = GetOffset(farthest, dir);

                // Create connection to new boss room
                grid[farthest.x, farthest.y].status[(int)dir] = true;
                grid[newRoomPos.x, newRoomPos.y].status[(int)Opposite(dir)] = true;
                grid[newRoomPos.x, newRoomPos.y].visited = true;

                grid[newRoomPos.x, newRoomPos.y].isBossRoom = true;
                return;
            }
        }

        // Step 4: force prune to dead-end if no room can be added
        PruneToDeadEnd(farthest);
        grid[farthest.x, farthest.y].isBossRoom = true;
    }

    #endregion

    #region Helper Functions

    private void MarkVisited(int x, int y)
    {
        if (grid[x, y].visited) return; // don't double count
        grid[x, y].visited = true;
        visitedCount++;
    }

    private int CountConnections(Vector2Int pos)
    {
        int c = 0;
        foreach (bool d in grid[pos.x, pos.y].status)
            if (d)
                c++;
        return c;
    }

    private void PruneToDeadEnd(Vector2Int pos)
    {
        // Keep only the first valid connection, remove others
        bool keptOne = false;
        for (int d = 0; d < 4; d++)
        {
            if (!grid[pos.x, pos.y].status[d]) continue;

            if (!keptOne)
            {
                keptOne = true;
                continue; // keep first connection
            }

            // remove extra connections
            grid[pos.x, pos.y].status[d] = false;

            Vector2Int neighbor = GetOffset(pos, (Direction)d);
            if (IsInBounds(neighbor.x, neighbor.y))
                grid[neighbor.x, neighbor.y].status[(int)Opposite((Direction)d)] = false;
        }
    }

    private bool HasExtraNeighbors(Vector2Int cell, Vector2Int parent)
    {
        foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
        {
            Vector2Int neighbor = GetOffset(cell, dir);
            if (!IsInBounds(neighbor.x, neighbor.y)) continue;
            if (grid[neighbor.x, neighbor.y].visited && neighbor != parent) return true;
        }

        return false;
    }

    private List<Direction> PickDirection(Vector2Int pos)
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

    private Direction Opposite(Direction dir) => (Direction)(((int)dir + 2) % 4);

    private bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < size.x && y < size.y;

    #endregion

    #region Misc Functions

    /// <summary>
    /// Draw cubes in the rooms to visualize room types
    /// </summary>
    private void OnDrawGizmos()
    {
        if (grid == null) return;

        // Scale gizmos relative to room spacing
        Vector3 gizmoSize = new Vector3(offset.x * 0.6f, 1, offset.y * 0.6f);

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (!grid[x, y].visited) continue;

                Vector3 pos = new Vector3(x * offset.x, 0.25f, y * offset.y);

                if (grid[x, y].isStartRoom)
                    Gizmos.color = Color.green;
                else if (grid[x, y].isBossRoom)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.blue;

                Gizmos.DrawCube(pos, gizmoSize);
            }
        }
    }

    #endregion
}
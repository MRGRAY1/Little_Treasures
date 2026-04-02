using System.Linq;
using UnityEngine;

/// <summary>
/// Item Spawner Class responsible for spawning items
/// into valid dungeon rooms after dungeon generation.
/// </summary>
/// TODOS:
/// - Add weighted loot tables
/// - Scale spawn counts with difficulty
/// - Support room-specific item rules
public class ItemSpawner : MonoBehaviour
{
    #region Variables

    [Header("Item Settings")] [SerializeField]
    private ItemsToSpawn itemsToSpawn;

    [SerializeField] private IntVariable maxItemSpawnAmount;

    [Header("Dungeon References")] [SerializeField]
    private DungeonCreator dungeonCreator;

    public BoolVariable CompleteCheck;

    #endregion

    #region Initialization Functions

    private void OnEnable()
    {
        GameEvents.DungeonGenerationComplete += Initialize;
    }

    private void OnDisable()
    {
        GameEvents.DungeonGenerationComplete -= Initialize;
    }

    /// <summary>
    /// Initialize the item spawning process
    /// </summary>
    public void Initialize(object sender)
    {
        if (itemsToSpawn == null || dungeonCreator == null)
        {
            Debug.LogError("ItemSpawner missing required references");
            return;
        }

        SpawnItems();
    }

    #endregion

    #region Main Functions

    /// <summary>
    /// Iterates through all generated rooms and spawns items
    /// </summary>
    private void SpawnItems()
    {
        GameEvents.ItemsSpawnStart?.Invoke(this);
        foreach (GameObject room in dungeonCreator.rooms)
        {
            SpawnItemsInRoom(room);
        }

        CompleteInit();
    }

    /// <summary>
    /// Spawns items in a single room based on room type
    /// and available spawn locations
    /// </summary>
    private void SpawnItemsInRoom(GameObject room)
    {
        RoomBehavior rb = room.GetComponent<RoomBehavior>();

        // Only normal rooms spawn items
        if (rb == null || rb.room_Type != RoomType.Normal)
            return;

        Transform spawnParent = room.transform.Find("ItemSpawns");
        Transform itemParent = room.transform.Find("items");

        if (spawnParent == null || itemParent == null)
            return;

        // Get all child transforms under spawnParent, skip the parent itself
        Transform[] spawnPoints = spawnParent.GetComponentsInChildren<Transform>(true).Skip(1).ToArray(); // skip index 0, which is the parent

        // Skip if no valid spawn points exist
        if (spawnPoints.Length == 0)
            return;

        // Determine spawn count: at least 1, at most maxItemSpawnAmount or available points
        int maxPossible = spawnPoints.Length;
        int spawnCount = Mathf.Clamp(
            Random.Range(1, maxItemSpawnAmount.getValue() + 1),
            1,
            maxPossible
        );

        // Shuffle spawn points for randomness
        Shuffle(spawnPoints);

        // Spawn items at the first N points
        for (int i = 0; i < spawnCount; i++)
        {
            int randItem = Random.Range(0, itemsToSpawn.itemsToSpawn.Count);
            Instantiate(
                itemsToSpawn.itemsToSpawn[randItem],
                spawnPoints[i].position,
                Quaternion.identity,
                itemParent
            );
        }
    }

    public void CompleteInit()
    {
        CompleteCheck.setValue(true);
        GameEvents.ItemsSpawnComplete?.Invoke(this);
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Randomizes the order of spawn points to ensure
    /// unique and unbiased item placement
    /// </summary>
    private void Shuffle(Transform[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int j = Random.Range(i, array.Length);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    #endregion
}
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;   // Assign in Inspector

    public GameObject CurrentPlayer { get; private set; }
    public bool PlayerReady { get; private set; } = false;

    private Transform spawnPoint;

    /// <summary>
    /// Called by DungeonCreator to set where the player should spawn.
    /// </summary>
    public void SetSpawnPoint(Transform spawn)
    {
        spawnPoint = spawn;
    }

    /// <summary>
    /// Spawns the player prefab at the assigned spawn point.
    /// </summary>
    public void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned in PlayerManager!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point not set. Spawning player at origin.");
            spawnPoint = new GameObject("DefaultSpawnPoint").transform;
            spawnPoint.position = Vector3.zero;
        }

        // Destroy any previous player
        if (CurrentPlayer != null)
        {
            Destroy(CurrentPlayer);
        }

        // Instantiate player
        CurrentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        PlayerReady = true;
        Debug.Log("Player spawned successfully.");
    }
}

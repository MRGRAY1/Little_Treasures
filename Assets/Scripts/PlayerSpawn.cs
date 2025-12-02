using UnityEngine;

public class PlayerSpawn : InitializeItem
{
    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab; // Assign in Inspector
    [SerializeField] private Vector3Variable spawnPoint;

    public GameObject CurrentPlayer { get; private set; }
    public bool PlayerReady { get; private set; }

    public override void Initialize()
    {
        base.Initialize();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (spawnPoint == null)
        {
            Logger.LogWarning("Spawn point not set. Spawning player at origin.");
            spawnPoint = ScriptableObject.CreateInstance<Vector3Variable>();
            spawnPoint.setValue(Vector3.zero);
        }

        // Prevent spawning again if a valid player already exists
        if (CurrentPlayer != null)
        {
            if (CurrentPlayer != playerPrefab && CurrentPlayer.activeInHierarchy)
            {
                Logger.Log($"Player already exists: {CurrentPlayer.name}, skipping spawn.", CurrentPlayer);
                PlayerReady = true;
                return;
            }
            else
            {
                Logger.Log($"Destroying old or invalid player: {CurrentPlayer.name}");
                Destroy(CurrentPlayer);
            }
        }

        // Spawn new player
        Vector3 pos = spawnPoint.getValue();
        CurrentPlayer = Instantiate(playerPrefab, pos, Quaternion.identity);
        CurrentPlayer.name = playerPrefab.name;
        Logger.Log($"Spawned new player: {CurrentPlayer.name}", CurrentPlayer);

        PlayerReady = true;
        Logger.Log("Player done");
        CompleteInit();
    }

    public override void CompleteInit()
    {
        base.CompleteInit();
    }
}

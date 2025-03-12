using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int shipsToPark = 3;
    [SerializeField] List<Ship> ships = new List<Ship>();
    int shipsParked = 0;
    int currentLevel;

    [SerializeField] List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    int nextSpawnIndex = 0;

    // Singleton Setup
    public static LevelManager Instance;

    void Awake()
    {
        // Singleton Setup
        if (Instance != null) { Destroy(Instance); }
        Instance = this;
    }

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //ships.AddRange(FindObjectsOfType<Ship>(true));

        ships[0].transform.SetPositionAndRotation(spawnPoints[nextSpawnIndex].transform.position, spawnPoints[nextSpawnIndex].transform.rotation);
        nextSpawnIndex = (nextSpawnIndex + 1) % (spawnPoints.Count - 1);
        ships[0].gameObject.SetActive(true);
        ships[0].EnableAfterSeconds(1);
    }

    public void ParkedShip()
    {
        // Disable current ship
        ships[shipsParked].gameObject.SetActive(false);

        // iterate shipsParked
        shipsParked++;

        // Add Score
        GameManager.Instance.AddPoints(currentLevel * 1000); // Points for Parking Ship

        if (shipsParked >= shipsToPark)
        {
            // Add Score
            GameManager.Instance.AddPoints(currentLevel * 5000); // Points for completing Level

            // Next Level
            GameManager.Instance.TryLevelSucceeded();
        }
        else
        {
            // Spawn/Activate new ship
            int nextShipIndex = shipsParked % shipsToPark;
            ships[nextShipIndex].transform.SetPositionAndRotation(spawnPoints[nextSpawnIndex].transform.position, spawnPoints[nextSpawnIndex].transform.rotation);
            nextSpawnIndex = (nextSpawnIndex + 1) % (spawnPoints.Count);
            ships[nextShipIndex].gameObject.SetActive(true);
            ships[nextShipIndex].EnableAfterSeconds(0.1f);
        }
    }
}
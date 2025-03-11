using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int shipsToPark = 3;
    int shipsParked = 0;
    int currentLevel;

    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void ParkedShip()
    {
        // iterate shipsParked
        shipsParked++;

        // TODO : Add Score
        GameManager.Instance.AddPoints(currentLevel * 1000); // Points for Parking Ship

        if (shipsParked >= shipsToPark)
        {
            // TODO : Add Score
            GameManager.Instance.AddPoints(currentLevel * 5000); // Points for completing Level

            // TODO : Next Level
            GameManager.Instance.TryLevelSucceeded();
        }
        else
        {
            // TODO : Spawn new ship
        }
    }
}
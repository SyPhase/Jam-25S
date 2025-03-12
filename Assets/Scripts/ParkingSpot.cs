using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingSpot : MonoBehaviour
{
    const string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        // Note: Player tag must be on collider object!
        if (other.CompareTag(playerTag))
        {
            LevelManager.Instance.ParkedShip();
        }
    }
}
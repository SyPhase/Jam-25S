using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShip : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    Rigidbody rb;

    float timeSinceDirectionSwitch = 10f;

    const float timeToSwitch = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (timeSinceDirectionSwitch > 0f)
        {
            rb.AddTorque(0f, 0f, rotationSpeed);
        }
        else if (timeSinceDirectionSwitch < -timeToSwitch)
        {
            timeSinceDirectionSwitch = timeToSwitch;
        }
        else
        {
            rb.AddTorque(0f, 0f, -rotationSpeed);
        }

        timeSinceDirectionSwitch -= Time.fixedDeltaTime;
        rb.AddRelativeForce(0f, Random.Range(0.5f, 5f), 0f);
    }
}
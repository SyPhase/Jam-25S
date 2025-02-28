using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 10f;
    [SerializeField] float rotationSpeed = 3f;
    [SerializeField] float maxThrustSpeed = 10f;
    [SerializeField] float maxRotationSpeed = 5f;

    Rigidbody rb;

    float inputVertical = 0;
    float inputHorizontal = 0;
    [SerializeField] bool inputAllowed = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //rb.maxLinearVelocity = maxThrustSpeed;
        rb.maxAngularVelocity = maxRotationSpeed;
    }

    void Update()
    {
        inputVertical = -Input.GetAxis("Horizontal");
        inputHorizontal = Input.GetAxisRaw("Vertical");

        // idk why the input axis are swapped...
        //Debug.Log("\nHorizontal: " + inputHorizontal + "\nVertical: " + inputVertical);
    }

    void FixedUpdate()
    {
        if (!inputAllowed) { return; }

        rb.AddRelativeForce(0f, inputHorizontal * thrustSpeed, 0f);
        //rb.AddRelativeTorque(0f, 0f, inputVertical * rotationSpeed);

        if (inputVertical < 0.1f && inputVertical > -0.1f)
        {
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.angularVelocity = new Vector3(0f, 0f, inputVertical * rotationSpeed);
        }

        if (inputHorizontal < 0.1f && inputHorizontal > -0.1f)
        {
            rb.drag = 0.1f * thrustSpeed;
        }
        else
        {
            rb.drag = 0f;
        }

        //Debug.Log(rb.velocity + ", " + rb.angularVelocity);
    }
}
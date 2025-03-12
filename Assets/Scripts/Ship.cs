using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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

        rb.maxLinearVelocity = maxThrustSpeed;
        rb.maxAngularVelocity = maxRotationSpeed;
    }

    public void EnableAfterSeconds(float seconds = 0f)
    {
        StartCoroutine(DoEnableAfterSeconds(seconds));
    }

    public void ActivateShip(bool activate)
    {
        inputAllowed = activate;
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
    }

    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // TODO : make LevelManager or ShipManager to spawn ships
        }
    }
    */

    void OnCollisionEnter(Collision collision)
    {
        // Removes a "life"
        GameManager.Instance.RemoveLife();

        // TODO : Add voice line SFX here
    }

    /// <summary>
    /// Waits for number of seconds inputted
    /// </summary>
    /// <param name="seconds">seconds to wait</param>
    /// <returns></returns>
    IEnumerator DoEnableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        GameManager.Instance.TryStartLevel(this);
        //Debug.Log("Ship is Enabled!");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovementScript : MonoBehaviour
{
    public Vector3 lookDir;

    public float movementMultiplier = 0.05f;

    private void Start()
    {
        lookDir = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Forward
        if (Input.GetKey(KeyCode.W))
            transform.Translate(0, 0, lookDir.z * Time.deltaTime * movementMultiplier * -1);

        // Back
        if (Input.GetKey(KeyCode.S))
            transform.Translate(0, 0, lookDir.z * Time.deltaTime * movementMultiplier);

        // Left
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(transform.rotation.x, transform.rotation.y * Time.deltaTime - 1, transform.rotation.z);

        // Right
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(transform.rotation.x, transform.rotation.y * Time.deltaTime + 1, transform.rotation.z);

    }
}

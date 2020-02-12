using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    public GameObject touchTarget;
    public bool hasTouchedTarget = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.name == touchTarget.name)
        {
            Debug.Log("Touch Detected!");
            hasTouchedTarget = true;
        }
    }
}

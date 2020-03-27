using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    public string touchTargetTag;
    public bool hasTouchedTarget = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.tag == touchTargetTag)
        {
            Debug.Log("Touch Detected!");
            hasTouchedTarget = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Update () {
        /* Here we want to rotate the camera 
        for the x directionn we impose: current_position +- 1˚ 
        for the y directionn we impose: current_position +- 1˚ 
        for the z directionn we impose: current_position +- 1˚
        */
        
        transform.rotation = Quaternion.Euler(transform.eulerAngles.y - 1.0f + (1.0f - -(1.0f)) * Random.value, 
                                      transform.eulerAngles.y - 1.0f + (1.0f - -(1.0f)) * Random.value,
                                      transform.eulerAngles.z - 1.0f + (1.0f - -(1.0f)) * Random.value;
    }
}

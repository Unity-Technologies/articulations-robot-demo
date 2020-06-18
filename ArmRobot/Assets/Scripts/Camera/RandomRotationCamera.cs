using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationCamera : MonoBehaviour
{
    public float rotationCamera = 1.0f;
    // Start is called before the first frame update
    void Update () {
        /* Here we want to rotate the camera 
        for the x directionn we impose: current_position +- 1˚ 
        for the y directionn we impose: current_position +- 1˚ 
        for the z directionn we impose: current_position +- 1˚
        */
        
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x - rotationCamera + 2 * rotationCamera * Random.value, 
                                      transform.eulerAngles.y - rotationCamera + 2 * rotationCamera * Random.value,
                                      transform.eulerAngles.z - rotationCamera + 2 * rotationCamera * Random.value
        );
    }
}

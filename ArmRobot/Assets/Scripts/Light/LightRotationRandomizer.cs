using System.Collections;
using UnityEngine;

public class LightRotationRandomizer : MonoBehaviour {

    void Update () {
        /* Here we want to rotate the light direction
        for the x directionn we impose a random rotation between 40˚ and 80˚
        for the y directionn we impose a random rotation between -180˚ and 180˚
        for the z directionn we don't change anything 
        */
        
        transform.rotation = Quaternion.Euler(40f + (80f - 40f) * Random.value, 
                                      -180f + (180f - (-180f)) * Random.value,
                                      transform.eulerAngles.z);
    }
}

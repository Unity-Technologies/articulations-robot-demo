using System.Collections;
using UnityEngine;

public class LightRotationRandomizer : MonoBehaviour {

    public float minRangeRotationLightXAxis = 40f;
    public float maxRangeRotationLightXAxis = 80f;
    public float minRangeRotationLightYAxis = -180f;
    public float maxRangeRotationLightYAxis = 180f;
    void Update () {
        /* Here we want to rotate the light direction
        for the x directionn we impose a random rotation between 40˚ and 80˚
        for the y directionn we impose a random rotation between -180˚ and 180˚
        for the z directionn we don't change anything 
        */
        
        transform.rotation = Quaternion.Euler(minRangeRotationLightXAxis + (maxRangeRotationLightXAxis - minRangeRotationLightXAxis) * Random.value, 
                                            minRangeRotationLightYAxis + (maxRangeRotationLightYAxis - minRangeRotationLightYAxis) * Random.value,
                                            transform.eulerAngles.z);
    }
}

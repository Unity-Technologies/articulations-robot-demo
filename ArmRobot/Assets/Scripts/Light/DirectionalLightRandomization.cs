using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightRandomization : MonoBehaviour
{
    public float minRangeReducedRValue = 0.4f;
    public float maxRangeReducedRValue = 1.0f;
    public float minRangeReducedGValue = 0.4f;
    public float maxRangeReducedGValue = 1.0f;
    public float minRangeReducedBValue = 0.4f;
    public float maxRangeReducedBValue = 1.0f; 

    public float minRangeIntensityLight = 0.45f;
    public float maxRangeIntensityLight = 0.7f;

    public float minRangeRotationLightXAxis = 40f;
    public float maxRangeRotationLightXAxis = 80f;
    public float minRangeRotationLightYAxis = -180f;
    public float maxRangeRotationLightYAxis = 180f;

    Light lt;
    // Start is called before the first frame update
    void Start()
    {
        lt = GetComponent<Light>();
    }

    public void UpdateLight()
    {
        // We change the color of the light 
        float R_value = minRangeReducedRValue + (maxRangeReducedRValue - minRangeReducedRValue) * Random.value;
        float G_value = minRangeReducedGValue + (maxRangeReducedGValue - minRangeReducedGValue) * Random.value;
        float B_value = minRangeReducedBValue + (maxRangeReducedBValue - minRangeReducedBValue) * Random.value;

        lt.color = new Color(R_value, G_value, B_value, 1.0f);

        // Here we change the intensity of the light 

        lt.intensity =  minRangeIntensityLight + (maxRangeIntensityLight - minRangeIntensityLight) * Random.value;

        
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

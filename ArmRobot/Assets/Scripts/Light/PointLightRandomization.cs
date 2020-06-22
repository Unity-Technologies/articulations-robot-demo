using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightRandomization : MonoBehaviour
{
    public float minRangeReducedRValue = 0.4f;
    public float maxRangeReducedRValue = 1.0f;
    public float minRangeReducedGValue = 0.4f;
    public float maxRangeReducedGValue = 1.0f;
    public float minRangeReducedBValue = 0.4f;
    public float maxRangeReducedBValue = 1.0f; 

    public float minRangeIntensityLight = 0.4f;
    public float maxRangeIntensityLight = 0.8f;

    public float minRangePositionLightXAxis = -0.2f;
    public float maxRangePositionLightXAxis = 0.2f;
    public float minRangePositionLightYAxis = 2.5f;
    public float maxRangePositionLightYAxis = 3f;
    public float minRangePositionLightZAxis = -0.2f;
    public float maxRangePositionLightZAxis = 0.2f;

    public Vector4 ColorUpdate()
    {
        float R_value = minRangeReducedRValue + (maxRangeReducedRValue - minRangeReducedRValue) * Random.value;
        float G_value = minRangeReducedGValue + (maxRangeReducedGValue - minRangeReducedGValue) * Random.value;
        float B_value = minRangeReducedBValue + (maxRangeReducedBValue - minRangeReducedBValue) * Random.value;

        Vector4 color = new Vector4(R_value, G_value, B_value, 1.0f);
        return color;

    }
    public float IntensityUpdate()
    {
        float intensity =  minRangeIntensityLight + (maxRangeIntensityLight - minRangeIntensityLight) * Random.value;
        return intensity;
    }

    public Vector3 PositionUpdate () {
        Vector3 position = new Vector3(minRangePositionLightXAxis + (maxRangePositionLightXAxis - minRangePositionLightXAxis) * Random.value, 
                                            minRangePositionLightYAxis + (maxRangePositionLightYAxis - minRangePositionLightYAxis) * Random.value,
                                            minRangePositionLightZAxis + (maxRangePositionLightZAxis - minRangePositionLightZAxis) * Random.value);
        return position;
    }
}

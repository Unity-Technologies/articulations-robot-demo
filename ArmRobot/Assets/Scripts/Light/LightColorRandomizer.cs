using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorRandomizer : MonoBehaviour
{
    public float minRangeReducedRValue = 0.4f;
    public float maxRangeReducedRValue = 1.0f;
    public float minRangeReducedGValue = 0.4f;
    public float maxRangeReducedGValue = 1.0f;
    public float minRangeReducedBValue = 0.4f;
    public float maxRangeReducedBValue = 1.0f; 
    
    Light lt;

    void Start()
    {
        lt = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        /* I generate a random number between 0.4 and 1 for each channel of 
         * color 
         */

        float R_value = minRangeReducedRValue + (maxRangeReducedRValue - minRangeReducedRValue) * Random.value;
        float G_value = minRangeReducedGValue + (maxRangeReducedGValue - minRangeReducedGValue) * Random.value;
        float B_value = minRangeReducedBValue + (maxRangeReducedBValue - minRangeReducedBValue) * Random.value;

        lt.color = new Color(R_value, G_value, B_value, 1.0f);
    }
}

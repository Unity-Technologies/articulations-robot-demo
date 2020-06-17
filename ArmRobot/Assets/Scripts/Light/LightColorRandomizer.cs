using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorRandomizer : MonoBehaviour
{
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

        float R_value = 0.4f + (1.0f - 0.4f) * Random.value;
        float G_value = 0.4f + (1.0f - 0.4f) * Random.value;
        float B_value = 0.4f + (1.0f - 0.4f) * Random.value;

        lt.color = new Color(R_value, G_value, B_value, 1.0f);
    }
}

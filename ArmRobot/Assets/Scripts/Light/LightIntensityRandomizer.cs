using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityRandomizer : MonoBehaviour
{
    Light lt;

    void Start()
    {
        lt = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        // I generate a random number between 0.6 and 1.2 for the intensity 

        lt.intensity = 0.6f + (1.2f - 0.6f) * Random.value;
    }
}

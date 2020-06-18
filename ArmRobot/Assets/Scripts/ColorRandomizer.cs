using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    public float hueMin = 0f; 
    public float hueMax = 1f;
    public float saturationMin = 1f; 
    public float saturationMax = 1f;
    public float valueMin = 0.5f; 
    public float valueMax = 1f;
    // Update is called once per frame
    void Update()
    {
        foreach (Material mat in GetComponent<Renderer>().materials) {
            mat.color = Random.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax);
        }
    }
}

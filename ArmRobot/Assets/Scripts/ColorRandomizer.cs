using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        foreach (Material mat in GetComponent<Renderer>().materials) {
            mat.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }
}

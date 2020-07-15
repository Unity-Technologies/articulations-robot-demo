using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CameraPixel cameraPixel = GetComponent<CameraPixel>();
        cameraPixel.GetPixelPosition();
    }

    // Update is called once per frame
}

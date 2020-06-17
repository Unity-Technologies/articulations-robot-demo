using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionCamera : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        float x_value = transform.position.x - 0.005f + (0.005f - (-0.005f)) * Random.value;
        float y_value = transform.position.y - 0.005f + (0.005f - (-0.005f)) * Random.value;
        float z_value = transform.position.z - 0.005f + (0.005f - (-0.005f)) * Random.value;
        transform.position = new Vector3(x_value, y_value, z_value);
    }
}

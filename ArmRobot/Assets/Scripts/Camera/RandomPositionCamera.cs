using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionCamera : MonoBehaviour
{
    public float changePositionCamera = 0.005f;

    // Update is called once per frame
    void Update()
    {
        float x_value = transform.position.x - changePositionCamera + 2 * changePositionCamera * Random.value;
        float y_value = transform.position.y - changePositionCamera + 2 * changePositionCamera * Random.value;
        float z_value = transform.position.z - changePositionCamera + 2 * changePositionCamera * Random.value;
        transform.position = new Vector3(x_value, y_value, z_value);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerPositionCamera : MonoBehaviour
{
    public float changePositionCamera = 0.005f;
    public float rotationCamera = 1.0f;

    public Vector3 initialCameraPosition = new Vector3(0.0f, 1.8f, 0.0f);

    public Vector3 initialCameraRotation = new Vector3(90.0f, 0.0f, 0.0f);
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = initialCameraPosition;
        transform.rotation = Quaternion.Euler(initialCameraRotation[0], initialCameraRotation[1], initialCameraRotation[2]);
    }

    
    public void Move() {
        
        // Here we want to move the position of the camera
        
        float x_value = initialCameraPosition[0] + changePositionCamera + 2 * changePositionCamera * Random.value;
        float y_value = initialCameraPosition[1] + changePositionCamera + 2 * changePositionCamera * Random.value;
        float z_value = initialCameraPosition[2] + changePositionCamera + 2 * changePositionCamera * Random.value;
        transform.position = new Vector3(x_value, y_value, z_value);
        
        
        /* Here we want to rotate the camera 
        for the x directionn we impose: current_position +- 1˚ 
        for the y directionn we impose: current_position +- 1˚ 
        for the z directionn we impose: current_position +- 1˚
        */
        
        transform.rotation = Quaternion.Euler(initialCameraRotation[0] - rotationCamera + 2 * rotationCamera * Random.value, 
                                      initialCameraRotation[1] - rotationCamera + 2 * rotationCamera * Random.value,
                                      initialCameraRotation[2] - rotationCamera + 2 * rotationCamera * Random.value
        );
        
    }
}

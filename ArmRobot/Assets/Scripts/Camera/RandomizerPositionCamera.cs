using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerPositionCamera : MonoBehaviour
{
    public float changePositionCamera = 0.005f;
    public float initialCameraPositionXAxis = 0.0f;
    public float initialCameraPositionYAxis = 1.8f;
    public float initialCameraPositionZAxis = 0.0f;
    
    public float rotationCamera = 1.0f;
    public float initialCameraRotationXAxis = 90.0f;
    public float initialCameraRotationYAxis = 0.0f;
    public float initialCameraRotationZAxis = 0.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(initialCameraPositionXAxis, initialCameraPositionYAxis, initialCameraPositionZAxis);
        transform.rotation = Quaternion.Euler(initialCameraRotationXAxis, initialCameraRotationYAxis, initialCameraRotationZAxis);
    }

    
    public void Move() {
        
        // Here we want to move the position of the camera
        
        float x_value = initialCameraPositionXAxis + changePositionCamera + 2 * changePositionCamera * Random.value;
        float y_value = initialCameraPositionYAxis + changePositionCamera + 2 * changePositionCamera * Random.value;
        float z_value = initialCameraPositionZAxis + changePositionCamera + 2 * changePositionCamera * Random.value;
        transform.position = new Vector3(x_value, y_value, z_value);
        
        
        /* Here we want to rotate the camera 
        for the x directionn we impose: current_position +- 1˚ 
        for the y directionn we impose: current_position +- 1˚ 
        for the z directionn we impose: current_position +- 1˚
        */
        
        transform.rotation = Quaternion.Euler(initialCameraRotationXAxis - rotationCamera + 2 * rotationCamera * Random.value, 
                                      initialCameraRotationXAxis - rotationCamera + 2 * rotationCamera * Random.value,
                                      initialCameraRotationXAxis - rotationCamera + 2 * rotationCamera * Random.value
        );
        
    }
}

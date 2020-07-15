using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotVisionCapture : MonoBehaviour
{
    public GameObject cube;
    public GameObject robot;
    public GameObject table;
    public GameObject DomainRandomizationObject;
    public int index = 0;

    

    void Start()
    {
        /*
        if (DomainRandomizationObject.GetComponent<DomainRandomization>() != null){
            DomainRandomization domainRandomization = DomainRandomizationObject.GetComponent<DomainRandomization>();
            domainRandomization.InitializationObjects();
        }
        */
    }

    private void Update()
    {
        VisionDataCollector visionDataCollector = GetComponent<VisionDataCollector>();
        string imageName = visionDataCollector.NextImageName();
        Vector3 relativeCubePosition = cube.transform.position - robot.transform.position;

        CameraPixel cameraPixel = GetComponent<CameraPixel>();
        Vector2 screenPos = cameraPixel.GetPixelPosition();


        RobotVisionDataPoint dataPoint = new RobotVisionDataPoint(screenPos, relativeCubePosition, imageName);

        Debug.Log("the data is" + JsonUtility.ToJson(dataPoint));
        bool didCapture = visionDataCollector.CaptureIfNecessary(imageName, dataPoint);
        if (didCapture)
        {
            index = 0;
            Reset(index);
        }

    }

    // HELPERS

    private void Reset(int index)
    {   
        /* Here we will do the domain randomization. Thus we will randomize:
        - number and shape of distractor objects on the table (cylinders and spheres)
        - position and texture of all objects on the table 
        - position, orientation and filed of view of camera 
        - position, orientation and specular characteristics of the lights
        */
        if (DomainRandomizationObject.GetComponent<DomainRandomization>() != null){
            DomainRandomization domainRandomization = DomainRandomizationObject.GetComponent<DomainRandomization>();
            domainRandomization.DomainRandomizationScene(index);
        }
        
    }


}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotVisionCapture : MonoBehaviour
{
    public GameObject cube;
    public GameObject robot;
    public GameObject table;
    public GameObject DomainRandomizationObject;


    private void Update()
    {
        VisionDataCollector visionDataCollector = GetComponent<VisionDataCollector>();
        string imageName = visionDataCollector.NextImageName();
        Vector3 relativeCubePosition = cube.transform.position - robot.transform.position;

        RobotVisionDataPoint dataPoint = new RobotVisionDataPoint(relativeCubePosition, imageName);

        
        bool didCapture = visionDataCollector.CaptureIfNecessary(imageName, dataPoint);
        if (didCapture)
        {
            Reset();
        }

    }
    private void Reset()
    {   
        /* Here we will do the domain randomization. Thus we will randomize:
        - The number of source lights  
        - The direction, the position and the intensity of the light for the source lights
        - The position and the texture of the cube 
        - The position of the robot  
        - The position, orientation and field of view of the camera 
        - The texture of the table and the robot 
        */
        if (DomainRandomizationObject.GetComponent<DomainRandomization>() != null){
            DomainRandomization domainRandomization = DomainRandomizationObject.GetComponent<DomainRandomization>();
            domainRandomization.DomainRandomizationScene();
        }
        
    }


}

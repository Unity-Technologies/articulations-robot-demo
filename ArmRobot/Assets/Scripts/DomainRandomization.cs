using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainRandomization : MonoBehaviour
{
    public GameObject table;
    public GameObject cube;
    public GameObject robot;
    float robotMinReach;
    float robotMaxReach;
    


    public Vector3 scaleObject = new Vector3 (7f, 7f, 7f);

    public int nbMaxDistractorObjects = 8;
    public float rotationAngle = 10.0f;

    public float yAltitudeTable = 57f;

    public int nbMaxLights = 3;

    
    void Start()
    {
        cube.tag = "Cube";

        RandomizerPositionObject tablePositionRandomizerCube = cube.GetComponent<RandomizerPositionObject>();
        robotMinReach = tablePositionRandomizerCube.robotMinReach;
        robotMaxReach = tablePositionRandomizerCube.robotMaxReach;

    }
    
    
    public void DomainRandomizationScene()
    {
        // move cube
        RandomizerPositionObject tablePositionRandomizerCube = cube.GetComponent<RandomizerPositionObject>();
        tablePositionRandomizerCube.Move();
        
        
        // then we change its pattern 
        CheckerBoard checkerBoardCube = cube.GetComponent<CheckerBoard>();
        //checkerBoardCube.CheckerBoardChange();
          

        // then we change its color 
        ColorRandomizer colorRandomizerCube = cube.GetComponent<ColorRandomizer>();
        //colorRandomizerCube.ChangeColor();

        
        // move robot 
        MoveRobot(rotationAngle);
        
        // then we change its color and the color of all its children
        
        foreach (Transform child in robot.transform){
            ColorRandomizer colorRandomizerChildRobot = child.gameObject.GetComponent<ColorRandomizer>();
            colorRandomizerChildRobot.ChangeColor();
        } 
 
        // move the camera
        GameObject camera = GameObject.Find("VisionCamera");
        RandomizerPositionCamera randomizedPositionCamera = camera.GetComponent<RandomizerPositionCamera>();
        randomizedPositionCamera.Move();
        
        
        // change the color of the table 
        ColorRandomizer colorRandomizerTable = table.GetComponent<ColorRandomizer>();
        colorRandomizerTable.ChangeColor();

        // change the Directionalight
        GameObject directionLight = GameObject.Find("DirectionalLight");
        DirectionalLightRandomization directionalLightRandomizer = directionLight.GetComponent<DirectionalLightRandomization>();
        directionalLightRandomizer.UpdateLight();
           
    }
    public void MoveRobot(float rotationAngle)
    {
        // this function is designed to do the move operation for the robot
        RobotController robotController = robot.GetComponent<RobotController>();
        float rangeRotation = -rotationAngle + 2 * rotationAngle * Random.value;
        float[] rotation = {rangeRotation, rangeRotation, rangeRotation, rangeRotation, rangeRotation, rangeRotation};
        robotController.ForceJointsToRotations(rotation);
    }

    void CreateRandomListLights()
    {
        // this function is designed to create a list of a random number of lights and instantiate them 
        int randomNumberLights = 1 + Random.Range(1, nbMaxLights); 
        for (var i = 0; i < randomNumberLights; i++) {
            GameObject light = CreateLight();
        }
    }

    GameObject CreateLight(){
        GameObject light = new GameObject();
        light.tag = "Light";

        Light lightComp = light.AddComponent<Light>();
        light.AddComponent<PointLightRandomization>();
        PointLightRandomization lightRandomization = light.GetComponent<PointLightRandomization>();
            
        light.transform.position = lightRandomization.PositionUpdate();
        lightComp.intensity = lightRandomization.IntensityUpdate();
        Vector4 color = lightRandomization.ColorUpdate();
        lightComp.color = new Color(color[0], color[1], color[2], color[3]);
        lightComp.range = 120f;
        return light;
    }
    
}

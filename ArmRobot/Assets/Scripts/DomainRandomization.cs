using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainRandomization : MonoBehaviour
{
    public GameObject table;
    public GameObject cube;
    //public GameObject robot;
    float robotMinReach;
    float robotMaxReach;
    


    public Vector3 scaleObject = new Vector3 (0.1f, 0.1f, 0.1f);

    public int nbMaxDistractorObjects = 8;
    public float rotationAngle = 10.0f;

    public float yAltitudeTable = 8.13f;
    public float minimumDistanceWithoutObjects = 0.5f;

    public int nbMaxLights = 3;

    
    void Start()
    {
        cube.tag = "Cube";

        RandomizerPositionObject tablePositionRandomizerCube = cube.GetComponent<RandomizerPositionObject>();
        robotMinReach = tablePositionRandomizerCube.robotMinReach;
        robotMaxReach = tablePositionRandomizerCube.robotMaxReach;

    }
    
    
    /*
    public List<GameObject> InitializationObjects(){ 

        List<GameObject> listOfObjectsTable = CreateRandomListDistractors();
        // we also generate a list of lights objects but these objects will not be part of the list of Objects 
        CreateRandomListLights();

        return listOfObjectsTable;
    }
    */
    
    public void DomainRandomizationScene()
    {
        // we create a list that will contains all the objects we have already moved 
        List<GameObject> listOfAlreadyMovedObjects = new List<GameObject>();

        // Then we start moving the objects and changing the color 
        // move cube
        RandomizerPositionObject tablePositionRandomizerCube = cube.GetComponent<RandomizerPositionObject>();
        tablePositionRandomizerCube.Move(listOfAlreadyMovedObjects);
        
        /*
        // then we change its pattern 
        CheckerBoard checkerBoardCube = cube.GetComponent<CheckerBoard>();
        checkerBoardCube.CheckerBoardChange();
        

        // then we change its color 
        ColorRandomizer colorRandomizerCube = cube.GetComponent<ColorRandomizer>();
        colorRandomizerCube.ChangeColor();

        listOfAlreadyMovedObjects.Add(cube);

        // We desactive the older objects 
        DesactiveObjects();

        // Create and move the new objects 
        // We create the new ojects 
        List<GameObject> listOfObjectsTable = InitializationObjects();

        // then we move them
        // we iterate through the listOfObjects and move them one by one 
        
        foreach (GameObject gameobject in listOfObjectsTable) {
            RandomizerPositionObject tablePositionRandomizerObject = gameobject.GetComponent<RandomizerPositionObject>();
            tablePositionRandomizerObject.Move(listOfAlreadyMovedObjects);
            listOfAlreadyMovedObjects.Add(gameobject); // we add the gameobject to the already moved objects 
        }
        
        
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

        */        
    }
    
    /*
    public void MoveRobot(float rotationAngle)
    {
        // this function is designed to do the move operation for the robot
        RobotController robotController = robot.GetComponent<RobotController>();
        float rangeRotation = -rotationAngle + 2 * rotationAngle * Random.value;
        // float[] rotation = {rangeRotation, rangeRotation, rangeRotation, rangeRotation, rangeRotation, rangeRotation};
        float[] rotation = {0,0,0,0,0,0};
        robotController.ForceJointsToRotations(rotation);
    }

    List<GameObject> CreateRandomListDistractors()
    {
        // this function is designed to create a list of a random number of cylinders and instantiate them 
        float minimumDistanceBetweenObjects = scaleObject[0] + minimumDistanceWithoutObjects;
        int randomNumberDistractors = 1 + Random.Range(1, nbMaxDistractorObjects); 
        List<GameObject> listOfDistractors = new List<GameObject>();
        for (var i = 0; i < randomNumberDistractors; i++) {
            int typeObject = Random.Range(0,2);
            GameObject distractor;
            if (typeObject == 0){

                Vector3 position = new Vector3(robotMaxReach - 2*minimumDistanceWithoutObjects , 
                                    yAltitudeTable, robotMaxReach - 2*minimumDistanceWithoutObjects - minimumDistanceBetweenObjects*i);
                distractor = CreateCylinder(position, scaleObject);
            }
            else {
                Vector3 position = new Vector3(robotMaxReach - 2*minimumDistanceWithoutObjects - minimumDistanceBetweenObjects, 
                                    yAltitudeTable, robotMaxReach - 2*minimumDistanceWithoutObjects - minimumDistanceBetweenObjects*i);
                distractor = CreateShere(position, scaleObject);
            }
            listOfDistractors.Add(distractor);
        }
        return listOfDistractors;
    }
    void CreateRandomListLights()
    {
        // this function is designed to create a list of a random number of lights and instantiate them 
        int randomNumberLights = 1 + Random.Range(1, nbMaxLights); 
        for (var i = 0; i < randomNumberLights; i++) {
            GameObject light = CreateLight();
        }
    }

    void DesactiveObjects(){
        GameObject[] arrayOfSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        GameObject[] arrayOfCylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        GameObject[] arrayOfLights = GameObject.FindGameObjectsWithTag("Light");
        List<GameObject> listOfObjects = new List<GameObject>();
        listOfObjects.AddRange(arrayOfSpheres);
        listOfObjects.AddRange(arrayOfCylinders);
        listOfObjects.AddRange(arrayOfLights);
        
        foreach (GameObject gameobject in listOfObjects){
            gameobject.SetActive(false);
        }
    }

    GameObject CreateCylinder(Vector3 position, Vector3 scale) {
        GameObject cylinder  = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = position;
        cylinder.tag = "Cylinder";
        
        cylinder.transform.localScale = scale;
        cylinder.AddComponent<CheckerBoard>();
        cylinder.AddComponent<ColorRandomizer>();
        cylinder.AddComponent<RandomizerPositionObject>();

        // we change the pattern of the mesh renderer: introduce a check pattern or not 
        CheckerBoard checkerBoard = cylinder.GetComponent<CheckerBoard>();
        checkerBoard.CheckerBoardChange();

        ColorRandomizer colorRandomizer = cylinder.GetComponent<ColorRandomizer>();
        colorRandomizer.ChangeColor();
        return cylinder;
    }

    GameObject CreateShere(Vector3 position, Vector3 scale) {
        GameObject sphere  = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position;
        sphere.tag = "Sphere";
        
        sphere.transform.localScale = scale;
        sphere.AddComponent<CheckerBoard>();
        sphere.AddComponent<ColorRandomizer>();
        sphere.AddComponent<RandomizerPositionObject>();

        // we change the pattern of the mesh renderer: introduce a check pattern or not 
        CheckerBoard checkerBoard = sphere.GetComponent<CheckerBoard>();
        checkerBoard.CheckerBoardChange();

        ColorRandomizer colorRandomizer = sphere.GetComponent<ColorRandomizer>();
        colorRandomizer.ChangeColor();
        return sphere;
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
        lightComp.range = 20f;
        return light;
    }
    */
}

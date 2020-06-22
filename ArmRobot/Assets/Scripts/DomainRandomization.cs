using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainRandomization : MonoBehaviour
{
    public GameObject cube;
    public GameObject robot;
    public GameObject table;


    public float scaleObject = 0.1f;

    public float robotMinReach = 0.2f;
    public float robotMaxReach = 0.5f;

    public int nbMaxCylinders = 5;
    public int nbMaxSpheres = 5;
    public float rotationAngle = 10.0f;

    public float yAltitudeTable = 0.813f;
    public float minimumDistanceWithoutObjects = 0.05f;

    public int nbMaxLights = 3;

    
    void start()
    {
        // first we set the position of the cube 
        float minimumDistanceBetweenObjects = scaleObject + minimumDistanceWithoutObjects;

        cube.transform.position = new Vector3(-robotMaxReach + 2*minimumDistanceWithoutObjects, 
                                        yAltitudeTable, -robotMaxReach + 2*minimumDistanceWithoutObjects);
        
        cube.tag = "Cube";
    }
    
    public List<GameObject> InitializationObjects(){ 

        // First we create the Cylinders
        List<GameObject> listOfCylinders = CreateRandomListCylinders();
        
        // Then we create the spheres 
        List<GameObject> listOfSpheres = CreateRandomListSpheres();
        
        // Then we create the list gathering all the objects which are on the table that we have created 
        List<GameObject> listOfObjectsTable = ConcatenateListOfGameObjects(listOfCylinders, listOfSpheres);

        // we also create a list of lights objects but these objects will not be part of the list of Objects 
        List<GameObject> listOfLights = CreateRandomListLights();

        return listOfObjectsTable;
    }
    
    
    public void DomainRandomizationScene()
    {
        // we create a list that will contains all the objects we have already moved 
        List<GameObject> listOfAlreadyMovedObjects = new List<GameObject>();

        // Then we start moving the objects and changing the color 
        // move cube
        TablePositionRandomizer tablePositionRandomizer_cube = cube.GetComponent<TablePositionRandomizer>();
        tablePositionRandomizer_cube.Move();
        
        
        // then we change its color 
        ColorRandomizer colorRandomizercube = cube.GetComponent<ColorRandomizer>();
        colorRandomizercube.ChangeColor();

        listOfAlreadyMovedObjects.Add(cube);

        // We destroy the older objects 
        DestroyObjects();

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
    
    }

    public void MoveRobot(float rotationAngle)
    {
        // this function is designed to do the move operation for the robot
        RobotController robotController = robot.GetComponent<RobotController>();
        float rangeRotation = -rotationAngle + 2 * rotationAngle * Random.value;
        float[] rotation = {rangeRotation, rangeRotation, rangeRotation, rangeRotation, rangeRotation, rangeRotation};
        robotController.ForceJointsToRotations(rotation);
    }

    List<GameObject> CreateRandomListCylinders()
    {
        // this function is designed to create a list of a random number of cylinders and instantiate them 
        float minimumDistanceBetweenObjects = scaleObject + minimumDistanceWithoutObjects;
        int randomNumberCylinders = 1 + Random.Range(1, nbMaxCylinders); 
        List<GameObject> listOfCylinders = new List<GameObject>();
        for (var i = 0; i < randomNumberCylinders; i++) {
            GameObject cylinder  = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            
            cylinder.transform.position = new Vector3(robotMaxReach - 2*minimumDistanceWithoutObjects , 
                                                yAltitudeTable, robotMaxReach - 2*minimumDistanceWithoutObjects - minimumDistanceBetweenObjects*i);
            
            
            cylinder.transform.localScale = new Vector3(scaleObject, scaleObject, scaleObject);
            cylinder.AddComponent<ColorRandomizer>();
            cylinder.AddComponent<RandomizerPositionObject>();
            cylinder.tag = "Cylinder";
            ColorRandomizer colorRandomizer = cylinder.GetComponent<ColorRandomizer>();
            colorRandomizer.ChangeColor();
            listOfCylinders.Add(cylinder);
        }
        return listOfCylinders;
    }

    List<GameObject> CreateRandomListSpheres()
    {
        // this function is designed to create a list of a random number of spheres and instantiate them 
        float minimumDistanceBetweenObjects = scaleObject + minimumDistanceWithoutObjects;
        int randomNumberSpheres = 1 + Random.Range(1, nbMaxSpheres); 
        List<GameObject> listOfSpheres = new List<GameObject>();
        for (var i = 0; i < randomNumberSpheres; i++) {
            GameObject sphere  = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.position = new Vector3(robotMaxReach - 2*minimumDistanceWithoutObjects - minimumDistanceBetweenObjects, 
                                                yAltitudeTable, robotMaxReach - 2*minimumDistanceWithoutObjects - minimumDistanceBetweenObjects*i);
            
            sphere.transform.localScale = new Vector3(scaleObject, scaleObject, scaleObject);
            sphere.AddComponent<ColorRandomizer>();
            sphere.AddComponent<RandomizerPositionObject>();
            sphere.tag = "Sphere";
            ColorRandomizer colorRandomizer = sphere.GetComponent<ColorRandomizer>();
            colorRandomizer.ChangeColor();
            listOfSpheres.Add(sphere);
        }
        return listOfSpheres;
    }
    List<GameObject> CreateRandomListLights()
    {
        // this function is designed to create a list of a random number of lights and instantiate them 
        int randomNumberLights = 1 + Random.Range(1, nbMaxLights); 
        List<GameObject> listOfLights = new List<GameObject>();
        for (var i = 0; i < randomNumberLights; i++) {
            GameObject light = new GameObject();
            light.tag = "Light";

            Light lightComp = light.AddComponent<Light>();

            PointLightRandomization lightRandomization = GetComponent<PointLightRandomization>();
            
            light.transform.position = lightRandomization.PositionUpdate();
            lightComp.intensity = lightRandomization.IntensityUpdate();
            Vector4 color = lightRandomization.ColorUpdate();
            lightComp.color = new Color(color[0], color[1], color[2], color[3]);
            lightComp.range = 20f;

            listOfLights.Add(light);
        }
        return listOfLights;
    }

    List<GameObject> ConcatenateListOfGameObjects(List<GameObject> list1, List<GameObject> list2){
        // This function is designed to concatenate two lists of Gameobjects 
        List<GameObject> listFinal  = new List<GameObject>();
        foreach (GameObject gameobject in list1){
            listFinal.Add(gameobject);
        }
        foreach (GameObject gameobject in list2){
            listFinal.Add(gameobject);
        }
        return listFinal;
    }
    
    void DestroyObjects(){
        // this function is designed to destroy all the cylinders and spheres which are on the scene 
        GameObject[] arrayOfSpheres = GameObject.FindGameObjectsWithTag("Sphere");
        GameObject[] arrayOfCylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        GameObject[] arrayOfLights = GameObject.FindGameObjectsWithTag("Light");
        List<GameObject> listOfObjects = new List<GameObject>();
        listOfObjects.AddRange(arrayOfSpheres);
        listOfObjects.AddRange(arrayOfCylinders);
        listOfObjects.AddRange(arrayOfLights);
        
        foreach (GameObject gameobject in listOfObjects){
            Destroy(gameobject);
        }
    }
}

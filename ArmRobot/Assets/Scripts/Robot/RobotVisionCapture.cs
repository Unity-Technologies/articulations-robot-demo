using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotVisionCapture : MonoBehaviour
{
    public GameObject cube;
    public GameObject cylinder;
    public GameObject robot;
   
    public int nbMaxGameobjectScene = 10;
    public float yAltitudeTable = 0.778f;
    public float minimumDistanceWithoutObjects = 0.05f;
    public float scaleObject = 0.1f;

    
    public float robotMinReach = 0.2f;
    public float robotMaxReach = 0.5f;


    void Start()
    {
        // first we set the position of the cube 
        float minimumDistanceBetweenObjects = scaleObject + minimumDistanceWithoutObjects;

        cube.transform.position = new Vector3(-robotMaxReach + minimumDistanceWithoutObjects, 
                                        yAltitudeTable, -robotMaxReach + minimumDistanceWithoutObjects);
        // then we set the position of the cylinder 
        cylinder.transform.position = new Vector3(-robotMaxReach + minimumDistanceWithoutObjects + minimumDistanceBetweenObjects, 
                                        yAltitudeTable, -robotMaxReach + minimumDistanceWithoutObjects + minimumDistanceBetweenObjects);
        
        // generate a random integer betwwen 1 and 10 but at least 1
        int randomNumberGameObjects = 1 + Random.Range(1, nbMaxGameobjectScene); 

        // we create game objects 
        int maxNumberObjectsPerRow = (int)(1/(minimumDistanceWithoutObjects + scaleObject)); 
        for (var i = 0; i < randomNumberGameObjects; i++) {
            GameObject sphere  = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // we separate the case where i < maxNumberObjectsPerRow to the case i > maxNumberObjectsPerRow 
            // because no more than maxNumberObjectsPerRow objects can have the same z coordinate 
            if (i < maxNumberObjectsPerRow){
                sphere.transform.position = new Vector3(robotMaxReach - minimumDistanceWithoutObjects - minimumDistanceBetweenObjects*i, 
                                                    yAltitudeTable, robotMaxReach - minimumDistanceWithoutObjects);
            }
            else{
                sphere.transform.position = new Vector3(robotMaxReach - minimumDistanceWithoutObjects - minimumDistanceBetweenObjects*(i-maxNumberObjectsPerRow+1), 
                                                    yAltitudeTable, robotMaxReach - minimumDistanceWithoutObjects - minimumDistanceBetweenObjects);
            }
            
            sphere.transform.localScale = new Vector3(scaleObject, scaleObject, scaleObject);
            sphere.AddComponent<ColorRandomizer>();
            sphere.AddComponent<RandomizerPositionObject>();
            sphere.tag = "Sphere";
            sphere.name = "Sphere_" + i;
 }
    }


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

    // HELPERS

    private void Reset()
    {
        // we create a list that will contains all the objects we have already moved 
        List<GameObject> listOfAlreadyMovedObjects = new List<GameObject>();

        // move cube
        TablePositionRandomizer tablePositionRandomizer_cube = cube.GetComponent<TablePositionRandomizer>();
        tablePositionRandomizer_cube.Move();

        listOfAlreadyMovedObjects.Add(cube);

        // move other objects 
        // first we create an array of all the objects in the scene except the cube 

        GameObject[] arrayOfSpheres= GameObject.FindGameObjectsWithTag("Sphere");
        List<GameObject> listOfObjects = new List<GameObject>();
        listOfObjects.AddRange(arrayOfSpheres); // we need to do that because we want to work with list and not array and arrayOfSpheres is an array 
        listOfObjects.Add(cylinder);

        // then we will iterate through the arrayOfObjects and move them one by one 
        foreach (GameObject gameobject in listOfObjects) {
            RandomizerPositionObject tablePositionRandomizerObject = gameobject.GetComponent<RandomizerPositionObject>();
            tablePositionRandomizerObject.Move(listOfAlreadyMovedObjects);
            listOfAlreadyMovedObjects.Add(gameobject); // we add the gameobject to the already moved objects 

        }

        // move robot 
        RobotController robotController = robot.GetComponent<RobotController>();
        float[] rotation = {Random.value * 10, Random.value * 10, Random.value * 10, Random.value * 10, Random.value * 10, Random.value * 10};
        robotController.ForceJointsToRotations(rotation);
    }


}

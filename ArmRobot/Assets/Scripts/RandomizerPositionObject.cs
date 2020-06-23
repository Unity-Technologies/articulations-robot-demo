using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerPositionObject : MonoBehaviour
{


    private GameObject gameObjectSeen;
    public float robotMinReach = 0.2f;
    public float robotMaxReach = 0.45f;

    Bounds tableBounds;
    float yAltitudeTable = 0.778f;


    // CONTROL

    public void Move(List<GameObject> listOfAlreadyMovedObjects)
    {
        // random position (on table, within reach)     
        GameObject table = GameObject.Find("Table");
        tableBounds = table.GetComponent<Collider>().bounds;

        Vector2 tableTopPoint = RandomReachablePointOnTable(listOfAlreadyMovedObjects);
        Vector3 tableCenter = tableBounds.center;
        float x = tableCenter.x + tableTopPoint.x;
        float z = tableCenter.z + tableTopPoint.y;
        transform.position = new Vector3(x, yAltitudeTable, z);
        

        // random rotation
        Vector3 randomRotation = new Vector3(
            transform.rotation.eulerAngles.x,
            Random.value * 360.0f,
            transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(randomRotation);
    }


    // HELPERS

    Vector2 RandomReachablePointOnTable(List<GameObject> listOfAlreadyMovedObjects)
    {   
        
        while (true)
        {   
            Vector2 randomOffset = RandomPoint(robotMinReach, robotMaxReach, listOfAlreadyMovedObjects);
            
            
            bool onTable = PointOnTable(randomOffset);
            if (onTable)
            {
                return randomOffset;
            }
        }
    }

    bool PointOnTable(Vector2 point)
    {
        /*  point: The 2D point on table top, relative to center of table top.
         *  Determines if this point would be on the table or not.      
         */
        Vector3 tableExtents = tableBounds.extents;
        float targetRadius = GameObjectRadius(gameObject);
        
        float safeXDistance = tableExtents.x - targetRadius;
        float safeZDistance = tableExtents.z - targetRadius;
        float xDistance = Mathf.Abs(point.x);
        float yDistance = Mathf.Abs(point.y);

        
        bool onTable = (xDistance < safeXDistance) && (yDistance < safeZDistance);
        return onTable;
    }

    public Vector2 RandomPoint(float minRadius, float maxRadius, List<GameObject> listOfAlreadyMovedObjects)
    {
        /*  Picks a 2D point randomly at uniform. Must be between minRadius and
         *  maxRadius from center and not at the same place than the cube. 
         *  Point given relative to center.      
         */

        while (true)
        {
            // pick a random point between the square of edge (2 * minRadius + radiusOfTheGameObject) and the one with 
            // edge (2 * maxRadius - radiusOfTheGameObject) to be sure that the point is reachable by the robot 
            
            float gameObjectRadius = GameObjectRadius(gameObject);

            float randomX;
            int randomSignX = 2 * Random.Range(0,2) - 1;
            
            if (randomSignX == 1){
                randomX = minRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
            }
            else {
                randomX = -maxRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
            }

            float randomZ;
            int randomSignZ = 2 * Random.Range(0,2) - 1;
            if (randomSignZ == 1){
                randomZ = minRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
            }
            else {
                randomZ = -maxRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
            }
            
            Vector2 randomPoint = new Vector2(randomX, randomZ); // now we are sure the point is in the area reachable by the robot 

            // now we need to check if their is no conflict with the position of objects already moved 
            // we call the PositionIsValid ;ethod and check whether or not the random point generated is a good fit
            bool goodPoint = PositionIsValid(randomPoint, listOfAlreadyMovedObjects);

            if (goodPoint == true){
                return randomPoint;
            }
            
        }
    }

    bool PositionIsValid(Vector2 randomPoint, List<GameObject> listOfAlreadyMovedObjects){
        /* method to check if their is no conflict with the position of objects already moved 
        and the point we have generated. 
        */

        bool goodPoint = true; 
            
        // we iterate through all the list of already moved objects 
        for (var i = 0; i < listOfAlreadyMovedObjects.Count; i++){
            gameObjectSeen = listOfAlreadyMovedObjects[i];
            // first we will compute the distance between the datapoint generated and one of the object of the list of already moved objects 
            Vector2 interObject = new Vector2(gameObjectSeen.transform.position.x - randomPoint[0], gameObjectSeen.transform.position.z - randomPoint[1]);
            float distanceBetweenObjects = interObject.magnitude;
            
            // now we need to access to the radius of the gameobjectSeen
            float gameObjectSeenRadius = GameObjectRadius(gameObjectSeen);
            
            // now we need to access to the radius of the current object
            float gameObjectRadius = GameObjectRadius(gameObject);

            /* keep only if the center of the object from the list of already moved objects is superior 
            * to the sum of their respective radius  
            */
            if (distanceBetweenObjects < gameObjectRadius + gameObjectSeenRadius){
                goodPoint = false;
                break;
            }
        }
        return goodPoint;
    }

    public static float GameObjectRadius(GameObject gameObject){
        /* method to take out the radius of a gameObject
        */
        float gameObjectRadius = 0f;
        if (gameObject.tag == "Cube"){
                    gameObjectRadius = Mathf.Sqrt(2) * gameObject.GetComponent<Collider>().bounds.extents.x; 
                }
        else if (gameObject.tag == "Cylinder"){
                    gameObjectRadius = gameObject.GetComponent<CapsuleCollider>().radius * gameObject.GetComponent<Transform>().localScale[0]; // for the sphere 
                }
        else if (gameObject.tag == "Sphere"){
                    gameObjectRadius = gameObject.GetComponent<SphereCollider>().radius * gameObject.GetComponent<Transform>().localScale[0]; // for the sphere 
                }
        return gameObjectRadius;
    }

}

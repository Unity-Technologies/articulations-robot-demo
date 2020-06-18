using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerPositionObject : MonoBehaviour
{
    private GameObject table;

    private GameObject gameobjectSeen;
    float robotMinReach = 0.2f;
    float robotMaxReach = 0.5f;

    Bounds tableBounds;
    float targetY;


    void Start()
    {
        //GameObject[] arrayofspheres = GameObject.FindGameObjectsWithTag("Sphere");
        table = GameObject.Find("Table");
        tableBounds = table.GetComponent<Collider>().bounds;
        targetY = transform.position.y;
    }


    // CONTROL

    public void Move(List<GameObject> listOfAlreadyMovedObjects)
    {
        // random position (on table, within reach)     
        Vector2 tableTopPoint = RandomReachablePointOnTable(listOfAlreadyMovedObjects);
        Vector3 tableCenter = tableBounds.center;
        float x = tableCenter.x + tableTopPoint.x;
        float z = tableCenter.z + tableTopPoint.y;
        transform.position = new Vector3(x, targetY, z);

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
        float targetRadius = 0f;
        if (tag == "Sphere"){
                targetRadius = GetComponent<SphereCollider>().radius * GetComponent<Transform>().localScale[0]; // for the sphere 
            }
            // otherwise it is a cylinder 
            else{
                targetRadius = GetComponent<CapsuleCollider>().radius * GetComponent<Transform>().localScale[0]; 
            }

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
        //for (var j = 0; j < 100; j++)
        while (true)
        {
            // pick a random point between the square of edge 2*minRadius and the one with edge 2*maxRadius to be sure that the point is reachable by the robot 
            float randomX = 0.25f;
            float randomZ = 0.25f;
            float randomSignX = Random.value;
            float criticalValueForSign = 0.5f;
            if (randomSignX > criticalValueForSign){
                randomX = minRadius + (maxRadius - minRadius) * Random.value;
            }
            else {
                randomX = -maxRadius + (-minRadius - (-maxRadius)) * Random.value;
            }

            float randomSignZ = Random.value;
            if (randomSignZ > criticalValueForSign){
                randomZ = minRadius + (maxRadius - minRadius) * Random.value;
            }
            else {
                randomZ = -maxRadius + (-minRadius - (-maxRadius)) * Random.value;
            }
            Vector2 randomPoint = new Vector2(randomX, randomZ); // now we are sure the point is in the area reachable by the robot 

            // now we need to check if their is no conflict with the position of objects already moved 
            // we define a flag that will give information on whether or not the random point fits 
            bool flag = true; 
            
            // we iterate through all the list of already moved objects 
            for (var i = 0; i < listOfAlreadyMovedObjects.Count; i++){
                gameobjectSeen = listOfAlreadyMovedObjects[i];

                // first we will compute the distance between the datapoint generated and one of the object of the list of already moved objects 
                Vector2 inter_object = new Vector2(gameobjectSeen.transform.position.x - randomX, gameobjectSeen.transform.position.z - randomZ);
                float distance_between_objects = inter_object.magnitude;
                
                // now we need to access to the radius of the gameobjectSeen
                float gameobjectSeenRadius = 0f; 
                if (gameobjectSeen.tag == "Sphere"){
                    gameobjectSeenRadius = gameobjectSeen.GetComponent<SphereCollider>().radius * gameobjectSeen.GetComponent<Transform>().localScale[0]; // for the sphere 
                }
                else if (gameobjectSeen.name == "Cube"){
                    gameobjectSeenRadius = Mathf.Sqrt(2) * gameobjectSeen.GetComponent<BoxCollider>().bounds.extents.x;
                }
                else{
                    gameobjectSeenRadius = gameobjectSeen.GetComponent<CapsuleCollider>().radius * gameobjectSeen.GetComponent<Transform>().localScale[0]; // for the cylinder
                }

                // now we need to access to the radius of the current object
                float objectRadius = 0f;
                if (tag == "Sphere"){
                    objectRadius = GetComponent<SphereCollider>().radius * GetComponent<Transform>().localScale[0]; // for the sphere 
                }
                // otherwise it is a cylinder 
                else{
                    objectRadius = GetComponent<CapsuleCollider>().radius * GetComponent<Transform>().localScale[0]; 
                }

                /* keep only if the center of the object from the list of already moved objects is superior 
                * to the sum of their respective radius  
                */
                if (distance_between_objects <= objectRadius + gameobjectSeenRadius){
                    flag = false;
                    break;
                }
            }
        
            if (flag == true){
                return randomPoint;
                
            }
            
        }
    }

}

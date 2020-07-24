using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizerPositionObject : MonoBehaviour
{


    public float robotMinReach = 0.25f;
    public float robotMaxReach = 0.5f;

    Bounds tableBounds;


    // CONTROL

    public void Move(float yAltitudeTable)
    {
        Vector3 initialRotation = new Vector3(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(initialRotation);
        
        // random position (on table, within reach)     
        GameObject table = GameObject.Find("Table");
        tableBounds = table.GetComponent<Collider>().bounds;

        Vector2 tableTopPoint = RandomReachablePointOnTable();
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

    Vector2 RandomReachablePointOnTable()
    {   
        
        while (true)
        {   
            Vector2 randomOffset = RandomPoint(robotMinReach, robotMaxReach);
            
            
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

    public Vector2 RandomPoint(float minRadius, float maxRadius)
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
            float randomZ;
            int randomConstraint = Random.Range(0, 2);
            if (randomConstraint == 1){
                // we put a constraint on X 
                int randomSignX = 2 * Random.Range(0,2) - 1;
            
                if (randomSignX == 1){
                    randomX = minRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
                }
                else {
                    randomX = -maxRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
                }

                randomZ = -maxRadius + gameObjectRadius + (2 * maxRadius - 2 * gameObjectRadius) * Random.value;
            }
            
            else {
                // we put a constraint on Z
                int randomSignZ = 2 * Random.Range(0,2) - 1;
            
                if (randomSignZ == 1){
                    randomZ = minRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
                }
                else {
                    randomZ = -maxRadius + gameObjectRadius + (maxRadius - minRadius - 2 * gameObjectRadius) * Random.value;
                }

                randomX = -maxRadius + gameObjectRadius + (2 * maxRadius - 2 * gameObjectRadius) * Random.value;
            }

            Vector2 randomPoint = new Vector2(randomX, randomZ); // now we are sure the point is in the area reachable by the robot 

            return randomPoint;   
        }
    }

    public static float GameObjectRadius(GameObject gameObject){
        /* method to take out the radius of a gameObject
        */
        float gameObjectRadius = 0f;
        if (gameObject.tag == "Cube"){
                    gameObjectRadius = Mathf.Sqrt(2) * gameObject.GetComponent<Collider>().bounds.extents.x; 
                }
        return gameObjectRadius;
    }

}

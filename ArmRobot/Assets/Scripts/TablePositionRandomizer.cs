using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablePositionRandomizer : MonoBehaviour
{
    public GameObject table;
    public float robotMinReach = 0.2f;
    public float robotMaxReach = 0.5f;

    Bounds tableBounds;
    float targetY;


    void Start()
    {
        tableBounds = table.GetComponent<Collider>().bounds;
        targetY = transform.position.y;
    }


    // CONTROL

    public void Move()
    {
        // random position (on table, within reach)     
        Vector2 tableTopPoint = RandomReachablePointOnTable();
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

        // reset velocity
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
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
        float targetRadius = GetComponent<Collider>().bounds.extents.x;
        float safeXDistance = tableExtents.x - targetRadius;
        float safeZDistance = tableExtents.z - targetRadius;
        float xDistance = Mathf.Abs(point.x);
        float yDistance = Mathf.Abs(point.y);
        bool onTable = (xDistance < safeXDistance) && (yDistance < safeZDistance);
        return onTable;
    }

    static Vector2 RandomPoint(float minRadius, float maxRadius)
    {
        /*  Picks a 2D point randomly at uniform. Must be between minRadius and
         *  maxRadius from center. Point given relative to center.      
         */
        while (true)
        {
            // pick a random point in a square
            float randomX = (Random.value * maxRadius * 2.0f) - maxRadius;
            float randomY = (Random.value * maxRadius * 2.0f) - maxRadius;
            Vector2 randomPoint = new Vector2(randomX, randomY);

            // compute distance to point
            float d = randomPoint.magnitude;

            // keep only if point is between min and max radius
            if (d > minRadius && d < maxRadius)
            {
                return randomPoint;
            }
        }
    }

}

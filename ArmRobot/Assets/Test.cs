using System.Collections;
using System.Collections.Generic;
using RosSharp;
using RosSharp.Urdf;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // get matrix from the Transform
        var matrix = transform.localToWorldMatrix;
        Debug.Log(matrix);
        // get position from the last column
        var position = new Vector3(matrix[0, 3], matrix[1, 3], matrix[2, 3]);
        Debug.Log("Transform position from matrix is: " + position);
        Debug.Log(ExportRpyData(transform));
    }

    // Update is called once per frame
    void Update()
    {

    }
    Vector3 Ros2Unity(Vector3 vector3)
    {
        return new Vector3(-vector3.y, vector3.z, vector3.x);
    }

private static double[] ExportRpyData(Transform transform)
        {
            Vector3 rpyVector = new Vector3(
                -transform.localEulerAngles.z ,
                transform.localEulerAngles.x ,
                -transform.localEulerAngles.y);
            Debug.Log("Export rounding check: "+transform.localEulerAngles.x + " " + transform.localEulerAngles.y + " " + transform.localEulerAngles.z + " " + rpyVector[0] + " " + rpyVector[1] + " "+ rpyVector[2] + " ");
            return rpyVector == Vector3.zero ? null : rpyVector.ToRoundedDoubleArray();
        }


}

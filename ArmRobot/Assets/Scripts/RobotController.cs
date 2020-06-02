using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotController : MonoBehaviour
{
    [System.Serializable]
    public struct Joint
    {
        public string inputAxis;
        public GameObject robotPart;
    }
    public Joint[] joints;


    // CONTROL

    public void StopAllJointRotations()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            GameObject robotPart = joints[i].robotPart;
            UpdateRotationState(RotationDirection.None, robotPart);
        }
    }

    public void RotateJoint(int jointIndex, RotationDirection direction)
    {
        StopAllJointRotations();
        Joint joint = joints[jointIndex];
        UpdateRotationState(direction, joint.robotPart);
    }

    // HELPERS

    static void UpdateRotationState(RotationDirection direction, GameObject robotPart)
    {
        var childrenList = robotPart.transform.GetComponentsInChildren<ArticulationJointController>();
        foreach (var a in childrenList)
        {
            Debug.Log(a.gameObject.name + " " + robotPart.name);
            if (a.gameObject.name == robotPart.name)
            {
                a.gameObject.GetComponent<ArticulationJointController>().rotationState = direction;
            }

        }
    }



}

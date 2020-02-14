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

    public void ForceJointsToRotations(float[] rotations)
    {
        /*
        for (int i = 0; i < rotations.Length; i++)
        {
            float rotation = rotations[i];
            Joint joint = joints[i];
            ArticulationJointController jointController = joint.robotPart.GetComponent<ArticulationJointController>();
            jointController.ForceToRotation(rotation);
        }
        */

        // just do first joint, for testing:
        Joint joint = joints[0];
        ArticulationJointController jointController = joint.robotPart.GetComponent<ArticulationJointController>();
        jointController.ForceToRotation(0.0f);
    }

    // HELPERS

    static void UpdateRotationState(RotationDirection direction, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.rotationState = direction;
    }



}

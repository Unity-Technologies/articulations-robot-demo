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


    // READ

    public float[] GetCurrentJointRotations()
    {
        float[] list = new float[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            Joint joint = joints[i];
            ArticulationJointController jointController = joint.robotPart.GetComponent<ArticulationJointController>();
            float currentRotation = jointController.CurrentPrimaryAxisRotation();
            list[i] = currentRotation;
        }
        return list;
    }


    // CONTROL

    public void StopAllJointRotations()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            GameObject robotPart = joints[i].robotPart;
            UpdateRotationState(RotationDirection.None, robotPart);
        }
    }

    public void RotateJoint(int jointIndex, RotationDirection direction, bool stopPrevious = true)
    {
        if (stopPrevious)
        {
            StopAllJointRotations();
        }
        Joint joint = joints[jointIndex];
        UpdateRotationState(direction, joint.robotPart);
    }

    public void ForceJointsToRotations(float[] rotations)
    {
        for (int i = 0; i < rotations.Length; i++)
        {
            Joint joint = joints[i];
            ArticulationJointController jointController = joint.robotPart.GetComponent<ArticulationJointController>();
            jointController.ForceToRotation(0.0f);
        }
    }

    // HELPERS

    static void UpdateRotationState(RotationDirection direction, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.rotationState = direction;
    }


}

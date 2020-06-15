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
        for (int i=0; i < rotations.Length; i++)
        {
            float rotation = rotations[i];
            Joint joint = joints[i];
            ForceJointToRotation(rotation, joint.robotPart);
        }
    }

    public void RandomJointRotation()
    {
        int randomJointIndex = Random.Range(0, joints.Length);
        RotationDirection randomRotationDirection = (RotationDirection)Random.Range(-1, 2);
        RotateJoint(randomJointIndex, randomRotationDirection);
    }


    // READ

    public float[] GetCurrentJointRotations()
    {
        float[] list = new float[joints.Length];
        for (int i = 0; i < joints.Length; i++)
        {
            Joint joint = joints[i];
            float currentRotation = GetJointRotation(joint.robotPart);
            list[i] = currentRotation;
        }
        return list;
    }


    // HELPERS

    static void UpdateRotationState(RotationDirection direction, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.rotationState = direction;
    }

    static void ForceJointToRotation(float rotation, GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.ForceToRotation(rotation);
    }

    float GetJointRotation(GameObject robotPart)
    {
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        return jointController.GetPrimaryAxisRotation();
    }
}

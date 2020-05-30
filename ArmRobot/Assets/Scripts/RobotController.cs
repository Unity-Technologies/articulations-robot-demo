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
        public ArticulationJointController bpJointController;
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
            UpdateRotationState(RotationDirection.None, joints[i].bpJointController);
        }
    }
    public void RotateJoints()
    {
        for (int i = 0; i < joints.Length; i++)
        {
            UpdateRotationState(RotationDirection.None, joints[i].bpJointController);
        }
    }

    public void RotateJoint(int jointIndex, RotationDirection direction, bool stopPrevious = true)
    {
        if (stopPrevious)
        {
            StopAllJointRotations();
        }
        var joint = joints[jointIndex].bpJointController;
        UpdateRotationState(direction, joint);
    }

    public void ForceJointsToRotations(float[] rotations)
    {
        for (int i = 0; i < rotations.Length; i++)
        {
            joints[i].bpJointController.ForceToRotation(0.0f);
        }
    }

    // HELPERS

    static void UpdateRotationState(RotationDirection direction, ArticulationJointController robotPartJoint)
    {
//        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        robotPartJoint.rotationState = direction;
    }


}

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
        
        for (int i = 0; i < rotations.Length; i++)
        {
            Debug.Log("forcing joint to zero: joint " + i.ToString("F1"));
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

    void DisableArticulations()
    {
        // NOTE: not being used right now
        for (int i = joints.Length - 1; i >= 0; i -= 1)
        {
            Debug.Log("Disabling articulation " + i.ToString("F1"));
            Joint joint = joints[i];
            ArticulationJointController jointController = joint.robotPart.GetComponent<ArticulationJointController>();
            jointController.SetArticulationToEnabled(false);
        }
    }

    void EnableArticulations()
    {
        // NOTE: not being used right now
        for (int i = 0; i < joints.Length; i++)
        {
            Debug.Log("Enabling articulation " + i.ToString("F1"));
            Joint joint = joints[i];
            ArticulationJointController jointController = joint.robotPart.GetComponent<ArticulationJointController>();
            jointController.SetArticulationToEnabled(true);
        }
    }

}

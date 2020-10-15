using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPhysicsSpectrometer;

[System.Serializable]
public class RobotControllerData : BaseData
{
    public string[] inputAxis;
    public int[] robotPartId;

    public RobotControllerData(RobotController controller)
    {
        instanceID = controller.GetInstanceID();
        inputAxis = new string[controller.joints.Length];
        robotPartId = new int[controller.joints.Length];

        for (int i = 0; i < controller.joints.Length; i ++)
        {
            inputAxis[i] = controller.joints[i].inputAxis;
            robotPartId[i] = controller.joints[i].robotPart.GetInstanceID();
        }
    }

    public override void UpdateComponent(Component component)
    {
        ((RobotController)component).joints = new RobotController.Joint[inputAxis.Length];
        for (int i = 0; i < inputAxis.Length; i ++)
        {
            ((RobotController)component).joints[i] = new RobotController.Joint()
            {
                inputAxis = inputAxis[i],
                robotPart = GameObjectInstanceMap.Instance.GetDeserializedObject(
                    GameObjectInstanceMap.Instance.GetDeserializedId(robotPartId[i])),
            };
        }
    }
}



public class RobotController : ScriptComponent
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
        ArticulationJointController jointController = robotPart.GetComponent<ArticulationJointController>();
        jointController.rotationState = direction;
    }


    public override BaseData ToBaseData()
    {
        return new RobotControllerData(this);
    }
}

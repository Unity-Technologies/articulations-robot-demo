using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPhysicsSpectrometer;

[System.Serializable]
public class RobotManualInputData : BaseData
{
    public int robotId;

    public RobotManualInputData(RobotManualInput input)
    {
        instanceID = input.GetInstanceID();
        robotId = input.robot.GetInstanceID();
    }

    public override void UpdateComponent(Component component)
    {
        ((RobotManualInput)component).robot =
            GameObjectInstanceMap.Instance.GetDeserializedObject(
                GameObjectInstanceMap.Instance.GetDeserializedId(robotId));
    }
}

public class RobotManualInput : ScriptComponent
{
    public GameObject robot;


    void Update()
    {
        RobotController robotController = robot.GetComponent<RobotController>();
        for (int i = 0; i < robotController.joints.Length; i++)
        {
            float inputVal = Input.GetAxis(robotController.joints[i].inputAxis);
            if (Mathf.Abs(inputVal) > 0)
            {
                RotationDirection direction = GetRotationDirection(inputVal);
                robotController.RotateJoint(i, direction);
                return;
            }
        }
        robotController.StopAllJointRotations();

    }


    // HELPERS

    static RotationDirection GetRotationDirection(float inputVal)
    {
        if (inputVal > 0)
        {
            return RotationDirection.Positive;
        }
        else if (inputVal < 0)
        {
            return RotationDirection.Negative;
        }
        else
        {
            return RotationDirection.None;
        }
    }

    public override BaseData ToBaseData()
    {
        return new RobotManualInputData(this);
    }
}

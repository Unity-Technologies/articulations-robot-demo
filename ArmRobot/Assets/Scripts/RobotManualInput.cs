using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManualInput : MonoBehaviour
{
    public GameObject robot;
    RobotController robotController;

    private void OnEnable()
    {
        robotController = robot.GetComponent<RobotController>();
    }

    void Update()
    {
        // check for robot movement
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

        //check for robot reset
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed reset!");
            float[] defaultRotations = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
            robotController.ForceJointsToRotations(defaultRotations);
        }
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
}

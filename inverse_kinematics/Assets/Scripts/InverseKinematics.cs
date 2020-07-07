using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseKinematics : MonoBehaviour
{
    public GameObject robot;


    public void MoveRobotInverseKinematics(float rotationAngle, int childPositionOfEndeffector)
    {
        // this function is designed to do the move operation for the robot
        RobotController robotController = robot.GetComponent<RobotController>();

        float[] rotation = new float[childPositionOfEndeffector];
        for (var i=0; i < childPositionOfEndeffector; i++){
            rotation[i] = -rotationAngle + 2 * rotationAngle * Random.value;
        }
        robotController.ForceJointsToRotations(rotation);
    }

    
}

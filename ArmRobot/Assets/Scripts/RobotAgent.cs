using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class RobotAgent : Agent
{
    public GameObject endEffector;
    public GameObject cube;
    public GameObject robot;

    RobotController robotController;
    TouchDetector touchDetector;
    TablePositionRandomizer tablePositionRandomizer;
    

    void Start()
    {
        robotController = robot.GetComponent<RobotController>();
        touchDetector = endEffector.GetComponent<TouchDetector>();
        tablePositionRandomizer = cube.GetComponent<TablePositionRandomizer>();
    }


    // AGENT
    
    public override void AgentReset()
    {
        
        float[] defaultRotations = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        robotController.ForceJointsToRotations(defaultRotations);
        touchDetector.hasTouchedTarget = false;
        tablePositionRandomizer.Move();
       
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // current rotations
        float[] rotations = robotController.GetCurrentJointRotations();
        foreach (float rotation in rotations)
        {
            float rotationRadians = rotation * (Mathf.PI / 180.0f);
            sensor.AddObservation(Mathf.Sin(rotationRadians));
            sensor.AddObservation(Mathf.Cos(rotationRadians));
        }

        //cube position
        Vector3 cubePosition = cube.transform.position - robot.transform.position;
        sensor.AddObservation(cubePosition);

        //end position
        Vector3 endPosition = endEffector.transform.position - robot.transform.position;
        sensor.AddObservation(endPosition);        
    }

    public override void AgentAction(float[] vectorAction)
    {
        /*
        // move
        int jointIndex = (int)vectorAction[0];
        int actionIndex = (int)vectorAction[1];
        RotationDirection rotationDirection = AgentHelpers.ActionIndexToRotationDirection(actionIndex);
        robotController.RotateJoint(jointIndex, rotationDirection);

        // end episode if we touched the cube
        if (touchDetector.hasTouchedTarget)
        {
            Done();
        }

        //reward
        float distanceToCube = Vector3.Distance(endEffector.transform.position, cube.transform.position);
        SetReward(-distanceToCube * 0.01f);
        */
    }



    


}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MLAgents.Sensors;
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
        touchDetector = cube.GetComponent<TouchDetector>();
        tablePositionRandomizer = cube.GetComponent<TablePositionRandomizer>();
    }


    // AGENT

    public override void OnEpisodeBegin()
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
            // normalize rotation to [0, 1] range
            float normalizedRotation = rotation / 360.0f;
            sensor.AddObservation(normalizedRotation);
        }

        // relative cube position
        Vector3 cubePosition = cube.transform.position - robot.transform.position;
        sensor.AddObservation(cubePosition);

        // relative end position
        Vector3 endPosition = endEffector.transform.position - robot.transform.position;
        sensor.AddObservation(endPosition);        
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // move
        robotController.RotateAllJoints(vectorAction);

        // end episode if we touched the cube
        if (touchDetector.hasTouchedTarget)
        {
            EndEpisode();
        }

        //reward
        float distanceToCube = Vector3.Distance(endEffector.transform.position, cube.transform.position);
        SetReward(-distanceToCube * 0.01f);
        
    }


    // HELPERS

    static public RotationDirection ActionIndexToRotationDirection(int actionIndex)
    {
        return (RotationDirection)(actionIndex + 1);
    }




}



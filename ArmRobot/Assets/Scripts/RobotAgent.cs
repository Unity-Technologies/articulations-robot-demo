using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
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
        if (robotController.joints[0].robotPart == null)
        {
            // No robot is present, no observation should be added
            return;
        }
        // current rotations
//        float[] rotations = robotController.GetCurrentJointRotations();
//        foreach (float rotation in rotations)
//        {
//            // normalize rotation to [-1, 1] range
//            float normalizedRotation = (rotation / 360.0f) %  1f;
//            sensor.AddObservation(normalizedRotation);
//        }

//        foreach (var joint in robotController.joints)
//        {
//            sensor.AddObservation(joint.robotPart.transform.position - robot.transform.position);
//            sensor.AddObservation(joint.robotPart.transform.forward);
//            sensor.AddObservation(joint.robotPart.transform.right);
//        }

        // relative cube position
        Vector3 cubePosition = cube.transform.position - robot.transform.position;
        sensor.AddObservation(cubePosition);

        // relative end position
        Vector3 endPosition = endEffector.transform.position - robot.transform.position;
        sensor.AddObservation(endPosition);
        sensor.AddObservation(cubePosition - endPosition);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // move
        for (int jointIndex = 0; jointIndex < vectorAction.Length; jointIndex ++)
        {
            RotationDirection rotationDirection = ActionIndexToRotationDirection((int) vectorAction[jointIndex]);
            robotController.RotateJoint(jointIndex, rotationDirection, false);
        }

        // end episode if we touched the cube
        if (touchDetector.hasTouchedTarget)
        {
            SetReward(1f);
            EndEpisode();
        }


        //reward
        float distanceToCube = Vector3.Distance(endEffector.transform.position, cube.transform.position); // roughly 0.7f


        var jointHeight = 0f; // This is to reward the agent for keeping high up // max is roughly 3.0f
        for (int jointIndex = 0; jointIndex < robotController.joints.Length; jointIndex ++)
        {
            jointHeight += robotController.joints[jointIndex].robotPart.transform.position.y - cube.transform.position.y;
        }
        var reward = - distanceToCube + jointHeight / 100f;

        SetReward(reward * 0.1f);

    }


    // HELPERS

    static public RotationDirection ActionIndexToRotationDirection(int actionIndex)
    {
        return (RotationDirection)(actionIndex - 1);
    }




}



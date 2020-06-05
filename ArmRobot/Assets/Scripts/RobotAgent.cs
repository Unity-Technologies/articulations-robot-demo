using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using Random = UnityEngine.Random;

public class RobotAgent : Agent
{
    public GameObject endEffector;
    public GameObject robot;
    
    RobotController robotController;
    TouchDetector touchDetector;
    TablePositionRandomizer tablePositionRandomizer;
    
    [Header("SCENARIO")] 
    
    [Header("TARGET")] 
    public GameObject target;

    public int updateTargetPosEveryXSeconds = 10;
    public Transform spawnOrigin;
    public float spawnRadiusFromBase = .4f;

    public float currentDistToTarget;
//    public Collider s ;

    [Header("REWARDS")] 
    public bool rewardPos;
    public bool rewardLookDirection;
    public bool rewardLookingDown;
    public Vector3 startingLookDir;
    private Vector3 dirToLook;

    public override void Initialize()
    {
        dirToLook = startingLookDir;
//        UpdateTargetPosAndRot();
        
        robotController = robot.GetComponent<RobotController>();
        touchDetector = target.GetComponent<TouchDetector>();
        tablePositionRandomizer = target.GetComponent<TablePositionRandomizer>();
//        InvokeRepeating("UpdateTargetPosAndRot", 2,updateTargetPosEveryXSeconds);
    }

    void UpdateTargetPosAndRot()
    {
        target.transform.position = spawnOrigin.position + (Random.onUnitSphere * spawnRadiusFromBase);
//        target.transform.rotation = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);
        var dir = target.transform.position - robot.transform.position;
        target.transform.rotation = Quaternion.LookRotation(dir);
//        target.transform.rotation = Random.rotationUniform;

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
//        // current rotations
//        float[] rotations = robotController.GetCurrentJointRotations();
//        foreach (float rotation in rotations)
//        {
//            // normalize rotation to [-1, 1] range
//            float normalizedRotation = (rotation / 360.0f) %  1f;
//            sensor.AddObservation(normalizedRotation);
//        }

        foreach (var joint in robotController.joints)
        {
            //local rotation of each body part
            sensor.AddObservation(joint.robotPart.transform.localRotation);
            //pos delta relative to the robot's orientation space
            sensor.AddObservation(robot.transform.InverseTransformDirection(joint.robotPart.transform.position - robot.transform.position));
        }

        sensor.AddObservation(robot.transform.InverseTransformDirection(target.transform.position - robot.transform.position));
        sensor.AddObservation(robot.transform.InverseTransformDirection(endEffector.transform.position - robot.transform.position));
        sensor.AddObservation(robot.transform.InverseTransformDirection(target.transform.position - endEffector.transform.position));
//        sensor.AddObservation(endEffector.transform.InverseTransformDirection(target.transform.position - endEffector.transform.position));
//        sensor.AddObservation(endEffector.transform.up);
//        sensor.AddObservation(Vector3.Dot(endEffector.transform.up, Vector3.up));
        sensor.AddObservation(endEffector.transform.forward);
//        sensor.AddObservation(target.transform.forward);
        sensor.AddObservation(Vector3.Dot(endEffector.transform.forward, Vector3.down));
//        sensor.AddObservation(Vector3.Dot(endEffector.transform.forward, target.transform.forward));
//        sensor.AddObservation(touchDetector.hasTouchedTarget);
//        sensor.AddObservation(robot.transform.InverseTransformPoint(endEffector.transform.position));


//        // relative cube position
//        Vector3 cubePosition = cube.transform.position - robot.transform.position;
//        sensor.AddObservation(cubePosition);
//
//        // relative end position
//        Vector3 endPosition = endEffector.transform.position - robot.transform.position;
//        sensor.AddObservation(endPosition);
//        sensor.AddObservation(cubePosition - endPosition);
    }

    void Update()
    {
        if (target.transform.localPosition.y < -.2f)
        {
            print("fell off table");
            touchDetector.hasTouchedTarget = false;
            tablePositionRandomizer.Move();
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        for (int i = 0; i < vectorAction.Length; i++)
        {
            //the magnitude of the action float determines the speed
            //the direction (positive or negative) determines the direction
            robotController.joints[i].bpJointController.rotationDirection = vectorAction[i];
        }
        
        
//        for (int jointIndex = 0; jointIndex < vectorAction.Length; jointIndex ++)
//        {
//        }
//        int i = -1;
//        robotController.joints[0].bpJointController.SetJointDriveTargetValue(vectorAction[++i]);
//        robotController.joints[0].bpJointController.SetJointDriveTargetValue(vectorAction[++i]);
//        robotController.joints[0].bpJointController.SetJointDriveTargetValue(vectorAction[++i]);
//        robotController.joints[0].bpJointController.SetJointDriveTargetValue(vectorAction[++i]);
//        robotController.joints[0].bpJointController.SetJointDriveTargetValue(vectorAction[++i]);
//        robotController.joints[0].bpJointController.SetJointDriveTargetValue(vectorAction[++i]);
//        robotController.joints[0].bpJointController.SetJointDriveTargetValue(vectorAction[++i]);
//
//        
//        int i = -1;
//        robotController.joints[0].bpJointController.rotationState = ActionIndexToRotationDirection((int) vectorAction[++i]);
//        robotController.joints[0].bpJointController.speed = 50 * (int)vectorAction[++i];
//        robotController.joints[1].bpJointController.rotationState = ActionIndexToRotationDirection((int) vectorAction[++i]);
//        robotController.joints[1].bpJointController.speed = 50 * (int)vectorAction[++i];
//        robotController.joints[2].bpJointController.rotationState = ActionIndexToRotationDirection((int) vectorAction[++i]);
//        robotController.joints[2].bpJointController.speed = 50 * (int)vectorAction[++i];
//        robotController.joints[3].bpJointController.rotationState = ActionIndexToRotationDirection((int) vectorAction[++i]);
//        robotController.joints[3].bpJointController.speed = 50 * (int)vectorAction[++i];
//        robotController.joints[4].bpJointController.rotationState = ActionIndexToRotationDirection((int) vectorAction[++i]);
//        robotController.joints[4].bpJointController.speed = 50 * (int)vectorAction[++i];
//        robotController.joints[5].bpJointController.rotationState = ActionIndexToRotationDirection((int) vectorAction[++i]);
//        robotController.joints[5].bpJointController.speed = 50 * (int)vectorAction[++i];
//        robotController.joints[6].bpJointController.rotationState = ActionIndexToRotationDirection((int) vectorAction[++i]);
//        robotController.joints[6].bpJointController.speed = 50 * (int)vectorAction[++i];
//
//        for (int x = 0; i < robotController.joints.Length; x++)
//        {
//            UpdateRotationState(RotationDirection.None, joints[i].bpJointController);
//        }


        //REWARDS
        currentDistToTarget = Vector3.Distance(endEffector.transform.position, target.transform.position);
        if (rewardPos)
        {
            //max 1
            float distRew = 3 - Mathf.Clamp(currentDistToTarget,0, 3);
            AddReward(0.01f * distRew);
        }
        
        //head should look at target
        if (rewardLookDirection)
        {
            AddReward(0.005f * Vector3.Dot(endEffector.transform.forward, target.transform.forward));
        }
        
        //head should look down
        if (rewardLookingDown)
        {
            AddReward(0.005f * Vector3.Dot(endEffector.transform.forward, Vector3.down));
        }
        
        
        
        
        ///////////////////
        // end episode if we touched the cube
        if (touchDetector.hasTouchedTarget)
        {
//            AddReward(1f);
            AddReward(1f * Vector3.Dot(endEffector.transform.forward, Vector3.down));
            touchDetector.hasTouchedTarget = false;
            tablePositionRandomizer.Move();
//            SetReward(1f);
//            EndEpisode();
        }
        
//        //encourage head alignment with up axis
//        AddReward(0.001f * Vector3.Dot(endEffector.transform.up, Vector3.up));

        
        
        
        

    }


    // HELPERS

    static public RotationDirection ActionIndexToRotationDirection(int actionIndex)
    {
        return (RotationDirection)(actionIndex - 1);
    }

}

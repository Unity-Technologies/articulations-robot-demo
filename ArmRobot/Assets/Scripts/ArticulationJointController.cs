using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };
//    public enum JointRotationSpeed {0, 50, 100, 150, 200, 250, 300};

public class ArticulationJointController : MonoBehaviour
{
    public RotationDirection rotationState = RotationDirection.None;
    public float speed = 300.0f;
    public float maxSpeed = 300.0f;
    public float driveTargetValue;
    public float rotationDirection;
    private ArticulationBody articulation;


    // LIFE CYCLE

    void OnEnable()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    void FixedUpdate() 
    {
        
            float rotationChange = rotationDirection * maxSpeed * Time.fixedDeltaTime;
            driveTargetValue = CurrentPrimaryAxisRotation() + rotationChange;
            RotateTo(driveTargetValue);
//        if (rotationState != RotationDirection.None && speed > 0) {
//            float rotationChange = (float)rotationState * speed * Time.fixedDeltaTime;
//            float rotationGoal = CurrentPrimaryAxisRotation() + rotationChange;
//            RotateTo(rotationGoal);
//        }


    }

//    public void SetJointDriveTargetValue(float dir = 0)
//    {
//        float rotationChange = dir * maxSpeed * Time.fixedDeltaTime;
//        driveTargetValue = CurrentPrimaryAxisRotation() + rotationChange;
//    }
//    

    // READ

    public float CurrentPrimaryAxisRotation()
    {
        float currentRotationRads = articulation.jointPosition[0];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }


    // CONTROL

    public void ForceToRotation(float rotation)
    {
        // set target
        RotateTo(rotation);
        
        // force position
        float rotationRads = Mathf.Deg2Rad * rotation;
        ArticulationReducedSpace newPosition = new ArticulationReducedSpace(rotationRads);
        articulation.jointPosition = newPosition;

        // force velocity to zero
        ArticulationReducedSpace newVelocity = new ArticulationReducedSpace(0.0f);
        articulation.jointVelocity = newVelocity;
        
    }


    // MOVEMENT HELPERS

    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }




}

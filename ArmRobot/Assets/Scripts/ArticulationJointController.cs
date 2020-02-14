using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };

public class ArticulationJointController : MonoBehaviour
{
    public RotationDirection rotationState = RotationDirection.None;
    public float speed = 500.0f;

    private ArticulationBody articulation;


    // LIFE CYCLE

    void Start()
    {
        articulation = GetComponent<ArticulationBody>();
    }

    void FixedUpdate() 
    {
        if (rotationState != RotationDirection.None) {
            float rotationChange = (float)rotationState * speed * Time.fixedDeltaTime;
            float rotationGoal = CurrentPrimaryAxisRotation() + rotationChange;
            RotateTo(rotationGoal);
        }


    }

    // CONTROL

    public void ForceToRotation(float primaryAxisRotation)
    {
        Debug.Log("Forcing rotation back to: " + primaryAxisRotation.ToString("F1"));
        //articulation.enabled = false;

        // set target
        RotateTo(primaryAxisRotation);

        // force position
        /*
        float rotationRads = Mathf.Deg2Rad * primaryAxisRotation;
        ArticulationReducedSpace newReducedSpace = new ArticulationReducedSpace();
        newReducedSpace[0] = rotationRads;
        newReducedSpace.dofCount = 1;
        articulation.jointPosition = newReducedSpace;
        */
        //articulation.enabled = true;
    }


    // MOVEMENT HELPERS

    float CurrentPrimaryAxisRotation()
    { 
        float currentRotationRads = articulation.jointPosition[0];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        
        //Debug.Log("target: " + articulation.xDrive.target.ToString("F1") + ", current: " + currentRotation.ToString("F1"));

        return currentRotation;
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }




}

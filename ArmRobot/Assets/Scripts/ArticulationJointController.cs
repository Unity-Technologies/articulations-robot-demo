using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };

public class ArticulationJointController : MonoBehaviour
{
    public RotationDirection rotationState = RotationDirection.None;
    public float speed = 300.0f;

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


    // MOVEMENT HELPERS

    float CurrentPrimaryAxisRotation()
    {
        float currentRotationRads = articulation.jointPosition[0];
        float currentRotation = Mathf.Rad2Deg * currentRotationRads;
        return currentRotation;
    }

    void RotateTo(float primaryAxisRotation)
    {
        var drive = articulation.xDrive;
        drive.target = primaryAxisRotation;
        articulation.xDrive = drive;
    }

    public void ForceToRotation(float primaryAxisRotation)
    {
        // set joint to given rotation
        RotateTo(primaryAxisRotation);

        // set transform to given rotation
        float x = (articulation.jointPosition[0] * primaryAxisRotation) + (1.0f - articulation.jointPosition[0]) * transform.localRotation.eulerAngles.x;
        float y = (articulation.jointPosition[1] * primaryAxisRotation) + (1.0f - articulation.jointPosition[1]) * transform.localRotation.eulerAngles.y;
        float z = (articulation.jointPosition[2] * primaryAxisRotation) + (1.0f - articulation.jointPosition[2]) * transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(x, y, z);
    }

    public float GetPrimaryAxisRotation()
    {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        float primaryAxisRotation = currentRotation.x * articulation.jointPosition[0] + currentRotation.y * articulation.jointPosition[1] + currentRotation.z * articulation.jointPosition[2];
        return primaryAxisRotation;
    }

}
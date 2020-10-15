using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPhysicsSpectrometer;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };

[Serializable]
public class ArticulationJointControllerData : BaseData
{
    public RotationDirection rotationState;
    public float speed;

    public ArticulationJointControllerData(ArticulationJointController controller)
    {
        instanceID = controller.GetInstanceID();
        rotationState = controller.rotationState;
        speed = controller.speed;
    }

    public override void UpdateComponent(Component component)
    {
        ((ArticulationJointController)component).rotationState = rotationState;
        ((ArticulationJointController)component).speed = speed;
    }
}

public class ArticulationJointController : ScriptComponent
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


    public override BaseData ToBaseData()
    {
        return new ArticulationJointControllerData(this);
    }
}

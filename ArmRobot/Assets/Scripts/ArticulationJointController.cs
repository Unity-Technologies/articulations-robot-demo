using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationDirection { None = 0, Positive = 1, Negative = -1 };

public class ArticulationJointController : MonoBehaviour
{
    public RotationDirection rotationState = RotationDirection.None;
    public float speed = 300.0f;
    public float r;

    private ConfigurableJoint joint;

    Vector3 lastLocalPosition;
    Quaternion lastLocalRotation;



    // LIFE CYCLE

    private void Awake()
    {
        joint = GetComponent<ConfigurableJoint>();
    }

    void Start()
    {
        lastLocalPosition = transform.localPosition;
        lastLocalRotation = transform.localRotation;
    }

    void FixedUpdate()
    {
        if (rotationState != RotationDirection.None) {
            float rotationChange = (float)rotationState * speed * Time.fixedDeltaTime;
            float rotationGoal = CurrentPrimaryAxisRotation() + rotationChange;
            RotateTo(rotationGoal);
        }

        UpdateLockState();
        r = GetPrimaryAxisRotation();
    }


    // CONTROL

    public void ForceToRotation(float primaryAxisRotation)
    {
        // set joint to given rotation
        RotateTo(primaryAxisRotation);

        // set transform to given rotation
        float x = (joint.axis.x * primaryAxisRotation) + (1.0f - joint.axis.x) * transform.localRotation.eulerAngles.x;
        float y = (joint.axis.y * primaryAxisRotation) + (1.0f - joint.axis.y) * transform.localRotation.eulerAngles.y;
        float z = (joint.axis.z * primaryAxisRotation) + (1.0f - joint.axis.z) * transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(x, y, z);
    }


    // READ

    public float GetPrimaryAxisRotation()
    {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        float primaryAxisRotation = currentRotation.x * joint.axis.x + currentRotation.y * joint.axis.y + currentRotation.z * joint.axis.z;
        return primaryAxisRotation;
    }


    // RIGIDITY HELPERS

    void UpdateLockState()
    {
        transform.localPosition = lastLocalPosition;

        // lock all but the axes of rotation
        float x = (joint.axis.x * transform.localRotation.eulerAngles.x) + ((1.0f - joint.axis.x) * lastLocalRotation.eulerAngles.x);
        float y = (joint.axis.y * transform.localRotation.eulerAngles.y) + ((1.0f - joint.axis.y) * lastLocalRotation.eulerAngles.y);
        float z = (joint.axis.z * transform.localRotation.eulerAngles.z) + ((1.0f - joint.axis.z) * lastLocalRotation.eulerAngles.z);
        transform.localRotation = Quaternion.Euler(x, y, z);

    }


    // MOVEMENT HELPERS

    float CurrentPrimaryAxisRotation()
    {
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        float rotation = joint.axis.x * currentRotation.x + joint.axis.y * currentRotation.y + joint.axis.z * currentRotation.z;
        return rotation;
    }

    void RotateTo(float primaryAxisRotation)
    {
        Vector3 goalRotation = new Vector3(primaryAxisRotation, 0.0f, 0.0f);
        joint.targetRotation = Quaternion.Inverse(Quaternion.Euler(goalRotation));
    }



}

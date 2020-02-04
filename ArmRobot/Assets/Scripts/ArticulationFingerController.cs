using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GripState { Fixed = 0, Opening = -1, Closing = 1 };

public class ArticulationFingerController : MonoBehaviour
{
    public GameObject fingerA;
    public GameObject fingerB;

    // Grip - the extent to which the pincher is closed. 0: fully open, 1: fully closed.
    public float grip;
    public float gripSpeed = 3.0f;
    public GripState gripState = GripState.Fixed;

    Vector3 fingerAOpenPosition;
    Vector3 fingerBOpenPosition;

    public float fingerAClosedZ;
    public float fingerBClosedZ;


    void Start()
    {
        fingerAOpenPosition = fingerA.transform.localPosition;
        fingerBOpenPosition = fingerB.transform.localPosition;
    }

    void FixedUpdate()
    {
        UpdateGrip();
        UpdateFingersForGrip();
    }


    // READ

    public float CurrentGrip()
    {
        // TODO - we can't really assume the fingers agree, need to think about that
        float fingerAGrip = Mathf.InverseLerp(fingerAOpenPosition.z, fingerAClosedZ, fingerA.transform.localPosition.z);
        float fingerBGrip = Mathf.InverseLerp(fingerBOpenPosition.z, fingerBClosedZ, fingerB.transform.localPosition.z);
        float meanGrip = (fingerAGrip + fingerBGrip) / 2.0f;
        return meanGrip;
    }

    public Vector3 CurrentGraspCenter()
    {
        /* Gets the point directly between the middle of the pincher fingers,
         * in the global coordinate system.      
         */
        Vector3 localCenterPoint = (fingerAOpenPosition + fingerBOpenPosition) / 2.0f;
        Vector3 globalCenterPoint = transform.TransformPoint(localCenterPoint);
        return globalCenterPoint;
    }


    // CONTROL

    public void ResetGripToOpen()
    {
        grip = 0.0f;
        UpdateFingersForGrip();
        fingerA.transform.localPosition = fingerAOpenPosition;
        fingerB.transform.localPosition = fingerBOpenPosition;

        gripState = GripState.Fixed;
    }

    // GRIP HELPERS

    void UpdateGrip()
    {
        if (gripState != GripState.Fixed)
        {
            float gripChange = (float)gripState * gripSpeed * Time.fixedDeltaTime;
            float gripGoal = CurrentGrip() + gripChange;
            grip = Mathf.Clamp01(gripGoal);

            //Debug.Log(CurrentGrip());
        }
    }

    void UpdateFingersForGrip()
    {
        // fingerA
        float zPositionA = Mathf.Lerp(fingerAOpenPosition.z, fingerAClosedZ, grip);
        MoveFinger(fingerA, fingerAOpenPosition.z, zPositionA);

        //fingerB
        float zPositionB = Mathf.Lerp(fingerBOpenPosition.z, fingerBClosedZ, grip);
        MoveFinger(fingerB, fingerBOpenPosition.z, zPositionB);
    }

    void MoveFinger(GameObject finger, float fingerLocalOpenZ, float toLocalZ)
    {
        ArticulationBody articulation = finger.GetComponent<ArticulationBody>();
        float targetZ = (toLocalZ - fingerLocalOpenZ) * transform.localScale.z;

        var drive = articulation.zDrive;
        drive.target = targetZ;
        articulation.zDrive = drive;

    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GripState { Fixed = 0, Opening = -1, Closing = 1 };

public class ArticulationFingerController : MonoBehaviour
{
    public GameObject fingerAGameObject;
    public GameObject fingerBGameObject;

    // Grip - the extent to which the pincher is closed. 0: fully open, 1: fully closed.
    public float grip;
    public float gripSpeed = 3.0f;
    public GripState gripState = GripState.Fixed;

    //Vector3 fingerAOpenPosition;
    //Vector3 fingerBOpenPosition;

    //public float fingerAClosedZ;
    //public float fingerBClosedZ;

    Finger fingerA;
    Finger fingerB;

    struct Finger
    {
        public float closedZ;
        public Vector3 openPosition;
        public GameObject gameObject;
        public ArticulationBody articulation;

        public void init(GameObject finger, float fingerClosedZ)
        {
            articulation = finger.GetComponent<ArticulationBody>();
            gameObject = finger;
            openPosition = finger.transform.localPosition;
            closedZ = fingerClosedZ;
        }

        public float CurrentGrip()
        {
            float grip = Mathf.InverseLerp(openPosition.z, closedZ, gameObject.transform.localPosition.z);
            return grip;
        }

        public float ZDriveTarget(float grip, Transform transform)
        {
            float zPosition = Mathf.Lerp(openPosition.z, closedZ, grip);
            float targetZ = (zPosition - openPosition.z) * transform.localScale.z;
            return targetZ;
        }

        public void UpdateGrip(float grip, Transform transform)
        {
            float targetZ = ZDriveTarget(grip, transform);
            var drive = articulation.zDrive;
            drive.target = targetZ;
            articulation.zDrive = drive;
        }

        public void ForceOpen(Transform transform)
        {
            gameObject.transform.localPosition = openPosition;
            UpdateGrip(0.0f, transform);
        }
    }


    void Start()
    {
        /*
        fingerAOpenPosition = fingerA.transform.localPosition;
        fingerBOpenPosition = fingerB.transform.localPosition;
        */

        //new
        fingerA = new Finger();
        fingerA.init(fingerAGameObject, -1.1f);

        fingerB = new Finger();
        fingerB.init(fingerBGameObject, 1.1f);
        

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
        //float fingerAGrip = Mathf.InverseLerp(fingerAOpenPosition.z, fingerAClosedZ, fingerA.transform.localPosition.z);
        //float fingerBGrip = Mathf.InverseLerp(fingerBOpenPosition.z, fingerBClosedZ, fingerB.transform.localPosition.z);
        float meanGrip = (fingerA.CurrentGrip() + fingerB.CurrentGrip()) / 2.0f;
        return meanGrip;
    }


    public Vector3 CurrentGraspCenter()
    {
        /* Gets the point directly between the middle of the pincher fingers,
         * in the global coordinate system.      
         */
        Vector3 localCenterPoint = (fingerA.openPosition + fingerB.openPosition) / 2.0f;
        Vector3 globalCenterPoint = transform.TransformPoint(localCenterPoint);
        return globalCenterPoint;
    }


    // CONTROL

    public void ResetGripToOpen()
    {
        grip = 0.0f;
        //UpdateFingersForGrip();
        //fingerA.transform.localPosition = fingerAOpenPosition;
        //fingerB.transform.localPosition = fingerBOpenPosition;
        fingerA.ForceOpen(transform);
        fingerB.ForceOpen(transform);
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
        /*
        // fingerA
        float zPositionA = Mathf.Lerp(fingerAOpenPosition.z, fingerAClosedZ, grip);
        MoveFinger(fingerA, fingerAOpenPosition.z, zPositionA);

        //fingerB
        float zPositionB = Mathf.Lerp(fingerBOpenPosition.z, fingerBClosedZ, grip);
        MoveFinger(fingerB, fingerBOpenPosition.z, zPositionB);
        */

        fingerA.UpdateGrip(grip, transform);
        fingerB.UpdateGrip(grip, transform);
    }

    /*
    void MoveFinger(GameObject finger, float fingerLocalOpenZ, float toLocalZ)
    {
        ArticulationBody articulation = finger.GetComponent<ArticulationBody>();
        float targetZ = (toLocalZ - fingerLocalOpenZ) * transform.localScale.z;

        var drive = articulation.zDrive;
        drive.target = targetZ;
        articulation.zDrive = drive;

    }
    */
    /*
    float FingerAZDriveTarget(float grip)
    {
        
        float zPositionA = Mathf.Lerp(fingerAOpenPosition.z, fingerAClosedZ, grip);
        float targetZ = (zPositionA - fingerAOpenPosition.z) * transform.localScale.z;
        return targetZ;
    }
    */



}

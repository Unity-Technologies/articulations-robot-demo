using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlState { Fixed = 0, PositiveMotion = 1, NegativeMotion = -1 };

public class VacuumGripperController : MonoBehaviour
{

    public GameObject horizontalJoint;
    public ControlState horizontalMotion = ControlState.Fixed;
    public ControlState verticalMotion = ControlState.Fixed;
    public float speed = 1.0f;


    private void FixedUpdate()
    {
        // move horizontally
        if (horizontalMotion != ControlState.Fixed)
        {
            ArticulationBody articulation = horizontalJoint.GetComponent<ArticulationBody>();

            //get jointPosition along axis
            float drivePosition = articulation.jointPosition[0];

            //increment this position
            float targetPosition = drivePosition + (float)horizontalMotion * Time.fixedDeltaTime * speed;

            //set joint Drive to new position
            var drive = articulation.yDrive;
            drive.target = targetPosition;
            articulation.yDrive = drive;
        }

    }

}

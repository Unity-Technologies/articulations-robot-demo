using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlState { Fixed = 0, PositiveMotion = 1, NegativeMotion = -1 };

public class VacuumGripperController : MonoBehaviour
{

    public GameObject horizontalJoint;
    public ControlState horizontalMotion = ControlState.Fixed;
    public ControlState verticalMotion = ControlState.Fixed;



    private void FixedUpdate()
    {
        
    }
}

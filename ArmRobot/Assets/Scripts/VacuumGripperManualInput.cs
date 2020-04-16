using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumGripperManualInput : MonoBehaviour
{

    public GameObject vacuumGripper;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Base");
        float verticalInput = Input.GetAxis("Shoulder");

        ControlState horizontalControlState = InputToControlState(horizontalInput);
        ControlState verticalControlState = InputToControlState(verticalInput);

        VacuumGripperController controller = vacuumGripper.GetComponent<VacuumGripperController>();
        controller.horizontalMotion = horizontalControlState;
        controller.verticalMotion = verticalControlState;
    }


    // HELPERS

    ControlState InputToControlState(float input)
    {
        if (input > 0)
        {
            return ControlState.PositiveMotion;
        }
        else if (input < 0)
        {
            return ControlState.NegativeMotion;
        }
        else
        {
            return ControlState.Fixed;
        }
    }
}

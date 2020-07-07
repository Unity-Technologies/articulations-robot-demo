using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticulationHandManualInput : MonoBehaviour
{
    public GameObject hand;

    void Update()
    {
        // manual input
        float input = Input.GetAxis("Fingers");
        PincherController pincherController = hand.GetComponent<PincherController>();
        pincherController.gripState = GripStateForInput(input);
    }

    // INPUT HELPERS

    static GripState GripStateForInput(float input)
    {
        if (input > 0)
        {
            return GripState.Closing;
        }
        else if (input < 0)
        {
            return GripState.Opening;
        }
        else
        {
            return GripState.Fixed;
        }
    }
}

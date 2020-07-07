using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripperDemoManualInput : MonoBehaviour
{
    public GameObject hand;


    void Update()
    {
        float input = Input.GetAxis("BigHandVertical");
        BigHandState moveState = MoveStateForInput(input);
        GripperDemoController controller = hand.GetComponent<GripperDemoController>();
        controller.moveState = moveState;
    }

    BigHandState MoveStateForInput(float input)
    {
        if (input > 0)
        {
            return BigHandState.MovingUp;
        }
        else if (input < 0)
        {
            return BigHandState.MovingDown;
        }
        else
        {
            return BigHandState.Fixed;
        }
    }
}

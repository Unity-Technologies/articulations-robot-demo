using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityPhysicsSpectrometer;

[System.Serializable]
public class ArticulationHandManualInputData : BaseData
{
    public int handId;

    public ArticulationHandManualInputData(ArticulationHandManualInput input)
    {
        instanceID = input.GetInstanceID();
        handId = input.hand.GetInstanceID();
    }

    public override void UpdateComponent(Component component)
    {
        ((ArticulationHandManualInput)component).hand =
            GameObjectInstanceMap.Instance.GetDeserializedObject(
                GameObjectInstanceMap.Instance.GetDeserializedId(handId));
    }
}

public class ArticulationHandManualInput : ScriptComponent
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

    public override BaseData ToBaseData()
    {
        return new ArticulationHandManualInputData(this);
    }
}

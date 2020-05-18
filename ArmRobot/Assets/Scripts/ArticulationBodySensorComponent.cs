using Unity.MLAgents.Sensors;
using UnityEngine;
public class ArticulationBodySensorComponent : SensorComponent
{
    public ArticulationBody RootBody;

    /// <inheritdoc/>
    public override ISensor CreateSensor()
    {
        return new ArticulationBodySensor(RootBody);
    }

    /// <inheritdoc/>
    public override int[] GetObservationShape()
    {
        return new[] { ArticulationBodySensor.GetArticulationSensorSize(RootBody) };
    }
}

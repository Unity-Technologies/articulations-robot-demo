using System;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ArticulationBodySensor : ISensor
{
    string m_SensorName;
    int[] m_Shape;
    ArticulationBody[] m_Bodies;

    public ArticulationBodySensor(ArticulationBody rootBody, string name = null)
    {
        m_SensorName = string.IsNullOrEmpty(name) ? $"ArticulationBodySensor:{rootBody.name}" : name;
        // Note that m_Bodies[0] will always be rootBody
        m_Bodies = rootBody.GetComponentsInChildren<ArticulationBody>();

        var sensorSize = GetArticulationSensorSize(rootBody);
        m_Shape = new[] { sensorSize };
    }

    /// <inheritdoc/>
    public int[] GetObservationShape()
    {
        return m_Shape;
    }

    /// <inheritdoc/>
    public int Write(ObservationWriter writer)
    {
        int obsIndex = 0;
        foreach (var body in m_Bodies)
        {
            obsIndex = WriteBody(writer, body, obsIndex);
        }

        return obsIndex;
    }

    /// <inheritdoc/>
    public byte[] GetCompressedObservation()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void Update()
    {

    }

    /// <inheritdoc/>
    public void Reset() { }

    /// <inheritdoc/>
    public SensorCompressionType GetCompressionType()
    {
        return SensorCompressionType.None;
    }

    /// <inheritdoc/>
    public string GetName()
    {
        return m_SensorName;
    }

    public static int GetArticulationSensorSize(ArticulationBody rootBody)
    {
        if (rootBody == null)
        {
            return 0;
        }

        int numObs = 0;
        foreach (var childBody in rootBody.GetComponentsInChildren<ArticulationBody>())
        {
            numObs += GetArticulationObservationSize(childBody);
        }

        return numObs;
    }

    static int GetArticulationObservationSize(ArticulationBody body)
    {
        var isRoot = body.isRoot;
        var obsSize = 9; // 3 for transform pos, fwd, right

        // TODO more observations for dof depending on type
        var dof = body.dofCount;
        return obsSize + dof;
    }

    int WriteBody(ObservationWriter writer, ArticulationBody body, int observationIndex)
    {
        if (body == null)
        {
            // TODO - getting this error
            //   MissingReferenceException: The object of type 'ArticulationBody' has been destroyed but you are still trying to access it.
            //   Your script should either check if it is null or you should not destroy the object.
            // Handle later.
            return observationIndex;
        }
        var pos = body.transform.position;
        writer[observationIndex++] = pos.x;
        writer[observationIndex++] = pos.y;
        writer[observationIndex++] = pos.z;

        var fwd = body.transform.forward;
        writer[observationIndex++] = fwd.x;
        writer[observationIndex++] = fwd.y;
        writer[observationIndex++] = fwd.z;

        var right = body.transform.right;
        writer[observationIndex++] = right.x;
        writer[observationIndex++] = right.y;
        writer[observationIndex++] = right.z;

        // Write degree-of-freedom info. For now, assume all angular.
        for (var dofIndex = 0; dofIndex < body.dofCount; dofIndex++)
        {
            var jointRotationRads = body.jointPosition[dofIndex];
            var jointRotationDegs = jointRotationRads * Mathf.Rad2Deg;
            var rotationFmod = (jointRotationDegs / 360.0f) % 1f;
            writer[observationIndex++] = rotationFmod;
        }

        return observationIndex;
    }
}


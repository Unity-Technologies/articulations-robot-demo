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
            if (body == null)
            {
                // TODO - getting this error
                //   MissingReferenceException: The object of type 'ArticulationBody' has been destroyed but you are still trying to access it.
                //   Your script should either check if it is null or you should not destroy the object.
                // Handle later.
                continue;
            }
            var pos = body.transform.position;
            writer[obsIndex++] = pos.x;
            writer[obsIndex++] = pos.y;
            writer[obsIndex++] = pos.z;

            var fwd = body.transform.forward;
            writer[obsIndex++] = fwd.x;
            writer[obsIndex++] = fwd.y;
            writer[obsIndex++] = fwd.z;

            var right = body.transform.right;
            writer[obsIndex++] = right.x;
            writer[obsIndex++] = right.y;
            writer[obsIndex++] = right.z;
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
        return 9; // 3 for transform pos, fwd, right
    }
}


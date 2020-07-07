using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotVisionCapture : MonoBehaviour
{
    
    public GameObject EndEffector;
    public GameObject robot;
    public GameObject table;
    public float rotationAngle = 10.0f;

    public int childPositionOfEndeffector = 6;

    public GameObject inverseKinematicsObject;

    

    private void Update()
    {
        VisionDataCollector visionDataCollector = GetComponent<VisionDataCollector>();
        string imageName = visionDataCollector.NextImageName();
        Vector3 relativeEndEffectorPosition = EndEffector.transform.position - robot.transform.position;
        Vector3 relativeEndEffectorOrientation = new Vector3(EndEffector.transform.rotation.x, EndEffector.transform.rotation.y, EndEffector.transform.rotation.z);

        float[] JointAngles = new float[childPositionOfEndeffector];
        List<GameObject> JointList= new List<GameObject>();
        JointList.Add(robot);
        for (var indexJoint = 0; indexJoint < childPositionOfEndeffector; indexJoint++) {
            GameObject child = JointList[indexJoint].transform.GetChild(0).gameObject;
            JointList.Add(child);
            ArticulationBody articulation = child.GetComponent<ArticulationBody>();
            float currentRotationRads = articulation.jointPosition[0];
            JointAngles[indexJoint] = currentRotationRads;
        }
        
        RobotVisionDataPoint dataPoint = new RobotVisionDataPoint(relativeEndEffectorPosition, relativeEndEffectorOrientation, JointAngles, imageName);

        bool didCapture = visionDataCollector.CaptureIfNecessary(imageName, dataPoint);

        if (didCapture)
        {
            Reset();
        }
    }

    private void Reset()
    {    
        
        InverseKinematics inverseKinematics = inverseKinematicsObject.GetComponent<InverseKinematics>();
        inverseKinematics.MoveRobotInverseKinematics(rotationAngle, childPositionOfEndeffector);
    }


}

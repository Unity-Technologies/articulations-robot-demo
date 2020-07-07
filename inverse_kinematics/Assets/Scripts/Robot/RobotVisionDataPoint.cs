using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    
    
    public float x;
    public float y;
    public float z;
    public float alpha;
    public float beta;
    public float gamma;
    
    public float[] angle_joint;

    /*
    public float angle_joint1;
    public float angle_joint2;
    public float angle_joint3;
    public float angle_joint4;
    public float angle_joint5;
    public float angle_joint6;
    */

    public string screenCaptureName;

    public RobotVisionDataPoint(Vector3 relativeEndEffectorPosition, Vector3 relativeEndEffectorOrientation, float[] JointAngles, string screenCaptureName)
    {
        // for UR3 I consider the end effector as the wrist3
        
        this.x = relativeEndEffectorPosition.x;
        this.y = relativeEndEffectorPosition.y;
        this.z = relativeEndEffectorPosition.z;
        
        this.alpha = relativeEndEffectorOrientation[0];
        this.beta = relativeEndEffectorOrientation[1];
        this.gamma = relativeEndEffectorOrientation[2];

        this.angle_joint = JointAngles;

        /*

        this.angle_joint1 = JointAngles[0];
        this.angle_joint2 = JointAngles[1];
        this.angle_joint3 = JointAngles[2];
        this.angle_joint4 = JointAngles[3];
        this.angle_joint5 = JointAngles[4];
        this.angle_joint6 = JointAngles[5];
        */

        this.screenCaptureName = screenCaptureName;
    }
}

using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    public float pixel_x; 
    public float pixel_y;

    public float x;
    public float y;
    public float z; 
    public float q_w;
    public float q_x;
    public float q_y;
    public float q_z;
    public string screenCaptureName;

    public RobotVisionDataPoint(Quaternion cube_orientation, Vector3 relativeCubePosition, string screenCaptureName)
    {

        this.x = relativeCubePosition[0];
        this.y = relativeCubePosition[1];
        this.z = relativeCubePosition[2];

        this.q_w = cube_orientation[3];
        this.q_x = cube_orientation[0];
        this.q_y = cube_orientation[1];
        this.q_z = cube_orientation[2];

        this.screenCaptureName = screenCaptureName;
    }

}

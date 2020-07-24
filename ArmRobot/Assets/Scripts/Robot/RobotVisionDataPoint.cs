using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    public float x;
    public float y;
    public float z; 
    public string screenCaptureName;

    public RobotVisionDataPoint(Vector3 relativeCubePosition, string screenCaptureName)
    {
        this.x = relativeCubePosition[0];
        this.y = relativeCubePosition[1];
        this.z = relativeCubePosition[2];

        this.screenCaptureName = screenCaptureName;
    }

}

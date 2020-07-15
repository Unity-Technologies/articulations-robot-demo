using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    public float pixel_position_x; 
    public float pixel_position_y;

    public float x;
    public float y;
    public float z; 
    public string screenCaptureName;

    public RobotVisionDataPoint(Vector2 screenPos, Vector3 relativeCubePosition, string screenCaptureName)
    {
        
        this.pixel_position_x = screenPos[0];
        this.pixel_position_y = screenPos[1];

        this.x = relativeCubePosition[0];
        this.y = relativeCubePosition[1];
        this.z = relativeCubePosition[2];

        this.screenCaptureName = screenCaptureName;
    }

}

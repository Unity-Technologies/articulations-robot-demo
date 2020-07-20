using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    public float pixel_x; 
    public float pixel_y;

    public float x;
    public float y;
    public float z; 
    public string screenCaptureName;

    public RobotVisionDataPoint(Vector2 screenPos, Vector3 relativeCubePosition, string screenCaptureName)
    {
        
        this.pixel_x = screenPos[0];
        this.pixel_y = 1028 - screenPos[1]; // we need to switch from bottom left corner to top left corner

        this.x = relativeCubePosition[0];
        this.y = relativeCubePosition[1];
        this.z = relativeCubePosition[2];

        this.screenCaptureName = screenCaptureName;
    }

}

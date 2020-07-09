using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    public float pixel_position_x; 
    public float pixel_position_y;

    public string screenCaptureName;

    public RobotVisionDataPoint(Vector2 screenPos, string screenCaptureName)
    {
        
        this.pixel_position_x = screenPos[0];
        this.pixel_position_y = screenPos[1];

        this.screenCaptureName = screenCaptureName;
    }

}

using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    public float x_cube;
    public float y_cube;
    public float z_cube;

    public string screenCaptureName;

    public RobotVisionDataPoint(Vector3 relativeCubePosition, string screenCaptureName)
    {
        this.x_cube = relativeCubePosition.x;
        this.y_cube = relativeCubePosition.y;
        this.z_cube = relativeCubePosition.z;

        this.screenCaptureName = screenCaptureName;
    }

}

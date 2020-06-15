using UnityEngine;

[System.Serializable]
public class RobotVisionDataPoint
{
    public float x_cube;
    public float y_cube;
    public float z_cube;
    public float x_cylinder;
    public float y_cylinder;
    public float z_cylinder;

    public string screenCaptureName;

    public RobotVisionDataPoint(Vector3 relativeCubePosition, Vector3 relativeCylinderPosition, string screenCaptureName)
    {
        this.x_cube = relativeCubePosition.x;
        this.y_cube = relativeCubePosition.y;
        this.z_cube = relativeCubePosition.z;

        this.x_cylinder = relativeCylinderPosition.x;
        this.y_cylinder = relativeCylinderPosition.y;
        this.z_cylinder = relativeCylinderPosition.z;

        this.screenCaptureName = screenCaptureName;
    }

}

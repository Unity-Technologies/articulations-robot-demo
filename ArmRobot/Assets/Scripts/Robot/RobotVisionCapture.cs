using UnityEngine;


public class RobotVisionCapture : MonoBehaviour
{
    public GameObject cube;
    public GameObject robot;
    public GameObject cylinder;


    private void Update()
    {
        VisionDataCollector visionDataCollector = GetComponent<VisionDataCollector>();
        string imageName = visionDataCollector.NextImageName();
        Vector3 relativeCubePosition = cube.transform.position - robot.transform.position;
        Vector3 relativeCylinderPosition = cylinder.transform.position - robot.transform.position;
        RobotVisionDataPoint dataPoint = new RobotVisionDataPoint(relativeCubePosition, relativeCylinderPosition, imageName);

        bool didCapture = visionDataCollector.CaptureIfNecessary(imageName, dataPoint);

        if (didCapture)
        {
            Reset();
        }
    }

    // HELPERS

    private void Reset()
    {
        // move cube
        TablePositionRandomizer tablePositionRandomizer_cube = cube.GetComponent<TablePositionRandomizer>();
        tablePositionRandomizer_cube.Move();

        // move cylinder
        TablePositionRandomizerCylinder tablePositionRandomizer_cylinder = cylinder.GetComponent<TablePositionRandomizerCylinder>();
        tablePositionRandomizer_cylinder.Move();

        // randomize robot position
        RobotController robotController = robot.GetComponent<RobotController>();
        float[] rotation = {Random.value * 10, Random.value * 10, Random.value * 10, Random.value * 10, Random.value * 10, Random.value * 10};
        robotController.ForceJointsToRotations(rotation);
    }


}

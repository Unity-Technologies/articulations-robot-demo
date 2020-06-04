using RosSharp.RosBridgeClient;
using UnityEngine;
using RobotMessageType = Assets.RosSharp.ROSMessageTypes.RobotMessageType;
public class ROSTransformSubscriber : Subscriber<RobotMessageType>
{
    public GameObject robot;
    private float previousRealTime;
    private RobotMessageType receivedMessage;
    private bool isMessageReceived;
    private RobotController robotController;
    private const int numberOfJoints = 6;
    bool isCool = true;
    private const float speed = 50;
    private float[] currentPoseToRotateTo;
    private bool isRotationFinished = false;

    protected override void Start()
    {
        base.Start();
        robotController = robot.GetComponent<RobotController>();
        //gripping pose
        //currentPoseToRotateTo = new float[] { 17.8f, 57.55f, -31.1f, -99.8f, -9.4f, 14.7f };
        currentPoseToRotateTo = new float[] { 17.8f, 30f, 20f, -10f, 15f, 10f};


    }

    protected override void ReceiveMessage(RobotMessageType message)
    {
        receivedMessage = message;
        isMessageReceived = true;
    }

    private void FixedUpdate()
    {
        // if (isMessageReceived)
        ProcessMessage();
    }
    private void ProcessMessage()
    {
        Debug.Log(isRotationFinished);
        if (!isRotationFinished)
        {
            isRotationFinished = MoveJoints();

        }

    }

    private bool MoveJoints()
    {
        isRotationFinished = true;
        for (int i = 0; i < numberOfJoints; i++)
        {
            ArticulationBody articulation = robotController.joints[i].robotPart.GetComponent<ArticulationBody>();
            float currentRotation = robotController.joints[i].robotPart.GetComponent<ArticulationJointController>().CurrentPrimaryAxisRotation();
            float rotationChange = speed * Time.fixedDeltaTime;
            if (currentPoseToRotateTo[i] < currentRotation)
                rotationChange *= -1;

            //if within a rotationChange distance, then set to the currentPosteToRotate[i]
            if (Mathf.Abs(currentPoseToRotateTo[i] - currentRotation) < rotationChange)
                rotationChange = 0;
            else
            {
                isRotationFinished = false;
            }

            float rotationGoal = currentRotation + rotationChange;

            var drive = articulation.xDrive;

            drive.target = rotationGoal;
            articulation.xDrive = drive;

        }
        isMessageReceived = false;
        return isRotationFinished;

    }


}

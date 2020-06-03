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
    private const float speed = 100;
    protected override void Start()
    {
        base.Start();
        robotController = robot.GetComponent<RobotController>();

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
        //Debug.Log(receivedMessage.robot_base_target);
        MoveJoints();

    }

    private void MoveJoints()
    {
        float[] grippingPose = { 17.8f, 57.55f, -31.1f, -99.8f, -9.4f, 14.7f };
        for (int i = 0; i < numberOfJoints; i++)
        {
            ArticulationBody articulation = robotController.joints[i].robotPart.GetComponent<ArticulationBody>();
            float currentRotation = robotController.joints[i].robotPart.GetComponent<ArticulationJointController>().CurrentPrimaryAxisRotation();
            float rotationChange = speed * Time.fixedDeltaTime;
            float rotationGoal = currentRotation + rotationChange;
            var drive = articulation.xDrive;
            if (Mathf.Abs(rotationGoal) > grippingPose[i])
                rotationGoal = grippingPose[i];
            drive.target = rotationGoal;
            articulation.xDrive = drive;

        }
        isMessageReceived = false;

    }


}

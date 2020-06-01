using RosSharp.RosBridgeClient;
using UnityEngine;
using RobotMessageType = Assets.RosSharp.ROSMessageTypes.RobotMessageType;

public class ROSMovementPublisher : Publisher<RobotMessageType>
{

    public GameObject robot;
    private RobotMessageType message;
    private RobotController robotController;

    protected override void Start()
    {
        //get UR3 controller
        robotController = robot.GetComponent<RobotController>();
        base.Start();
        InitializeMessage();

    }

    private void Update()
    {
        UpdateMessage();

    }

    //creates a new message of type RobotMessageType
    private void InitializeMessage()
    {
        message = new RobotMessageType();

    }

    //function that sends an updated message that contains the robot state
    private void UpdateMessage()
    {
        //0 corresponds to the base, 1 to the shoulder, 2 to the elbow, 3 to the wrist1, 4 to the wrist2, 5 to the wrist3, and 6 to the hand
        message.robot_base = (int)robotController.joints[0].robotPart.GetComponent<ArticulationJointController>().rotationState;
        message.shoulder = (int)robotController.joints[1].robotPart.GetComponent<ArticulationJointController>().rotationState;
        message.elbow = (int)robotController.joints[2].robotPart.GetComponent<ArticulationJointController>().rotationState;
        message.wrist1 = (int)robotController.joints[3].robotPart.GetComponent<ArticulationJointController>().rotationState;
        message.wrist2 = (int)robotController.joints[4].robotPart.GetComponent<ArticulationJointController>().rotationState;
        message.wrist3 = (int)robotController.joints[5].robotPart.GetComponent<ArticulationJointController>().rotationState;
        message.hand = (int)robotController.joints[6].robotPart.GetComponent<ArticulationJointController>().rotationState;
        Publish(message);
    }

}

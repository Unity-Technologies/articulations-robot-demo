using RosSharp.RosBridgeClient;
using UnityEngine;
using RobotMessageType = Assets.RosSharp.ROSMessageTypes.RobotMessageType;

public class ROSMovementPublisher : Publisher<RobotMessageType>
{

    public GameObject robot;
    private RobotMessageType message;
    private RobotController robotController;
    public MeshRenderer renderer; 
    protected override void Start()
    {
        //get UR3 controller
        robotController = robot.GetComponent<RobotController>();
        base.Start();
        InitializeMessage();
        Debug.Log(renderer.bounds.size);

    }

    private void FixedUpdate()
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
        message.robot_base_target = robotController.joints[0].robotPart.GetComponent<ArticulationBody>().xDrive.target;
        message.shoulder_target = robotController.joints[1].robotPart.GetComponent<ArticulationBody>().xDrive.target;
        message.elbow_target = robotController.joints[2].robotPart.GetComponent<ArticulationBody>().xDrive.target;
        message.wrist1_target = robotController.joints[3].robotPart.GetComponent<ArticulationBody>().xDrive.target;
        message.wrist2_target = robotController.joints[4].robotPart.GetComponent<ArticulationBody>().xDrive.target;
        message.wrist3_target = robotController.joints[5].robotPart.GetComponent<ArticulationBody>().xDrive.target;
        message.hand_target = robotController.joints[6].robotPart.GetComponent<ArticulationBody>().xDrive.target;
        Publish(message);
    }

}

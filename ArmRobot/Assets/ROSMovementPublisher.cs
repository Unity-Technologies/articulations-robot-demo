using RosSharp.RosBridgeClient;
using Messages = RosSharp.RosBridgeClient.Messages;
using RobotMessageType = RosSharp.RosBridgeClient.Messages.RobotMessageType;
using UnityEngine;
using System.Collections.Generic;

public class ROSMovementPublisher : Publisher<RobotMessageType>
{

    public GameObject robot;
    private RobotMessageType message;
    private RobotController robotController;
    private Dictionary<int, string> robotJointIntToRosMessageVar;

    protected override void Start()
    {
        robotController = robot.GetComponent<RobotController>();
        base.Start();
        InitializeMessage();
        robotJointIntToRosMessageVar = new Dictionary<int, string>()
                                            {
                                                {0,"One"},
                                                {1, "Two"},
                                                {2,"Three"},
                                                {3,"Three"},
                                                {4,"Three"},
                                                {5,"Three"},
                                                {6,"Three"},
                                            };

    }

    private void Update()
    {
        UpdateMessage();

    }

    private void InitializeMessage()
    {
        message = new RobotMessageType();


    }

    private void UpdateMessage()
    {
        //0 corresponds to the base, 1 to the shoulder.....
        message.robotbase = RotationDirectionToInt(robotController.joints[0].robotPart.GetComponent<ArticulationJointController>().rotationState);
        message.shoulder = RotationDirectionToInt(robotController.joints[1].robotPart.GetComponent<ArticulationJointController>().rotationState);
        message.elbow = RotationDirectionToInt(robotController.joints[2].robotPart.GetComponent<ArticulationJointController>().rotationState);
        message.wrist1 = RotationDirectionToInt(robotController.joints[3].robotPart.GetComponent<ArticulationJointController>().rotationState);
        message.wrist2 = RotationDirectionToInt(robotController.joints[4].robotPart.GetComponent<ArticulationJointController>().rotationState);
        message.wrist3 = RotationDirectionToInt(robotController.joints[5].robotPart.GetComponent<ArticulationJointController>().rotationState);
        message.hand = RotationDirectionToInt(robotController.joints[6].robotPart.GetComponent<ArticulationJointController>().rotationState);
        Publish(message);
    }

    //convert the rotation direction from the defined enum back to an int so that we can put it in the ROS message 
    static int RotationDirectionToInt(RotationDirection inputRotation)
    {
        if (inputRotation == RotationDirection.Positive)
        {
            return 1;
        }
        else if (inputRotation == RotationDirection.None)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
}

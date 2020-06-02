using RosSharp.RosBridgeClient;
using Messages = RosSharp.RosBridgeClient.Messages;
using UnityEngine;
using Time = UnityEngine.Time;
using System.Collections.Generic;
public class ROSMovementController : Subscriber<Messages.Standard.String>
{
    public GameObject robot;
    private float previousRealTime;
    private string receivedMessage;
    private bool isMessageReceived;
    private RobotController robotController;
    private Dictionary<string, int> keyToJointIndex;
    private Dictionary<string, RotationDirection> keyToDirection;
    private int count = 0;
    protected override void Start()
    {
        base.Start();
        robotController = robot.GetComponent<RobotController>();
        keyToJointIndex = new Dictionary<string, int>()
                                            {
                                                {"a", 0},
                                                {"d", 0},
                                                {"s",1},
                                                {"w", 1},
                                                {"q", 2},
                                                {"e",2},
                                                {"o", 3},
                                                {"p", 3},
                                                {"k",4},
                                                {"l", 4},
                                                {"n",5},
                                                {"m", 5},
                                                {"v", 6},
                                                {"b",6},

                                            };

        keyToDirection = new Dictionary<string, RotationDirection>()
                                            {
                                                {"a", RotationDirection.Negative},
                                                {"d", RotationDirection.Positive},
                                                {"s",RotationDirection.Negative},
                                                {"w", RotationDirection.Positive},
                                                {"q", RotationDirection.Negative},
                                                {"e",RotationDirection.Positive},
                                                {"o", RotationDirection.Negative},
                                                {"p", RotationDirection.Positive},
                                                {"k",RotationDirection.Positive},
                                                {"l", RotationDirection.Positive},
                                                {"n",RotationDirection.Negative},
                                                {"m", RotationDirection.Positive},
                                                {"v", RotationDirection.Negative},
                                                {"b",RotationDirection.Positive}

                                            };


    }

    protected override void ReceiveMessage(Messages.Standard.String message)
    {
        receivedMessage = message.data;
        isMessageReceived = true;
    }

    private void FixedUpdate()
    {
        if(isMessageReceived)
            ProcessMessage();
    }
    private void ProcessMessage()
    {
        Debug.Log("Process message count " + count);
        count+=1; 
        float deltaTime = Time.realtimeSinceStartup - previousRealTime;
        
        //get the joint to rotate from the entered key
        int jointIndexToRotate = keyToJointIndex[receivedMessage];
        //get the rotation to rotate in from the entered key
        RotationDirection rotationDirection = keyToDirection[receivedMessage];
        //rotate the joint
        robotController.RotateJoint(jointIndexToRotate, rotationDirection);
        //Physics.Simulate(Time.fixedDeltaTime);
        //robotController.joints[0].robotPart.GetComponent<ArticulationJointController>().FixedUpdate();
        // robotController.joints[1].robotPart.GetComponent<ArticulationJointController>().FixedUpdate();




        previousRealTime = Time.realtimeSinceStartup;

        isMessageReceived = false;
    }
}

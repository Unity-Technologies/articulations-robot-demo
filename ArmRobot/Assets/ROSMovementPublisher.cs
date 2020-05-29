using RosSharp.RosBridgeClient;
using Messages = RosSharp.RosBridgeClient.Messages;
using RobotMessageType = RosSharp.RosBridgeClient.Messages.RobotMessageType;
using UnityEngine;

public class ROSMovementPublisher : Publisher<RobotMessageType>
{

    private RobotMessageType message; 
    

    protected override void Start()
    {
        base.Start();
        InitializeMessage();
    }

    private void Update()
    {
        UpdateMessage();

    }

    private void InitializeMessage(){
        message = new RobotMessageType(); 
        message.robotbase = 0;
    }

    private void UpdateMessage(){
        Publish(message);
    }
}

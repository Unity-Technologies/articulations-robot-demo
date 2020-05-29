using RosSharp.RosBridgeClient;
using Messages = RosSharp.RosBridgeClient.Messages;

using UnityEngine;
using Time = UnityEngine.Time;
using System.Collections.Generic;


public class ROSMovementPublisher : Publisher<Messages.Standard.String>
{

    private Messages.Standard.String message;
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
        message = new Messages.Standard.String(); 
    }

    private void UpdateMessage(){
        message.data = "hello world!";
        Publish(message);
    }
}

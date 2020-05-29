using RosSharp.RosBridgeClient;
using Messages = RosSharp.RosBridgeClient.Messages;

using UnityEngine;
using Time = UnityEngine.Time;
using System.Collections.Generic;

public class ROSMovementController : Subscriber<Messages.Standard.String>
{
    public Transform SubscribedTransform;
    public GameObject robot;


    public Vector3 lookDir;
    public float movementMultiplier = 0.05f;
    private float previousRealTime;
    private string receivedMessage;
    private bool isMessageReceived;
    //used to map incoming keys to the proper axis
    private Dictionary<string, string> dict;

    protected override void Start()
    {
        lookDir = SubscribedTransform.position;
        base.Start();
        // dict = new Dictionary<string, string>() { {"w","Shoulder" }, {""}}
    }

    static RotationDirection GetRotationDirection(float inputVal)
    {
        if (inputVal > 0)
        {
            return RotationDirection.Positive;
        }
        else if (inputVal < 0)
        {
            return RotationDirection.Negative;
        }
        else
        {
            return RotationDirection.None;
        }
    }
    protected override void ReceiveMessage(Messages.Standard.String message)
    {
        receivedMessage = message.data;
        isMessageReceived = true;
    }

    private void Update()
    {
        if (isMessageReceived)
            ProcessMessage();
    }
    private void ProcessMessage()
    {
        float deltaTime = Time.realtimeSinceStartup - previousRealTime;

        // Forward
        if (receivedMessage == "w")
        {
            RobotController robotController = robot.GetComponent<RobotController>();
            robotController.RotateJoint(1, GetRotationDirection(1f));
        }
        // Back
        if (receivedMessage == "s")
            SubscribedTransform.Translate(0, 0, lookDir.z * Time.deltaTime * movementMultiplier);

        // Left
        if (receivedMessage == "a")
            SubscribedTransform.Rotate(SubscribedTransform.rotation.x,
                SubscribedTransform.rotation.y * Time.deltaTime - 1, SubscribedTransform.rotation.z);

        // Right
        if (receivedMessage == "d")
            SubscribedTransform.Rotate(SubscribedTransform.rotation.x,
                SubscribedTransform.rotation.y * Time.deltaTime + 1, SubscribedTransform.rotation.z);


        previousRealTime = Time.realtimeSinceStartup;

        isMessageReceived = false;
    }
}

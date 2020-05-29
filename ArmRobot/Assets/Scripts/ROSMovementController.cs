using RosSharp.RosBridgeClient;
using Messages = RosSharp.RosBridgeClient.Messages;

using UnityEngine;
using Time = UnityEngine.Time;

public class ROSMovementController : Subscriber<Messages.Standard.String>
{
    public Transform SubscribedTransform;
    public ArticulationBody tester; 
    public GameObject robot;


    public Vector3 lookDir;
    public float movementMultiplier = 0.05f;
    private float previousRealTime;
    private string receivedMessage;
    private bool isMessageReceived;

    protected override void Start()
    {
        lookDir = SubscribedTransform.position;
        base.Start();
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

            Debug.Log("cool!");

            var drive = tester.xDrive;
            drive.target += 10;
            tester.xDrive = drive;

            SubscribedTransform.Translate(0, 0, lookDir.z * Time.deltaTime * movementMultiplier * -1);
            RobotController robotController = robot.GetComponent<RobotController>();
            for (int i = 0; i < robotController.joints.Length; i++)
            {   
                //Filler number for now
                float inputVal = 1f; 
                RotationDirection direction = GetRotationDirection(inputVal);
                robotController.RotateJoint(i, direction);

            }

            robotController.StopAllJointRotations();
        }
        // Back
        if (receivedMessage == "s")
            Debug.Log("cool");
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

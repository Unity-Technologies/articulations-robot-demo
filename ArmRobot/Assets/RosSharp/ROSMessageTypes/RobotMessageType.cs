/*
This message class is generated automatically with 'SimpleMessageGenerator' of ROS#
*/

using Newtonsoft.Json;
using RosSharp.RosBridgeClient;


namespace Assets.RosSharp.ROSMessageTypes
{
    public class RobotMessageType : Message
    {
        [JsonIgnore]
        public const string RosMessageName = "ros_unity_control/RobotMessageType";

        public int robot_base;
        public int shoulder;
        public int elbow;
        public int wrist1;
        public int wrist2;
        public int wrist3;
        public int hand;

        public RobotMessageType()
        {
            robot_base = new int();
            shoulder = new int();
            elbow = new int();
            wrist1 = new int();
            wrist2 = new int();
            wrist3 = new int();
            hand = new int();
        }
    }
}


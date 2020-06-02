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

        public float robot_base_target;
        public float shoulder_target;
        public float elbow_target;
        public float wrist1_target;
        public float wrist2_target;
        public float wrist3_target;
        public float hand_target;

        public RobotMessageType()
        {
            robot_base_target = new float();
            shoulder_target = new float();
            elbow_target = new float();
            wrist1_target = new float();
            wrist2_target = new float();
            wrist3_target = new float();
            hand_target = new float();
        }
    }
}


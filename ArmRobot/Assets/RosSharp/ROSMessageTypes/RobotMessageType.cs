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

        public float shoulder_target;
        public float upper_arm_target;
        public float forearm_target;
        public float wrist1_target;
        public float wrist2_target;
        public float wrist3_target;



        public RobotMessageType()
        {
            shoulder_target = new float();
            upper_arm_target = new float();
            forearm_target = new float();
            wrist1_target = new float();
            wrist2_target = new float();
            wrist3_target = new float();
        }


        public float[] GetAnglesAsArray()
        {
            float[] arrayToReturn = { shoulder_target, upper_arm_target, forearm_target, wrist1_target, wrist2_target, wrist3_target };
            return arrayToReturn;
        }
    }
}


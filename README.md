# Articulations Demo Robot

A simulation of the Universal Robotics UR3 robot with articulation joints.

Requires 2020.1.0b1 build of Unity or later.

Open the `ArmRobot` folder in Unity.

Open `Scenes` > `ArticulationRobot` and press play.

### Manual Controls
```
A/D - rotate base joint
S/W - rotate shoulder joint
Q/E - rotate elbow joint
O/P - rotate wrist1
K/L - rotate wrist2
N/M - rotate wrist3
V/B - rotate hand
X - close pincher
Z - open pincher
```

All manual control is handled through the scripts on the `ManualInput` object. To disable
manual input, just uncheck this object in the Hierarchy window.

### License

[Apache License 2.0](LICENSE)

### MLAgents

To install:

1. Download MLAgents 0.15.0 from Github with: `git clone https://github.com/Unity-Technologies/ml-agents.git`
2. Add MLAgents to the Unity project:
     a. Go to `Window` > `Package Manager`
     b. Select the plus button in the upper left corner, and then `add package from disk`
     c. Navigate to the MLAgents project you just cloned, and within that select `com.unity.ml-agents` > `package.json`
3. Install the corresponding MLAgents python package with `pip3 install mlagents==0.15.0`





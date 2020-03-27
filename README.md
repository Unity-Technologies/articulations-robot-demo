# Articulations Demo Robot

A simulation of the Universal Robotics UR3 robot with articulation joints.

Requires 2020.1.0b1 build of Unity or later.

Open the `ArmRobot` folder in Unity.

Open `Scenes` > `ArticulationRobot` and press play.

## Manual Controls

All manual control is handled through the scripts on the `ManualInput` object. To disable
manual input, just check this object in the Hierarchy window, and uncheck the `MLAgents` object.

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

## MLAgents

#### Install 

If you have MLAgents errors upon opening the project, uninstall MLAgents (look under `Window` > `Package Manager`), then reinstall as shown below.

1. Download MLAgents 0.15.0 from Github with: `git clone https://github.com/Unity-Technologies/ml-agents.git`
2. Navigate into that folder and run `git checkout release-0.15.0`
2. To add MLAgents to the Unity project:
     a. Go to `Window` > `Package Manager`
     b. Select the plus button in the upper left corner, and then `add package from disk`
     c. Navigate to the MLAgents project you just cloned, and within that select `com.unity.ml-agents` > `package.json`
3. Install the corresponding MLAgents python package with `pip3 install mlagents==0.15.0`

#### Train

`mlagents-learn config.yaml --run-id=[YOUR RUN ID] --train`

## License

[Apache License 2.0](LICENSE)

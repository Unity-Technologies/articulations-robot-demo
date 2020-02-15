# Articulations Demo Robot

A simulation of the Universal Robotics UR3 robot with articulation joints.

Requires 2020.1-alpha build of Unity (2020.1.0a23 or later).

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

Note: There's a lag on manual control in 2020.1.0a23, but the issue is not with the physics engine.
It seems that getting the input axis is slow for some reason.

### MLAgents

There is an issue installing MLAgents with 2020.1-alpha, documented here:
https://github.com/Unity-Technologies/ml-agents/issues/3109

To get around this, this project has its own local version of mlagents. If you have mlagents errors upon opening this project:
1. Uninstall MLAgents in `Window` > `Package Manager`
2. Reinstall it by clicking the plus button near the top of the package manager, selecting `Add package from disk`, and selecting `ml-agents/com.unity.ml-agents/package.json` in this project.

This should get rid of the errors.




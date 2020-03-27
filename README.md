# Articulations Demo Robot

This is a simulation of the [Universal Robotics UR3e](https://www.universal-robots.com/products/ur3-robot/) robot using Unity's new [articulation joint system](https://docs.unity3d.com/2020.1/Documentation/ScriptReference/ArticulationBody.html).


### Getting Started

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





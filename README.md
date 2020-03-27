# Articulations Demo Robot

This is a simulation of the [Universal Robotics UR3e](https://www.universal-robots.com/products/ur3-robot/) robot using Unity's new [articulation joint system](https://docs.unity3d.com/2020.1/Documentation/ScriptReference/ArticulationBody.html).

This new joint system, powered by [Nvidia's PhysX 4](https://news.developer.nvidia.com/announcing-physx-sdk-4-0-an-open-source-physics-engine/), is a dramatic improvement over the older joint types available in Unity. It uses Featherstone's algorithm and a reduced coordinate representation to gaurantee no unwanted stretch in the joints. In practice, this means that we can now chain many joints in a row and still achieve stable and precise movement.

This, along with other key improvements in PhysX 4, unlocks state of the art robotics simulation in Unity.


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





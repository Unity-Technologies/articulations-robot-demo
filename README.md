# Articulations Robot Demo

<img align="right" style="padding-left: 10px; padding-right: 10px; padding-bottom: 10px" width="350px" src="images/RobotHandDemo.gif">

This is a simulation of the [Universal Robotics UR3e](https://www.universal-robots.com/products/ur3-robot/) robot using Unity's new [articulation joint system](https://docs.unity3d.com/2020.1/Documentation/ScriptReference/ArticulationBody.html).

This new joint system, powered by [Nvidia's PhysX 4](https://news.developer.nvidia.com/announcing-physx-sdk-4-0-an-open-source-physics-engine/), is a dramatic improvement over the older joint types available in Unity. It uses Featherstone's algorithm and a reduced coordinate representation to gaurantee no unwanted stretch in the joints. In practice, this means that we can now chain many joints in a row and still achieve stable and precise movement. 


## Getting Started

Requires `2020.1.0b1` build of Unity or later. To get started:
1. Open the `ArmRobot` folder in Unity.
2. Open `Scenes` > `ArticulationRobot`
3. Press play

## Manual Controls

You can move the robot around manually using the following keyboard commands:

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

## Building with Articulations

When building a robot arm with articulation joints, each movable part of the robot should be a child of the previous part. You can see these successive parent-child relationships by expanding the `UR3` object in the Hierarchy window. 

<img width="300px" src="images/parent-child.png">

Next, you must add an `ArticulationBody` component to each of the game objects that compose the robot arm. You will not need to add `Rigidbody` components to any parts of the robot arm, but you may still want to add them to other objects in the environment that the robot will interact with. In our example, there are `Rigidbody` components only on the cube and the table. 

### The Root Body

If you examine the `ArticulationBody` component on the base of the robot (the `UR3` game object), you will notice that it has a very simple interface. This is the root body of the articulation, which plays a special role. The `immovable` property should be checked if you do not intend this part of the robot to move around.

<img width="500px" src="images/articulation_base.png">

The successive `ArticulationBody` components on your robot arm are much more customizable. We will dive into these settings next.

<img width="500px" src="images/articulation_other.png">

### Articulation Joint Types

The most important setting here is `Articulation Joint Type`. The available types are:
* `Fixed` - does not allow any relative movement of the connected bodies
* `Prismatic` - only allows relative translation of the connection bodies along one specified axis
* `Revolute` - allows rotational movement around the X axis of the parent's anchor
* `Spherical` - allows relative rotations (but not translations) of the two connected bodies

Since all the joints on our robot arm rotate on only one axis, all of our arm's articulation joints are `Revolute`. Although `Revolute` joints are contrained to only rotate around the X axis, you can use the `Angular Rotation` property to rotate the entire articulation body such that its X axis points in the desired direction. The two pincher fingers on the hand are `Prismatic` because they slide back and forth on one axis. 

### Damping and Friction
All the joint types have the following physical parameters:

* `Linear Damping` - Resistance that will slow down linear motion of the joint
* `Angular Damping` - Resistance that will slow down angular motion of the joint
* `Joint Friction` - Amount of friction that is applied as a result of connected bodies moving relative to this body

### Drives
There are two ways to move articulation joints - by applying forces, or by using drives. In this project, we use drives. A drive attempts to move the joint to the specified `target` or at the specified `target velocity`. Here, we move our revolute joints by updating the `target` to the desired rotation in degrees. You can see this being done in the `RotateTo` method in the `ArticulationJointController` script. 

The drive acts like a spring in its attempt to achieve and maintain the `target`.

On the drive, you can also specify:
* `Stiffness` - The stiffness of the spring connected to this drive.
* `Damping` - The damping of the spring attached to this drive.
* `Force Limit` - The maximum force this drive can apply to a body.

### Limits

By default, the `Motion` property on the articulation components will be set to `Free`. For a revolute joint, this means that the joint can rotate indefinitely. However, real systems rarely act this way. In the real UR3 robot, safeguards prevent the joints from moving beyond two full revolutions in either direction. Exceeding this limit in the real robot would twist the wires inside and damage the hardware.

We can mimic this in simulation by adding limits to our joints. To do this, change the `Motion` selection to `Limited` in the dropdown. Doing so on the revolute joint will add two new properties to the drive - a `Lower Limit` and an `Upper Limit`, defined in degrees.


## License

[Apache License 2.0](LICENSE)





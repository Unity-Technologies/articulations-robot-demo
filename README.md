# Training the UR3

<img align="right" style="padding-left: 10px; padding-right: 10px; padding-bottom: 10px" height="200px" src="images/robot_still.png">

This is a simulation of the [Universal Robotics UR3e](https://www.universal-robots.com/products/ur3-robot/) robot using Unity's new [articulation joint system](https://docs.unity3d.com/2020.1/Documentation/ScriptReference/ArticulationBody.html).

This new joint system, powered by [Nvidia's PhysX 4](https://news.developer.nvidia.com/announcing-physx-sdk-4-0-an-open-source-physics-engine/), is a dramatic improvement over the older joint types available in Unity. It uses Featherstone's algorithm and a reduced coordinate representation to gaurantee no unwanted stretch in the joints. In practice, this means that we can now chain many joints in a row and still achieve stable and precise movement.

## Requirements

Unity 2020.1.0b1 or later is needed for the new joint system. This UR3 demo works
with [ML-Agents release_1](https://github.com/Unity-Technologies/ml-agents/tree/release_1)
to demonstrate the UR3 learning to touch a cube.

## Installation

To install this demo and experiment with the UR3 simulation and ML-Agents training you will
need an appropriate version of Unity, a clone of this repo and the ML-Agents Toolkit.

#### Install Unity

If you do not have Unity 2020.1.0b1 or later, add the latest 2020.1 beta release
through [Unity Hub](https://unity3d.com/get-unity/download). This demo has been
last tested on Unity 2020.1.0b7.

#### Clone the Articulations Robot Demo Repo

Clone this branch of the repository:
```sh
git clone --branch mlagents https://github.com/Unity-Technologies/articulations-robot-demo`.
```

Alternatively, you can clone the entire repo and checkout the `mlagents` branch:
```sh
git clone https://github.com/Unity-Technologies/articulations-robot-demo
git checkout mlagents
```

The `ArmRobot` folder contains the `ArticulationRobot` scene. If you open the scene
in Unity you will see a few errors since the ML-Agents package still needs to be added.

#### Install the Unity ML-Agents Toolkit

Detailed instructions for installing release_1 of the ML-Agents Toolkit can be found on the
[ml-agents repo](https://github.com/Unity-Technologies/ml-agents/blob/release_1/docs/Installation.md).
However, do skip the [Install Unity section](https://github.com/Unity-Technologies/ml-agents/blob/release_1/docs/Installation.md#install-unity-20184-or-later) as we've already covered the necessary
Unity installation needed for this demo.

## UR3 Demo

After you have complete the installation, open the `ArmRobot` folder in Unity.
Then open `Scenes` > `ArticulationRobot`.

### Manual Controls

All manual control is handled through the scripts on the `ManualInput` object. The scene
is set-up for manual control - hit the **Play** button in the Unity Editor and experiment
with moving the arm using the controls defined below:
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

space - instant reset
```

Note that ManualInput must be set inactive in the scene if you are using the Agent to control
the arm and vice-versa.

### Training using the Unity ML-Agents Toolkit

In this project, we used the [Unity ML-Agents Toolkit](https://github.com/Unity-Technologies/ml-agents) to train the robot to touch the cube.

The agent controls the robot at the level of its six joints in a discrete manner. It uses a 'joint index' to select the joint, and an 'action index' to move that joint clockwise, counterclockwise, or not at all. For observations, the agent is given the current rotation of all six joints and the position of the cube.

The reward at each step is simply the negative distance between the end effector and the cube. The advantage of this is that changes in reward are continuous and it does incentivize the agent to pursue our goal of touching the cube. However, other reward schemes may work as well or better. You should experiment!

The episodes end immediately when the robot touches the cube, and are limited to a maximum of 500 steps. At the beginning of each new episode, the robot is reset to its base pose, and the cube is moved to a random location on the table.

#### Using a Pre-trained Model

This project comes with a pre-trained model using the ML-Agents Toolkit. To see that behavior
in action, just uncheck the `ManualInput` object in the Hierarchy window and check the `MLAgents`
object. Then hit **Play** in the Unity Editor.

#### Train

You can also train the arm yourself. Detailed instructions on how to train using the ML-Agents Toolkit can be found on the [ml-agents repo](https://github.com/Unity-Technologies/ml-agents/blob/release_1/docs/Training-ML-Agents.md).

To start training, just run this command:

`mlagents-learn ur3_config.yaml --run-id=[YOUR RUN ID]`

Then, press **Play** in the Unity Editor.

As the training runs, a `models` and a `summaries` folder will be automatically created in the root level of this project. As you might guess, the `models` folder stores the trained model files, and the `summaries` folder stores the event files where Tensorflow writes logs.

To monitor training, navigate into the `summaries` folder and run:

`tensorboard --logdir=[YOUR RUN ID]_TouchCube`

You can then view the live Tensorboard at `localhost:6006`.

#### Our Results

Since there is some randomness in the training process, your results might not look exactly like ours. Our reward gradually increased during training, as expected:

<img width='300px' src='images/reward.png'>

Since the reward was quite noisy, we used heavy smoothing in this graph to more easily see the trend. Tensorboard makes this easy, and you might want to try the same.

## Support

This demo is provided as-is without guarantees. Please [submit
an issue](https://github.com/Unity-Technologies/articulations-robot-demo/issues) if:
* You run into issues downloading or installing this demo after following the instructions above.
* You extend this demo in an interesting way. We may not be able to provide support, but would love to hear about it.


## License

[Apache License 2.0](LICENSE)

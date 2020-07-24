# Multiple objects Environment 

In this branch, we set up an environment with a [Universal Robotics UR3e](https://www.universal-robots.com/products/ur3-robot/) robot on a table along with a cube. 
<img align="right" style="padding-left: 10px; padding-right: 10px; padding-bottom: 10px" height="250px" src="docs/images/single_cube.png">
Open `Scenes` > `ArticulationRobot`.



## Purpose of the Scene 
This scene has been set up to predict the coordonates of the center of the cube in order to feed some algorithm as a Neural Network. The idea behind is to train neural network only using simulated data instead of real data. Nevertheless, the neural network will be then applied on real data. The focus is hence to decrease the reality gap as much as possible which is the difference between the simulated and the real worl. Domain randomization is one of the many techniques enabling to reduce the reality gap. Instead of training a model only on one simulator, we randomize the simulator to expose the model to multiple different environments during training time. 

<img align="right" style="padding-left: 10px; padding-right: 10px; padding-bottom: 10px" height="250px" src="docs/images/multiple_objects_domain_randomization.png">

Below is the list of element we randomize:
- The number of source lights  
- The direction, the position and the intensity of the light for the source lights
- The number of distractor objects (either cylinders or spheres) on the table
- The position and the texture of the objects on the table  
- The position of the robot  
- The position, orientation and field of view of the camera 
- The texture of the table and the robot 

The domain randomization process is controlled by the script `DomainRandomization.cs` under `Assets` > `Scripts`

`IMPORTANT`: to activate more than one object on the scene you need to go in the `Hierarchy` project and select `DomainRandomizationObject` game object. Go in the `Inspector` pannel. Then in the component `DomainRandomization` there is a variable called `NbMaxDistractorObjects`. This variable corresponds to maximum number of distractors (objects other than the cube) than we can generate on the table at each frame (each simulation). 


## Glimpse into the code 
The orchestror file is `Robot` > `RobotVisionCapture.cs`. In this file, at each frame, we extract the position of the cube relative to the camera and we create a datapoint object which is the vector of data we want to extract (see `Robot` > `RobotVisionDataPoint.cs`). Then, we capture the image of the `VisionCamera` and we load the image and the datapoint. We do these two actions in when we call the `CaptureIfNeccessary` method inside `VisionDataCollector.cs` file. Then, if we didn't reach the number of simulated data we want to extract, we repeat the process but we need to change the environment of the data by performing domain randomization. This is done in the `Reset` method. 

Inside `DomainRandomization.cs`, we need to generate new objects at each frame. The number of generated objects is controlled by the variable `nbMaxDistractorObjects` inside inspector in the `DomainRandomizationObject` game object. The objects are randomly either cylinders of spheres. We create those inside the methods `CreateCylinder` and `CreateSphere`. 

we need to move the robot and the different objects in the scence. We distinguish their motion as they are completely different objects. 
For the robot, I create a method `MoveRobot` which calls the method `ForceJointsToRotation` inside the `RobotController.cs` file. This method forces the joints of the robot to move. 
For the other objects, I call the `Move` method from the `RandomizerPositionObject.cs` file. To generate a new position of the cube, I pick a random point between a square of edge minRobotReach and a square of edge maxRobotReach. This is the area where the robot can reach the object and perform task on it. I also change the orientation of the cube.
Then, I need to check if the proposed position does not interfer which other objects. This is done inside the `PositionIsValid` method.  

For the other elements which are randomized, follow the code inside `DomainRandomization.cs` and go to the corresponding files to understand how it is done. 

## Switch from a single cube to a scene composed by a cube and multiple object
`Hierarchy` > `DomainRandomizationObject` then in the `Inspector` pannel, go in the `DomainRandomization` component and set the variable `NbMaxDistractorObjects` to more than 0. 

## Switch to an asymetric cube 
The advantage to switch to an asymmetric cube is to train a model for predicting orientation. We define the orientation of an object by its quaternion represensation and to have an asymmetric cube in the scene they are some few steps to do.

<img align="right" style="padding-left: 10px; padding-right: 10px; padding-bottom: 10px" height="250px" src="docs/images/asymmetric_cube.png">

- in the `Project` pannel click on `Materials`. You will see a file called `NonSymmetricCubeTexture`
- then click on the `Cube` GameObject in the `Hierarchy`. 
- go under `Alphabet Cube Material (Material)`
- drag and drop the `NonSymmetricCubeTexture` file in the `Albedo` variable under `Main Maps`
- go into `Scripts` under `Project pannel` and open the `DomainRandomization.cs` script. Scroll down and comment `//` the lines 45 and 49. These two lines are responsible of imposing a checker pattern to the cube and also of changing its color. 

Then if you want to go back to a symmetric cube, click on the little circle in front of `Albedo` and select `None`. Then you can also change the color of the cube. You can also uncomment lignes 45 and 49 of the `DomainRandomization.cs` script. 


## Run Simulation 
Open `Scenes` > `ArticulationRobot`, and press play. To control the number of data you want to extract, you need to change the value of variable `Max Samples` in the inspector of the `VisionDataCollector` game object. Then, when you press start and the ismulation is over, if you go to the log, you can see the path on your computer where the data was loaded. 

<img align="center" style="padding-left: 10px; padding-right: 10px; padding-bottom: 10px" height="250px" src="docs/images/path_data.png">

Then, there is two folders inside the data folder: Logs and ScreenCapture. The ScreenCapture folder gathers all the images captured by the mainCanera object whereas Logs gathers all the datapoints inside txt files. 


## Survey
Your opinion matters a great deal to us. Only by hearing your thoughts can we continue to make Unity a better simulator for robotics. Please take a few minutes to let us know about it.

[Fill out the survey](https://docs.google.com/forms/d/e/1FAIpQLSc77ah4azt6D4AOxCWhjpCBgM6Si6f0DA_dunM-ZhDf5xJlgg/viewform)


## License
[Apache License 2.0](LICENSE)





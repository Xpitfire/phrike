# Myo Documentation

## Requirement

We were looking for a more intuitive movement control while using the Oculus rift. The target was to make the VR even more realistic. 
We also needed to consider certain limitations:
* Oculus Rift is wired
* No Eyesight in Real World 
* Keep the Balance while using Oculus Rift 
* resource limits (Time only 2 Semester, Human resources) 

All technologies we considered so far:
* **Mouse and Keyboard** is therefor not very practicable, as it is hard to find the correct keys. 
* **Gamepad** is easier to use without eyesight, as all Buttons, Analog Sticks are within reach. Hence the Gamepad is our fallback solution.
* **Kinect 2** is probably the most intuitive Control. Therefore it will be used as our main input controller for movements (Moving forward, turn left, turn right).
* **Virtux Omni** to expensive  
* **Leap Motion** After some basic research it turned out, that Leap Motion could be used for showing the actual hand movements in game, but not really for controlling any movements like moving forward. 
* **Myo Armband** Good gesture recognition, easy to implement as it come up with a Script API as well as an SDK. Some of the gestures will be incorporarted to control the movement or turn speed.


## Basic Information

### Installing Guide

Download the Myo Connect installer [[here](https://www.thalmic.com/start/)]. Once the download finished, double click to run it.
During the installation, you will receive the a security dialog for installing the driver for the Bluetooth adapter packaged with your Myo armband. Click Install to continue.

Once Myo Connect is successfully installed, it will launch by default and the Getting Started Guide display.Click Continue to begin and follow the Guide.

If there is an update available for your Myo, you will be prompted to update before proceeding. Click Update Myo to continue.

After the Guide is successfully finished the Myo is ready.

### Unreal Engine Integration
For the integration of the Myo into the Unreal Engine the getnamo plugin is used.
For more information check the [getnamo repository](https://github.com/getnamo/myo-ue4).
 
## Technology
### Myo 
[Official Website - Thalmic](https://www.thalmic.com/en/myo/)

[Downloads - SDK + Firmware](https://developer.thalmic.com/downloads) 

[Getnamo GitHub](https://github.com/getnamo/myo-ue4)

## Implementation Script (depricated)
### Basic
We decided to use the Script API for our implementation. As it provides everything we need so far and there is no performance or functional win if we go with the SDK unless we use raw data and analyse them on our own( e.g. add new gestures )
  
### Architecture 
 
We decided to use the Arm movements (Pitch & Yaw) as basis for the movement in Game. This means that with a Pitch(Up or Down) you move in Game Forward or Backward, same goes for Yaw(left & right). 

As you can not hold your arm entirely still or rather you don't now exactly were the zero point is, there is a defined neutral area. As for now once you activate movement via a Double Tap the position of your arm will be the Zero point Every time you "unlock" with double Tab the Zero position is set to the current arm position (Yaw & Pitch).

We also added 2 gestures to enabled enhanced movement controls:
* Fist: As long as Fist gesture is active you can control mouse movements instead of character movements( forward,backward,turn left, turn right) 
 * to be tested with oculus rift, could be obsolete due to head tracking 
* fingersSpread: disables the Backward and Forward movements only, that enables you to just turn left and right without unnecessary movements. 

## Implementation in Unreal Engine
### Basic
Initially we used the Script API, but as we decided to use the Kinnect as the main motion controller, the API will not be sufficient anymore. If we implement the Myo controls directly in the Unreal Engine we have more possibilities to directly interact with the Kinnect.

### Architecture 
As most of the movement is now controlled by the Kinnect, the Myo is now only responsible for the movement speed.
The Myo plugin enables us to use the different gestures in the input config.

**Sprint**:
The first step was to add a new input mapping for the sprint key.

![MyoFist](https://gitlab.com/OperationPhrike/phrike/uploads/40b3e933f50e85efd586d48427b125ce/MyoFist.jpg)

Afterwards the character blueprint needs to be modified:

![MyoSprint](https://gitlab.com/OperationPhrike/phrike/uploads/8a1570a935e479583e3651d9921bab44/MyoSprint.jpg)

This increases the movement speed if the "Fist" - gesture is recognized. A normal movement speed is used if no gesture is recognized.

**Turn left/right**:
The same modifications as already described in the point above, have to be done to add turning.
With this setup the character can turn left or right. This is used as a backup functionality for the Kinect.

## Issues 
### Occurred and Solved 
* **Left/Right Hand** Normally you should wear the Myo on your "strong" arm, but for movements and simple gestures it doesn't make much different. Also the Myo works rather good on the "weak" arm as well. So no need to switch from right to left or the other way round.  
* **Calibration**  The default profile will be used for all applications. So there is no need for any user calibrations.
* **Syn Gesture** The syn gesture changed in the latest version. To sync the Myo you have to hold the "wave out" - gesture for a few seconds.  

### Known and Accepted 
* **Warming Up** It can take up to 4 minutes to warm up after you put on the Myo,but gestures can also be recognized pretty good during warm up.
* **Gesture** If you use the Myo the first time it takes a while until you now "how" to perform a proper gesture ( how much force or rather what position is needed to trigger the gesture).


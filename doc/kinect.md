# Kinect DOCUMENTATION [ IN WORK  ]

## Basic Information

After testing various technologies like Myo, Leap Motion and other input devices, we wanted to test the compatibility of Kinect v2 with Unreal Engine. After a short research we found a nice plugin, which provides us the functionality needed for our project. But it only supports the Kinect v2.

### Installing Guide

Download the Kinect 4 Unreal plugin [here](http://www.opaquemultimedia.com/download-k4u). Once the download finished, extract the zip. Copy the plugin folder to your Unreal Engine Project /Plugins folder.

Example:
C:\public\yourproject\Plugins --> paste the plugin folder (Kinect4Unreal v1.1) here.

Once the Kinect 4 Unreal plugin is successfully added, it will be found in the plugins tab of UE4 under project settings.

### Integration into the Project

Once you've done with the installing, COPY: ExplorationCharacter assets + Mannequin folder in CONTENT into your project.
Set the Controller to ExplorationCharacter and you're done.

## Technology

### Kinect
[Order Kinect v2](http://www.microsoftstore.com/store/msusa/en_US/pdp/Kinect-for-Xbox-One/productID.307499400)

[Downloads - SDK for Kinect v2](http://www.microsoft.com/en-us/download/details.aspx?id=44561)

### Kinect 4 Unreal

[Kinect 4 Unreal](http://www.opaquemultimedia.com/kinect-4-unreal/#about-k4u)

[Kinect 4 Unreal - Plugin](http://www.opaquemultimedia.com/download-k4u)

[Kinect 4 Unreal - Documentation](http://www.opaquemultimedia.com/documentation-overview)

## Implementation
### Basic

The Kinect 4 Unreal Plugin already includes a whole bunch of functionality, like the recognition of joints [see here](http://static1.squarespace.com/static/5560483fe4b048b2365a129c/t/55617618e4b076ae612bbeb6/1432450590720/VitruvianManKinectImage.PNG?format=750w).

The 2 things, which were needed and created are the forward movement and the torso rotation.

Forward Movement: Whenever the delta (difference) between the 2 knees are higher than a specified amount of Unreal Units, the Character moves one step forward.

Torso Rotation: Whenever the specified gesture is done, the Torso will allow rotation towards a specific direction. To activate that gesture you have to hold your 2 hands (wrists) nearby and then "point" them towards the direction you want to rotate to.

## Issues
Can't rebuild or create new source files after integrating the K4U plugin.
possible fix:

- Rename your Plugins folder to something like Plugins_;
- Open Unreal. It'll complain about K4U classes etc missing. Don't worry. Just don't edit any K4U dependant BP or you'll have to re-create the K4U nodes again later;
- Refresh the VS project and work your C++ out. At this point, I like to close Unreal and test my project pressing F5 whenever I make some change. I don't trust the Hot Reload feature as it breaks when you change BP nodes.
- When you're finished with C++, or need to test it together with the plugin, Ctrl+Shift+B to build the project then rename Plugins_ folder to it's original name and launch UE.

Just remember to always change the name of the Plugins folder to something else when you need to recompile your code, so UE doesn't try to compile the plugin too. That has to be when the Engine is closed, as it blocks the Plugins folder when it's open. Hence my F5 way of testing the code. It automatically launches and closes Unreal from VS. Another thing I recommend is when using F5 to launch and test the code in UE4, close UE4 using the software's X button after testing, not the stop button in VS. So Unreal saves any changes you've made to BPs etc.

source: [link](https://forums.unrealengine.com/showthread.php?52749-Kinect-4-Unreal/page5)

### Occurred and Solved 
We integrate the K4U-Plugin as the last feature, so the project should be ready to release at that state.

### Known and Accepted 
The K4U-Plugin doesn't support UE 4.10 currently.

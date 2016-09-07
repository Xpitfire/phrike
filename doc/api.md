# Screen Recorder

## API Documentation

### Description
The Screen Recorder uses FFMPEG to capture video and audio files. The recorder creates two video files that capture the monitor and the webcam. Currently an mkv - container is created as output. Here is a link to the FFMPEG documentation for further information: [FFMPEG Documentation](https://trac.ffmpeg.org/wiki/Capture/Desktop).
***
### Interface
The Screen Recorder is implemented as a Singleton. Therefore you only need to get the instance of it and can use it without creating an actual object.

```csharp
	/**
	 * Returns the Singleton instance of the Screen Recorder.
	 * There can be only one instance of this class.
	 */
	static ScreenRecorder* getInstance();

	/**
	 * Destructor
	 */
	~ScreenRecorder();

	/**
	 * Starts the recording of the game window (incuding audio) and of the webcam.
	 * @param directory: specifies the directory in which the files will be stored
	 * @param gameFilename: specifies the filename of the game video (inlcuding extension)
	 * @param cameraFilename: specifies the filename of the webcam video (including extension)
         */
	void startRecording(std::string directory = "C:\\tmp\\", std::string gameFilename = "game.mkv", std::string cameraFilename = "camera.mkv");

	/**
	 * Stops the recording of the game window and of the webcam.
	 */
	void stopRecording();

	/**
	 * Starts the recording of the game window (including audio)
	 * @param directory: specifies the directory in which the file will be stored
	 * @param gameFilename: specifies the filename of the game video (inlcuding extension)
	 */
	void startGameRecording(std::string directory = "C:\\tmp\\", std::string filename = "game.mkv");

	/**
	 * Stops the recording of the game window.
	 */
	void stopGameRecording();

	/**
	 * Starts the recording of the webcam.
	 * @param directory: specifies the directory in which the file will be stored
	 * @param gameFilename: specifies the filename of the webcam video (inlcuding extension)
	 */
	void startCameraRecording(std::string directory = "C:\\tmp\\", std::string filename = "camera.mkv");

	/**
	 * Stops the recording of the webcam.
	 */
	void stopCameraRecording();

	/**
	 * Load config from config file.
	 * @param filename: specifies the filename where the config is located
	 */
	void loadConfig(const std::string filename = "C:\\tmp\\config.txt");

	/**
	 * Set the videoConfig property
	 * @param config: a string with the configuration for the video capturing
	 */
	void setVideoConfig(std::string config);

	/**
	 * Set the cameraConfig property
	 * @param config: a string with the configuration for the camera capturing
	 */
	void setCameraConfig(std::string config);

	/**
	 * Get the videoConfig property
	 */
	std::string getVideoConfig();

	/**
	 * Get the cameraConfig property
	 */
	std::string getCameraConfig();
``` 
***
### Example
The Screen Recorder consists of "ScreenRecorder.h" and "ScreenRecorder.cpp". You need to add both files to your project in order to use the class.

To get a new instance of the Screen Recorder use the following command:
```csharp
	ScreenRecorder *recorder = ScreenRecorder::getInstance();
``` 
<br>
Before starting the recording you need to load the config file:
```csharp
	recorder->loadConfig("C:\\tmp\\config.txt");
```
<br>
You can also set the config directly in the code (which probably isn't the best way to do it):
```csharp
	recorder->setVideoConfig("video=\"screen-capture-recorder\":audio=\"virtual-audio-capturer\"");
	recorder->setCameraConfig("video=\"Integrated Camera\"");
```
> If you use the following command in the command line you will get all available recording devices: ``` ffmpeg -list_devices true -f dshow -i dummy ```

<br>
After getting the instance you can start the recording:
```csharp
	recorder->startRecording();
```
<br>
You can also add your own parameters:
```csharp
	recorder->startRecording("C:\\tmp\\", "test.mkv", "cam.mkv");
```

<br>
If you only want to record either the game window or the webcam, you can start the recording separately:
```csharp
	recorder->startCameraRecording();
	recorder->startGameRecording();
```
>You can also use the parameters defined in the Interface Section.

<br>
To stop the recording simply use the following command:
```csharp
	recorder->stopRecording();
```
>This stops both the game and webcam recording

<br>
If you want to stop a specific recording you can call one of the following methods:
```csharp
	recorder->stopGameRecording();
	recorder->stopCameraRecording();
```
<br>
You can find an example project here: [ScreenRecording](https://gitlab.com/OperationPhrike/phrike/tree/master/Phrike/ScreenRecording)
>These are the sources of an Unreal Engine project, so you have to add the starter Content. The easiest way to do this, is to create a new "C++ First Person" project with the name "PhrikeScreenCapture" in UE4 and replace all the files in the source folder with the content of the folder above.
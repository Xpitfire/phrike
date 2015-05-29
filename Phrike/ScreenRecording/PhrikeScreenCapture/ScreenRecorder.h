// Fill out your copyright notice in the Description page of Project Settings.

#pragma once
#include "AllowWindowsPlatformTypes.h"
#include "shellapi.h"
#include <string>
#include "HideWindowsPlatformTypes.h"

class PHRIKESCREENCAPTURE_API ScreenRecorder
{
public:
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
	void startRecording(std::wstring directory = L"C:\\tmp\\", std::wstring gameFilename = L"game.mkv", std::wstring cameraFilename = L"camera.mkv");

	/**
	 * Stops the recording of the game window and of the webcam.
	 */
	void stopRecording();

	/**
	 * Starts the recording of the game window (including audio)
	 * @param directory: specifies the directory in which the file will be stored
	 * @param gameFilename: specifies the filename of the game video (inlcuding extension)
	 */
	void startGameRecording(std::wstring directory = L"C:\\tmp\\", std::wstring filename = L"game.mkv");

	/**
	 * Stops the recording of the game window.
	 */
	void stopGameRecording();

	/**
	* Starts the recording of the webcam.
	* @param directory: specifies the directory in which the file will be stored
	* @param gameFilename: specifies the filename of the webcam video (inlcuding extension)
	*/
	void startCameraRecording(std::wstring directory = L"C:\\tmp\\", std::wstring filename = L"camera.mkv");

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

private:
	ScreenRecorder();
	SHELLEXECUTEINFO game;
	SHELLEXECUTEINFO camera;
	std::string cameraConfig;
	std::string videoConfig;

	static ScreenRecorder *recorder;

	bool checkIfFileExists(const std::string filename);
	std::wstring checkFilename(std::wstring directory, std::wstring filename);

};
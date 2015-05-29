// Fill out your copyright notice in the Description page of Project Settings.

#include "PhrikeScreenCapture.h"
#include "ScreenRecorder.h"
#include <iostream>
#include <fstream>
#include <sys/stat.h>

ScreenRecorder* ScreenRecorder::recorder = NULL;

ScreenRecorder::ScreenRecorder()
{
	// Private singleton constructor
	loadConfig("C:\\tmp\\config.txt");
}

ScreenRecorder::~ScreenRecorder()
{

}

// Get singleton instance
ScreenRecorder* ScreenRecorder::getInstance() 
{
	if (recorder == NULL)
	{
		//Create new instance if not available
		recorder = new ScreenRecorder();
	}
	return recorder;
}

// Start recording of game and camera
void ScreenRecorder::startRecording(std::wstring directory, std::wstring gameFilename, std::wstring cameraFilename)
{
	startGameRecording(directory, gameFilename);
	startCameraRecording(directory, cameraFilename);
}

// Start recording of game
void ScreenRecorder::startGameRecording(std::wstring directory, std::wstring filename)
{
	// Check if the filename is available
	filename = checkFilename(directory, filename);

	// Set recording parameters: capture audio and video
	std::string config = "-f dshow -i " + videoConfig + " ";
	std::wstring params(config.begin(), config.end());
	params = params + directory + filename;

	std::cout << "Video Params: " << params.c_str() << std::endl;

	// Set ShellExecute parameter
	game.cbSize = sizeof(SHELLEXECUTEINFO);
	game.fMask = SEE_MASK_NOCLOSEPROCESS;
	game.hwnd = NULL;
	game.lpVerb = L"open";
	game.lpFile = L"ffmpeg.exe";
	game.lpParameters = params.c_str();
	game.lpDirectory = NULL;
	game.nShow = SW_HIDE;
	game.hInstApp = NULL;

	// Start game recording via commandline
	ShellExecuteEx(&game);
}

// Start recording of camera
void ScreenRecorder::startCameraRecording(std::wstring directory, std::wstring filename)
{
	// Check if the filename is available
	filename = checkFilename(directory, filename);

	// Set recording parameters: capture webcam video
	std::string config = "-f dshow -i " + cameraConfig + " ";
	std::wstring params(config.begin(), config.end());
	params = params + directory + filename;
	
	// Set ShellExecute parameter
	camera.cbSize = sizeof(SHELLEXECUTEINFO);
	camera.fMask = SEE_MASK_NOCLOSEPROCESS;
	camera.hwnd = NULL;
	camera.lpVerb = L"open";
	camera.lpFile = L"ffmpeg.exe";
	camera.lpParameters = params.c_str();
	camera.lpDirectory = NULL;
	camera.nShow = SW_HIDE;
	camera.hInstApp = NULL;

	// Start camera recording via commandline
	ShellExecuteEx(&camera);
}

// Stop recording of game and camera
void ScreenRecorder::stopRecording()
{
	stopGameRecording();
	stopCameraRecording();
}

// Stop recording of game
void ScreenRecorder::stopGameRecording()
{
	TerminateProcess(game.hProcess, 1);
}

// Stop recording of camera
void ScreenRecorder::stopCameraRecording()
{
	TerminateProcess(camera.hProcess, 1);
}

// Check if the filename is still available or a new one is needed. Returns a filename that is available
std::wstring ScreenRecorder::checkFilename(std::wstring directory, std::wstring filename)
{
	bool exists = true;
	while(exists)
	{
		std::wstring path = directory + filename;
		std::string file(path.begin(), path.end());
		if (checkIfFileExists(file))
		{
			filename = L"test" + filename;
		}
		else
		{
			exists = false;
		}
	}
	return filename;
}

// Check if the file already exists on the filesystem
bool ScreenRecorder::checkIfFileExists(const std::string filename)
{
	struct stat buffer;
	return (stat(filename.c_str(), &buffer) == 0);
}

// Load config from config file
void ScreenRecorder::loadConfig(const std::string filename)
{
	// Open config file
	std::ifstream infile(filename);
	if(infile.good())
	{
		std::string line;

		// Read video config
		std::getline(infile, line);
		setVideoConfig(line);

		// Read camera config
		std::getline(infile, line);
		setCameraConfig(line);
	}
}

// Set video config
void ScreenRecorder::setVideoConfig(std::string config)
{
	videoConfig = config;
}

// Set camera config
void ScreenRecorder::setCameraConfig(std::string config)
{
	cameraConfig = config;
}

// Get video config
std::string ScreenRecorder::getVideoConfig()
{
	return videoConfig;
}

// Get camera config
std::string ScreenRecorder::getCameraConfig()
{
	return cameraConfig;
}
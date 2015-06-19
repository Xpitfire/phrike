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
	isRunningGame = false;
	isRunningCamera = false;
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
void ScreenRecorder::startRecording(std::string directory, std::string gameFilename, std::string cameraFilename)
{
	startGameRecording(directory, gameFilename);
	startCameraRecording(directory, cameraFilename);
}

// Start recording of game
void ScreenRecorder::startGameRecording(std::string directory, std::string filename)
{
	if (isRunningGame)
	{
		return;
	}

	// Check if the filename is available
	filename = checkFilename(directory, filename);

	// Set recording parameters: capture audio and video
	std::string command = "ffmpeg.exe -framerate " + framesPerSecond + " -f dshow -i " + videoConfig + " \"" + directory + filename + "\"";	

	STARTUPINFO gameStartupInfo = {};
	gameStartupInfo.cb = sizeof gameStartupInfo;
	gameProcessInfo = {};
	std::wstring commandLine(command.begin(), command.end());
	LPTSTR szCmdline = _tcsdup(commandLine.c_str());

	if (CreateProcess(NULL, szCmdline, 0, false, 0, CREATE_NO_WINDOW, 0, 0, &gameStartupInfo, &gameProcessInfo))
	{
		isRunningGame = true;
	}
}

// Start recording of camera
void ScreenRecorder::startCameraRecording(std::string directory, std::string filename)
{
	if (isRunningCamera)
	{
		return;
	}

	// Check if the filename is available
	filename = checkFilename(directory, filename);

	// Set recording parameters: capture audio and video
	std::string command = "ffmpeg.exe -framerate " + framesPerSecond + " -f dshow -i " + cameraConfig + " \"" + directory + filename + "\"";
	
	STARTUPINFO cameraStartupInfo = {};
	cameraStartupInfo.cb = sizeof cameraStartupInfo;
	cameraProcessInfo = {};
	std::wstring commandLine(command.begin(), command.end());
	LPTSTR szCmdline = _tcsdup(commandLine.c_str());

	if (CreateProcess(NULL, szCmdline, 0, false, 0, CREATE_NO_WINDOW, 0, 0, &cameraStartupInfo, &cameraProcessInfo))
	{
		isRunningCamera = true;
	}
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
	if (isRunningGame && gameProcessInfo.dwProcessId != NULL)
	{
		stopProcess(gameProcessInfo);
		isRunningGame = false;
	}
}

// Stop recording of camera
void ScreenRecorder::stopCameraRecording()
{
	if (isRunningCamera && cameraProcessInfo.dwProcessId != NULL)
	{
		stopProcess(cameraProcessInfo);
		isRunningCamera = false;
	}
}

// Check if the filename is still available or a new one is needed. Returns a filename that is available
std::string ScreenRecorder::checkFilename(std::string directory, std::string filename)
{
	bool exists = true;
	while(exists)
	{
		std::string path = directory + filename;
		if (checkIfFileExists(path))
		{
			filename = "test" + filename;
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
		setFPS(line);

		// Read video config
		std::getline(infile, line);
		setVideoConfig(line);

		// Read camera config
		std::getline(infile, line);
		setCameraConfig(line);
	}
}

// Set framesPerSecond config
void ScreenRecorder::setFPS(std::string framesPerSecond)
{
	this->framesPerSecond = framesPerSecond;
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

// Get framesPerSecond config
std::string ScreenRecorder::getFPS()
{
	return framesPerSecond;
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

// Stop the process using Ctrl+C signal
void ScreenRecorder::stopProcess(PROCESS_INFORMATION pi)
{
	std::wstring param = std::to_wstring(pi.dwProcessId);
	SHELLEXECUTEINFO process;

	// Set ShellExecute parameter
	process.cbSize = sizeof(SHELLEXECUTEINFO);
	process.fMask = SEE_MASK_NOCLOSEPROCESS;
	process.hwnd = NULL;
	process.lpVerb = L"open";
	process.lpFile = L"SendSignalCtrlC.exe";
	process.lpParameters = param.c_str();
	process.lpDirectory = NULL;
	process.nShow = SW_HIDE;
	process.hInstApp = NULL;

	// Stop recording via commandline
	ShellExecuteEx(&process);
}
// Fill out your copyright notice in the Description page of Project Settings.

#pragma once
#include "AllowWindowsPlatformTypes.h"
#include "shellapi.h"
#include <string>
#include "HideWindowsPlatformTypes.h"

class PHRIKESCREENCAPTURE_API ScreenRecorder
{
public:
	static ScreenRecorder* getInstance();
	~ScreenRecorder();

	void startRecording(std::wstring directory = L"C:\\tmp\\", std::wstring gameFilename = L"game.mkv", std::wstring cameraFilename = L"camera.mkv");
	void stopRecording();

	void startGameRecording(std::wstring directory = L"C:\\tmp\\", std::wstring filename = L"game.mkv");
	void stopGameRecording();

	void startCameraRecording(std::wstring directory = L"C:\\tmp\\", std::wstring filename = L"camera.mkv");
	void stopCameraRecording();

private:
	ScreenRecorder();
	SHELLEXECUTEINFO game;
	SHELLEXECUTEINFO camera;
	static ScreenRecorder *recorder;

	bool checkIfFileExists(const std::string filename);
	std::wstring checkFilename(std::wstring directory, std::wstring filename);

};
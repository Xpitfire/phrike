// Copyright 1998-2015 Epic Games, Inc. All Rights Reserved.

#include "PhrikeScreenCapture.h"
#include "PhrikeScreenCaptureGameMode.h"
#include "PhrikeScreenCaptureHUD.h"
#include "PhrikeScreenCaptureCharacter.h"
#include "ScreenRecorder.h"

APhrikeScreenCaptureGameMode::APhrikeScreenCaptureGameMode(const FObjectInitializer& ObjectInitializer)
	: Super(ObjectInitializer)
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPerson/Blueprints/FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

	// use our custom HUD class
	HUDClass = APhrikeScreenCaptureHUD::StaticClass();

}

//Mosaic: https://trac.ffmpeg.org/wiki/Create%20a%20mosaic%20out%20of%20several%20input%20videos
void APhrikeScreenCaptureGameMode::BeginPlay() 
{
	/*game.cbSize = sizeof(SHELLEXECUTEINFO);
	game.fMask = SEE_MASK_NOCLOSEPROCESS;
	game.hwnd = NULL;
	game.lpVerb = L"open";
	game.lpFile = L"ffmpeg.exe";
	game.lpParameters = L"-f dshow -i video=\"screen-capture-recorder\":audio=\"virtual-audio-capturer\" C:\\tmp\\game.mkv";
	game.lpDirectory = NULL;
	game.nShow = SW_HIDE;
	game.hInstApp = NULL;

	camera.cbSize = sizeof(SHELLEXECUTEINFO);
	camera.fMask = SEE_MASK_NOCLOSEPROCESS;
	camera.hwnd = NULL;
	camera.lpVerb = L"open";
	camera.lpFile = L"ffmpeg.exe";
	camera.lpParameters = L"-f dshow -i video=\"Integrated Camera\" C:\\tmp\\camera.mkv";
	camera.lpDirectory = NULL;
	camera.nShow = SW_HIDE;
	camera.hInstApp = NULL;

	ShellExecuteEx(&game); // start game process
	ShellExecuteEx(&camera); // start camera process*/
	ScreenRecorder *recorder = ScreenRecorder::getInstance();
	recorder->startRecording();
}

void APhrikeScreenCaptureGameMode::EndPlay(EEndPlayReason::Type reason)
{
	/*TerminateProcess(game.hProcess, 1);
	TerminateProcess(camera.hProcess, 1);*/
	ScreenRecorder *recorder = ScreenRecorder::getInstance();
	recorder->stopRecording();
}

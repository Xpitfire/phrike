// Copyright 1998-2015 Epic Games, Inc. All Rights Reserved.
#pragma once

#include "AllowWindowsPlatformTypes.h"
#include "shellapi.h"
#include "HideWindowsPlatformTypes.h"

#include "GameFramework/GameMode.h"
#include "PhrikeScreenCaptureGameMode.generated.h"

UCLASS(minimalapi)
class APhrikeScreenCaptureGameMode : public AGameMode
{
	GENERATED_BODY()

public:
	APhrikeScreenCaptureGameMode(const FObjectInitializer& ObjectInitializer);
	void BeginPlay() override;
	void EndPlay(EEndPlayReason::Type reason) override;

/*private:
	SHELLEXECUTEINFO game;
	SHELLEXECUTEINFO camera;*/
};




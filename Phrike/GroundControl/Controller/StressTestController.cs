﻿using NLog;
using Phrike.GroundControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phrike.GroundControl.Controller
{
    public class StressTestController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private StressTestViewModel stressTestViewModel;
        private UnrealEngineController unrealEngineModel;
        private SensorsController sensorsModel;

        public StressTestController()
        {
            stressTestViewModel = StressTestViewModel.Instance;
        }

        public void StartStressTest(SubjectVM subject, ScenarioVM scenario)
        {
            StartUnrealEngineTask();
            StartSensorsTask();
            StartScreenCaptureTask();
        }

        public void StopStressTest()
        {
            StopUnrealEngineTask();
            StopSensorsTask();
            StopScreenCaptureTask();
        }

        private Task StartUnrealEngineTask()
        {
            return Task.Run(() =>
            {
                // start the external application sub-process
                ProcessController.StartProcess(UnrealEngineController.UnrealEnginePath, true, new string[] { "-fullscreen" });
                Logger.Info("Unreal Engine process started!");
                // create the Unreal Engine communication object
                unrealEngineModel = new UnrealEngineController(ShowStressTestError, DisableUnrealEngineAndScreenCapturingColor);
                Logger.Info("Unreal Engine is ready to use!");
                stressTestViewModel.UnrealStatusColor = GCColors.Active;
            });
        }

        public Task StopUnrealEngineTask()
        {
            return Task.Run(() =>
            {
                if (unrealEngineModel == null)
                {
                    const string message = "Could not stop the Unreal Engine! No Unreal Engine instance active.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }

                unrealEngineModel.Close();
                unrealEngineModel = null;
                ProcessController.StopProcess(UnrealEngineController.UnrealEnginePath);
                Logger.Info("Unreal Engine process stoped!");
                DisableUnrealEngineAndScreenCapturingColor();
            });
        }

        public Task StartSensorsTask()
        {
            return Task.Run(() =>
            {
                if (sensorsModel != null)
                {
                    const string message = "Could not start sensors recording! Recording task is already running.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }

                sensorsModel = new SensorsController(ShowStressTestError);
                Logger.Info("Sensors instance created!");

                var active = sensorsModel.StartRecording();
                if (!active)
                {
                    sensorsModel = null;
                }
                else
                {
                    stressTestViewModel.SensorStatusColor = GCColors.Active;
                }
            });
        }

        public Task StopSensorsTask()
        {
            return Task.Run(() =>
            {
                if (sensorsModel == null)
                {
                    const string message = "Could not stop sensors recording! No sensors recording instance enabled.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }
                sensorsModel.Close();
                sensorsModel = null;
                Logger.Info("Sensors recording successfully stopped!");
                stressTestViewModel.SensorStatusColor = GCColors.Disabled;
            });
        }

        public Task StartScreenCaptureTask()
        {
            return Task.Run(() =>
            {
                if (unrealEngineModel == null)
                {
                    const string message = "Could not start screen recording! Recording task is already running.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }

                unrealEngineModel.StartCapture();
                Logger.Info("Screen Capture successfully started!");
                stressTestViewModel.ScreenCapturingStatusColor = GCColors.Active;
            });
        }

        public Task StopScreenCaptureTask()
        {
            return Task.Run(() =>
            {
                if (unrealEngineModel == null)
                {
                    const string message = "Could not stop screen recording! No recording running.";
                    Logger.Warn(message);
                    ShowStressTestError(message);
                    return;
                }

                unrealEngineModel.StopCapture();
                Logger.Info("Screen Capture successfully stopped!");
                stressTestViewModel.ScreenCapturingStatusColor = GCColors.Disabled;
            });
        }

        public void ApplicationCloseTask()
        {
            StopSensorsTask();
            StopScreenCaptureTask();
            StopUnrealEngineTask();
        }

        #region Callbacks

        internal void DisableUnrealEngineAndScreenCapturingColor()
        {
            stressTestViewModel.UnrealStatusColor = GCColors.Disabled;
            stressTestViewModel.ScreenCapturingStatusColor = GCColors.Disabled;
        }

        private void ShowStressTestError(string message)
        {
            MainViewModel.Instance.ShowDialogMessage("Stress Test Error", message);
        }

        #endregion
    }
}

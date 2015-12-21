using DataAccess;
using DataModel;
using NLog;
using Phrike.GroundControl.Helper;
using Phrike.GroundControl.ViewModels;
using System;
using System.Threading.Tasks;

namespace Phrike.GroundControl.Controller
{
    public class StressTestController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private SubjectVM tempSubject;
        private ScenarioVM tempScenario;

        // View to display the status
        private StressTestViewModel stressTestViewModel;

        private UnrealEngineController unrealEngineController;
        private SensorsController sensorsController;

        private UnitOfWork unitOfWork;

        private Test test;

        public StressTestController()
        {
            stressTestViewModel = StressTestViewModel.Instance;

            // create the Unreal Engine communication object
            unrealEngineController = new UnrealEngineController();
            unrealEngineController.PositionReceived += (s, e) => 
            {
                test.PositionData.Add(e);
            };
            unrealEngineController.Ending += (s, e) =>
            {
                StopStressTest();
                unitOfWork.Save();
            }
            unrealEngineController.Ending += (sender, args) => DisableUnrealEngineAndScreenCapturingColor();
            unrealEngineController.Restarting += (s, e) =>
            {
                StopStressTest();
                StartStressTest(tempSubject, tempScenario);
            };
            unrealEngineController.ErrorOccoured += (s, e) =>
            {
                StopStressTest();
                DialogHelper.ShowErrorDialog("Fehler in der Simulation aufgetreten.");
                Logger.Error(e);
            };

        }

        public void StartStressTest(SubjectVM subject, ScenarioVM scenario)
        {
            tempSubject = subject;
            tempScenario = scenario;

            if (subject == null || scenario == null)
            {
                return;
            }
            using (unitOfWork = new UnitOfWork())
            {
                test = new Test()
                {
                    Subject = unitOfWork.SubjectRepository.GetByID(subject.Id),
                    Scenario = unitOfWork.ScenarioRepository.GetByID(scenario.Id),
                    Time = DateTime.Now,
                    Title = "Testrun - " + subject.FullName + " " + DateTime.Now,
                    Location = "Test"
                };
                unitOfWork.TestRepository.Insert(test);
                unitOfWork.Save();
                StartUnrealEngineTask();
                if (Settings.SelectedSensorType == Models.SensorType.GMobiLab)
                {
                    StartSensorsTask();
                }
                if (Settings.ScreenRecordingEnabled)
                {
                    StartScreenCaptureTask(test.Id);
                }
                if (Settings.WebcamRecordingEnabled)
                {
                    StartWebcamCaptureTask(test.Id);
                }
            }
        }


        public void StopStressTest()
        {
            StopUnrealEngineTask();
            if (Settings.SelectedSensorType == Models.SensorType.GMobiLab)
            {
                StopSensorsTask();
            }
            if (Settings.ScreenRecordingEnabled)
            {
                StopScreenCaptureTask();
            }
            if (Settings.WebcamRecordingEnabled)
            {
                StopWebcamCaptureTask();
            }
        }

        private Task StartUnrealEngineTask()
        {
            return Task.Run(() =>
            {
                // start the external application sub-process
                ProcessController.StartProcess(UnrealEngineController.UnrealEnginePath, true, new string[] { "-fullscreen" });
                Logger.Info("Unreal Engine process started!");

                stressTestViewModel.UnrealStatusColor = GCColors.Active;
            });
        }

        public Task StopUnrealEngineTask()
        {
            return Task.Run(() =>
            {
                if (unrealEngineController == null)
                {
                    const string message = "Could not stop the Unreal Engine! No Unreal Engine instance active.";
                    Logger.Warn(message);
                    return;
                }

                unrealEngineController.Close();
                unrealEngineController = null;
                ProcessController.StopProcess(UnrealEngineController.UnrealEnginePath);
                Logger.Info("Unreal Engine process stoped!");
                DisableUnrealEngineAndScreenCapturingColor();
            });
        }

        public Task StartSensorsTask()
        {
            return Task.Run(() =>
            {
                if (sensorsController != null)
                {
                    const string message = "Could not start sensors recording! Recording task is already running.";
                    Logger.Warn(message);
                    return;
                }

                sensorsController = new SensorsController();
                Logger.Info("Sensors instance created!");

                var active = sensorsController.StartRecording();
                if (!active)
                {
                    sensorsController = null;
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
                if (sensorsController == null)
                {
                    const string message = "Could not stop sensors recording! No sensors recording instance enabled.";
                    Logger.Warn(message);
                    return;
                }
                sensorsController.Close();
                sensorsController = null;
                Logger.Info("Sensors recording successfully stopped!");
                stressTestViewModel.SensorStatusColor = GCColors.Disabled;
            });
        }

        public Task StartScreenCaptureTask(int testId)
        {
            return Task.Run(() =>
            {
                ScreenCaptureHelper screenCapture = ScreenCaptureHelper.GetInstance();
                screenCapture.StartGameRecording(testId);
                if (!screenCapture.IsRunningGame)
                {
                    const string message = "Could not start screen recording!";
                    Logger.Warn(message);
                    return;
                }
                Logger.Info("Screen Capture successfully started!");
                stressTestViewModel.ScreenCapturingStatusColor = GCColors.Active;
            });
        }

        public Task StopScreenCaptureTask()
        {
            return Task.Run(() =>
            {
                ScreenCaptureHelper screenCapture = ScreenCaptureHelper.GetInstance();
                screenCapture.StopGameRecording();
                if (screenCapture.IsRunningGame)
                {
                    const string message = "Could not stop screen recording!";
                    Logger.Warn(message);
                    return;
                }
                Logger.Info("Screen Capture successfully stopped!");
                stressTestViewModel.ScreenCapturingStatusColor = GCColors.Disabled;
            });
        }

        public Task StartWebcamCaptureTask(int testId)
        {
            return Task.Run(() =>
            {
                ScreenCaptureHelper screenCapture = ScreenCaptureHelper.GetInstance();
                screenCapture.StartCameraRecording(testId);
                if (!screenCapture.IsRunningCamera)
                {
                    const string message = "Could not start screen recording!";
                    Logger.Warn(message);
                    return;
                }
                Logger.Info("Screen Capture successfully started!");
                stressTestViewModel.WebcamCapturingStatusColor = GCColors.Active;
            });
        }

        public Task StopWebcamCaptureTask()
        {
            return Task.Run(() =>
            {
                ScreenCaptureHelper screenCapture = ScreenCaptureHelper.GetInstance();
                screenCapture.StopCameraRecording();
                if (screenCapture.IsRunningCamera)
                {
                    const string message = "Could not stop screen recording!";
                    Logger.Warn(message);
                    return;
                }
                Logger.Info("Screen Capture successfully stopped!");
                stressTestViewModel.WebcamCapturingStatusColor = GCColors.Disabled;
            });
        }


        public void ApplicationCloseTask()
        {
            StopStressTest();
        }

        #region Callbacks

        internal void DisableUnrealEngineAndScreenCapturingColor()
        {
            stressTestViewModel.UnrealStatusColor = GCColors.Disabled;
            stressTestViewModel.ScreenCapturingStatusColor = GCColors.Disabled;
            stressTestViewModel.WebcamCapturingStatusColor = GCColors.Disabled;
        }

        #endregion
    }
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Media.Media3D;
using NLog;
using Phrike.GroundControl.ViewModels;
using Phrike.PhrikeSocket;

namespace Phrike.GroundControl.Model
{
    class UnrealEngineModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Execution path of the Unreal Engine.
        /// </summary>
        public const string UnrealEnginePath = @"/UnrealData/Balance.exe";

        public const int UnrealEngineSocketPort = 5678;

        // The socket for the Unreal Engine command communication
        private Socket socket;
        private SocketWriter unrealSocketWriter;
        private SocketReader unrealSocketReader;

        private UnrealEngineModel unrealEngineModel;

        /// <summary>
        /// Is alive flag for the socket communication thread.
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// Create a new Unreal Engine instance and connect to the socket.
        /// </summary>
        public UnrealEngineModel()
        {
            try
            {
                TcpListener socketListener = new TcpListener(IPAddress.Any, UnrealEngineSocketPort);
                socketListener.Start();
                Logger.Info("Unreal Engine socket connection established on port {0} and waiting for connections...",
                    UnrealEngineSocketPort);
                socket = socketListener.AcceptSocket();
                unrealSocketWriter = new SocketWriter(socket);
                unrealSocketReader = new SocketReader(socket);
                // run command listener thread
                Thread trackingThread = new Thread(new ThreadStart(Run));
                trackingThread.Start();
                Logger.Info("Listener socket thread initialized.");
            }
            catch (Exception e)
            {
                const string message = "Could not initialize Unreal Engine socket instance.";
                Logger.Error(message, e);
                ShowUnrealEngineError(message);
            }
        }

        /// <summary>
        /// Close Unreal Engine communication instance.
        /// </summary>
        public void Close()
        {
            try
            {
                // stop running screen capturing task
                StopCapture();
                // send close command
                unrealSocketWriter.WriteString("end");
                unrealSocketWriter.Send();
            }
            catch (Exception e)
            {
                const string message = "Could not send Unreal Engine exit command!";
                Logger.Error(message, e);
            }
            finally
            {
                // fix for too fast socket close 
                Thread.Sleep(2000);
                try
                {
                    unrealSocketReader.Receive(1000);
                    string readString;
                    if ((readString = unrealSocketReader.ReadString()) == "end")
                    {
                        if (socket != null)
                            socket.Close();
                        Logger.Info("Successfully closed socket connection!");
                    }
                    else
                    {
                        Logger.Error("Expected 'end' from Unreal Engine received '" + readString + "'. Killing connection!");
                        if (socket != null)
                            socket.Close();
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn("unrealSocketReader connection lost.");
                }
            }
        }

        /// <summary>
        /// Start a screen capturing instance.
        /// </summary>
        public void StartCapture()
        {
            try
            {
                unrealSocketWriter.WriteString("start_capture");
                unrealSocketWriter.Send();
            }
            catch (Exception e)
            {
                const string message = "Could not send Unreal Engine start capture command!";
                Logger.Error(message, e);
                ShowUnrealEngineError(message);
            }
        }
        /// <summary>
        /// Stop a screen capturing instance.
        /// </summary>
        public void StopCapture()
        {
            try
            {
                unrealSocketWriter.WriteString("stop_capture");
                unrealSocketWriter.Send();
            }
            catch (Exception e)
            {
                const string message = "Could not send Unreal Engine stop capture command!";
                Logger.Error(message, e);
                ShowUnrealEngineError(message);
            }
        }

        /// <summary>
        /// Initialize position tracking interval / refresh rate and
        /// start position and angle transmition.
        /// </summary>
        private void InitPositionTracking()
        {
            try
            {
                unrealSocketWriter.WriteString("pos");
                unrealSocketWriter.WriteFloat(0.5f);
                unrealSocketWriter.WriteString("agl");
                unrealSocketWriter.WriteFloat(0.5f);
                unrealSocketWriter.Send();
                Logger.Info("Unreal Engine successfully initialized position tracking!");
            }
            catch (Exception e)
            {
                const string message = "Could not send Unreal Engine position tracking initalization command!";
                Logger.Error(message, e);
                ShowUnrealEngineError(message);
            }
        }

        /// <summary>
        /// Unreal Engine Socket command receive thread.
        /// </summary>
        public void Run()
        {
            InitPositionTracking();
            Logger.Info("Unreal Engine listener socket thread active!");
            while (IsAlive)
            {
                unrealSocketReader.Receive();

                String cmd = unrealSocketReader.ReadString();
                Logger.Info("Received command with Length: {0} - {1}", cmd.Length, cmd);

                Vector3D pos = default(Vector3D);

                switch (cmd.ToLower())
                {
                    case "pos":
                    case "agl":
                        float x = unrealSocketReader.ReadFloat();
                        float y = unrealSocketReader.ReadFloat();
                        float z = unrealSocketReader.ReadFloat();

                        pos = new Vector3D() { X = x, Y = y, Z = z };
                        Logger.Debug("Received position: {0}", pos);
                        break;

                    case "end":
                        IsAlive = false;
                        break;

                    default:
                        Logger.Error("Unkown Command: {0}", cmd);
                        break;
                }

                Logger.Debug("Received command: {0}", cmd);
            }
        }

        /// <summary>
        /// Show a default Unreal Engine error message to the UI.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        private void ShowUnrealEngineError(string message)
        {
            MainViewModel.Instance.ShowDialogMessage("Unreal Engine Error", message);
        }
    }
}

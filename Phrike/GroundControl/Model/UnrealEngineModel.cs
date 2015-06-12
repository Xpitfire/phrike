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

        public const int UnrealEngineSocketPort = 5678;

        private Socket socket;

        private SocketWriter unrealSocketWriter;
        private SocketReader unrealSocketReader;

        private UnrealEngineModel unrealEngineModel;
        private bool isAlive = true;

        public bool IsAlive
        {
            get { return isAlive; }
            private set { isAlive = value; }
        }

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

        public void Close()
        {
            try
            {
                StopCapture();
                unrealSocketWriter.WriteString("exit");
                unrealSocketWriter.Send();
            }
            catch (Exception e)
            {
                const string message = "Could not send Unreal Engine exit command!";
                Logger.Error(message, e);
            }
            finally
            {
                if (socket != null)
                    socket.Close();
                Logger.Info("Successfully closed socket connection!");
            }
        }

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
                        isAlive = false;
                        break;

                    default:
                        Logger.Error("Unkown Command: {0}", cmd);
                        break;
                }

                Logger.Debug("Received command: {0}", cmd);
            }
        }

        private void ShowUnrealEngineError(string message)
        {
            MainViewModel.Instance.ShowDialogMessage("Unreal Engine Error", message);
        }
    }
}

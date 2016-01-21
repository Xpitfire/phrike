using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using DataAccess;
using DataModel;
using NLog;
using Phrike.PhrikeSocket;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.Controller
{
    class UnrealEngineController
    {
        private class UnrealSocket
        {
            public Socket Socket { get; private set; }
            public SocketWriter UnrealSocketWriter { get; private set; }
            public SocketReader UnrealSocketReader { get; private set; }

            public UnrealSocket(Socket socket)
            {
                if (socket == null)
                    throw new ArgumentNullException(nameof(socket), "Socket mustn't be null");
                this.Socket = socket;
                UnrealSocketWriter = new SocketWriter(socket);
                UnrealSocketReader = new SocketReader(socket);
            }

            public void Close()
            {
                if (Socket != null && Socket.Connected)
                    Socket.Close();
                Socket = null;
            }

            public bool IsSocketConnected
            {
                get
                {
                    if (Socket == null)
                        return false;

                    // http://stackoverflow.com/a/2661876
                    bool part1 = Socket.Poll(1000, SelectMode.SelectRead);
                    bool part2 = (Socket.Available == 0);
                    if (part1 && part2)
                        return false;
                    else
                        return true;
                }
            }
        }

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string unrealEnginePath;
        /// <summary>
        /// Execution path of the Unreal Engine.
        /// </summary>
        public static string UnrealEnginePath
        {
            get
            {
                if (unrealEnginePath == null)
                {
                    using (var unitOfWork = new UnitOfWork())
                    {
                        // TODO: Szenario is fixed name!!!
                        var scenario = unitOfWork.ScenarioRepository.Get(
                            data => data.Name == "Balance").FirstOrDefault();
                        unrealEnginePath = System.IO.Path.Combine(PathHelper.PhrikeScenario, scenario.ExecutionPath);
                    }
                }
                return unrealEnginePath;
            }
            set { unrealEnginePath = value; }
        }

        public const int UnrealEngineSocketPort = 5678;

        // The socket for the Unreal Engine command communication
        //private Socket socket;
        private List<UnrealSocket> sockets;

        private Thread socketListenerThread;
        private readonly TcpListener socketListener;

        public event EventHandler Restarting;
        public event EventHandler Ending;
        public event EventHandler<PositionData> PositionReceived;
        public event EventHandler<Exception> ErrorOccoured;

        /// <summary>
        /// Is alive flag for the socket communication thread.
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// Create a new Unreal Engine instance and connect to the socket.
        /// </summary>
        public UnrealEngineController()
        {
            if (UnrealEnginePath == null)
            {
                throw new NotSupportedException("Could not find scenario data!");
            }

            this.sockets = new List<UnrealSocket>();

            IsAlive = true;
            socketListener = new TcpListener(IPAddress.Any, UnrealEngineSocketPort);

            socketListenerThread = new Thread(ListenForEngineConnect);
            socketListenerThread.IsBackground = true;
            socketListenerThread.Name = "TCP Socket Listener";
            socketListenerThread.Start();
        }

        private void ListenForEngineConnect()
        {
            // TODO Dispose socketListener
            // TPDP Correct message (no connection there at this point!)
            try
            {
                socketListener.Start();
                Logger.Info("Unreal Engine socket connection started on port {0} and waiting for connections...",
                    UnrealEngineSocketPort);
                while (IsAlive)
                {
                    try
                    {
                        Socket socket = socketListener.AcceptSocket();

                        UnrealSocket unrealSocket = new UnrealSocket(socket);
                        sockets.Add(unrealSocket);

                        // run command listener thread
                        Thread trackingThread = new Thread(() => Run(unrealSocket));
                        trackingThread.Name = "Unreal Engine Socket Listener";
                        trackingThread.IsBackground = true;
                        trackingThread.Start();
                        Logger.Info("Listener socket thread initialized.");
                    }
                    catch (Exception e)
                    {
                        SocketException socketException = e as SocketException;
                        if (socketException?.SocketErrorCode == SocketError.Interrupted)
                        {
                            // Everything alright
                            IsAlive = false;
                            return;
                        }

                        const string message = "Could not initialize Unreal Engine socket instance.";
                        Logger.Error(e, message);
                    }
                }
            }
            finally
            {
                socketListener.Stop();
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
                //StopCapture();
                // send close commands
                foreach (UnrealSocket socket in sockets)
                {
                    if (socket.IsSocketConnected)
                    {
                        socket.UnrealSocketWriter.WriteString("end");
                        socket.UnrealSocketWriter.Send();
                    }
                }
            }
            catch (Exception e)
            {
                const string message = "Could not send Unreal Engine exit command!";
                Logger.Error(e, message);
            }
            finally
            {
                // fix for too fast socket close 
                //Thread.Sleep(2000); // TODO Is this needed? We wait for end message anyway.
                try
                {
                    Parallel.ForEach(sockets, socket =>
                    {
                        if (!socket.IsSocketConnected)
                            return;

                        try
                        {
                            // Wait 1 sec for UE to update peacefully
                            socket.UnrealSocketReader.Receive(1000);
                            string readString;
                            if ((readString = socket.UnrealSocketReader.ReadString()) == "end")
                            {
                                socket.Close();
                                Logger.Info("Successfully closed socket connection!");
                            }
                            else
                            {
                                Logger.Error($"Expected 'end' from Unreal Engine received '{readString}'. Killing connection!");
                                socket.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            socket.Close();
                        }
                    });

                    socketListener.Stop();
                }
                catch (Exception e)
                {
                    Logger.Warn(e, "unrealSocketReader connection lost.");
                }
            }
        }

        /// <summary>
        /// Initialize position tracking interval / refresh rate and
        /// start position and angle transmition.
        /// </summary>
        private void InitPositionTracking(UnrealSocket socket)
        {
            SocketWriter unrealSocketWriter = socket.UnrealSocketWriter;
            try
            {
                unrealSocketWriter.WriteString("init");
                unrealSocketWriter.WriteFloat(0.5f);
                unrealSocketWriter.Send();
                Logger.Info("Unreal Engine successfully initialized position tracking!");
            }
            catch (Exception e)
            {
                const string message = "Could not send Unreal Engine position tracking initalization command!";
                //Logger.Error(message, e);
                Logger.Error(e, message);
            }
        }

        /// <summary>
        /// Unreal Engine Socket command receive thread.
        /// </summary>
        private void Run(UnrealSocket unrealSocket)
        {
            Socket socket = unrealSocket.Socket;
            SocketReader unrealSocketReader = unrealSocket.UnrealSocketReader;
            SocketWriter unrealSocketWriter = unrealSocket.UnrealSocketWriter;

            InitPositionTracking(unrealSocket);
            Logger.Info("Unreal Engine listener socket thread active!");
            bool keepRunning = true;
            bool exceptionOccured = false;
            while (IsAlive && keepRunning)
            {
                try
                {
                    unrealSocketReader.Receive();

                    while (unrealSocketReader.BytesAvailable > 0)
                    {
                        exceptionOccured = false;
                        try
                        {
                            string cmd = unrealSocketReader.ReadString();

                            Logger.Info("Received command with Length: {0} - {1}", cmd.Length, cmd);

                            switch (cmd.ToLower())
                            {
                                case "posagl":
                                    float x = unrealSocketReader.ReadFloat();
                                    float y = unrealSocketReader.ReadFloat();
                                    float z = unrealSocketReader.ReadFloat();

                                    Vector3D pos = new Vector3D() { X = x, Y = y, Z = z };

                                    float roll = unrealSocketReader.ReadFloat();
                                    float pitch = unrealSocketReader.ReadFloat();
                                    float yaw = unrealSocketReader.ReadFloat();

                                    Vector3D agl = new Vector3D() { X = roll, Y = pitch, Z = yaw };
                                    Logger.Debug("Received position: {0}", pos);
                                    Logger.Debug("Received angle: {0}", agl);

                                    PositionData pd = new PositionData()
                                    {
                                        X = x,
                                        Y = y,
                                        Z = z,
                                        Roll = roll,
                                        Pitch = pitch,
                                        Yaw = yaw,
                                        Time = DateTime.Now // ,
                                                            // Test = test
                                    };
                                    OnPositionReceived(pd);

                                    break;
                                case "end":
                                    IsAlive = false;
                                    keepRunning = false;
                                    OnEnding();
                                    break;
                                case "restart":
                                    keepRunning = false;
                                    OnRestarting();
                                    break;
                                default:
                                    Logger.Error("Unknown Command: {0}", cmd);
                                    break;
                            }
                            Logger.Debug("Received command: {0}", cmd);
                        }
                        catch (Exception e)
                        {
                            // Try to reread message. If this fails the loop breaks automatically
                        }
                    }
                }
                catch (Exception e)
                {
                    keepRunning = false;
                }
            }
        }

        protected virtual void OnRestarting()
        {
            Logger.Info("Restarting Test");
            Restarting?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnEnding()
        {
            this.Close();

            Logger.Info("Ending Test");
            Ending?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPositionReceived(PositionData e)
        {
            PositionReceived?.Invoke(this, e);
        }

        protected virtual void OnErrorOccurred(Exception e)
        {
            ErrorOccoured?.Invoke(this, e);
        }
    }
}
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
                Socket.Close();
                Socket = null;
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
                        // TODO: Correct Path (PathHelper)
                        unrealEnginePath = unitOfWork.ScenarioRepository.Get(
                            data => data.Name == "Balance").FirstOrDefault()?.ExecutionPath;
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

        private readonly ControlDelegates.ViewModelCallbackMethod disableUnrealEngineCallback;
        private readonly ControlDelegates.ErrorMessageCallbackMethod errorMessageCallback;

        private Thread socketListenerThread;
        private TcpListener socketListener;

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
        public UnrealEngineController(ControlDelegates.ErrorMessageCallbackMethod errorMessageCallback, ControlDelegates.ViewModelCallbackMethod disableUnrealEngineCallback)
        {
            this.errorMessageCallback = errorMessageCallback;
            this.disableUnrealEngineCallback = disableUnrealEngineCallback;

            if (UnrealEnginePath == null)
            {
                //throw new NotSupportedException("Could not find scenario data!");
            }

            this.sockets = new List<UnrealSocket>();

            IsAlive = true;
            socketListener = new TcpListener(IPAddress.Any, UnrealEngineSocketPort);
            socketListener.Start();

            socketListenerThread = new Thread(ListenForEngineConnect);
            socketListenerThread.Start();
        }

        private void ListenForEngineConnect()
        {
            try
            {
                // TODO Dispose socketListener
                // TPDP Correct message (no connection there at this point!)
                Logger.Info("Unreal Engine socket connection established on port {0} and waiting for connections...",
                    UnrealEngineSocketPort);
                while (IsAlive)
                {
                    Socket socket = socketListener.AcceptSocket();

                    UnrealSocket unrealSocket = new UnrealSocket(socket);
                    sockets.Add(unrealSocket);

                    // run command listener thread
                    Thread trackingThread = new Thread(() => Run(unrealSocket));
                    trackingThread.Start();
                    Logger.Info("Listener socket thread initialized.");
                }
            }
            catch (Exception e)
            {
                IsAlive = false;
                const string message = "Could not initialize Unreal Engine socket instance.";
                //Logger.Error(message, e);
                Logger.Error(e, message);
                errorMessageCallback(message);
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
                    socket.UnrealSocketWriter.WriteString("end");
                    socket.UnrealSocketWriter.Send();
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
                Thread.Sleep(2000); // TODO Is this needed? We wait for end message anyway.
                try
                {
                    Parallel.ForEach(sockets, socket =>
                    {
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
                    });
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
                errorMessageCallback(message);
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

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                // TODO: This is for test purpose only !!!!
                #region Test purpose only
                //Subject subject = unitOfWork.SubjectRepository.Get().FirstOrDefault();
                //if (subject == null)
                //{
                //    subject = new Subject()
                //    {
                //        FirstName = "Max",
                //        LastName = "Musterman",
                //        DateOfBirth = new DateTime(1981, 10, 24),
                //        Gender = Gender.Male,
                //        CountryCode = "AT",
                //        Function = "-debug-",
                //        City = "Hagenberg",
                //        ServiceRank = "Kloputzer"
                //    };
                //    unitOfWork.SubjectRepository.Insert(subject);
                //    unitOfWork.Save();
                //}
                //Scenario scenario = unitOfWork.ScenarioRepository.Get().FirstOrDefault();
                //if (scenario == null)
                //{
                //    scenario = new Scenario
                //    {
                //        Name = "Balance",
                //        ExecutionPath = "ich bin eine Biene",
                //        MinimapPath = "Balance/minimap.png",
                //        Version = "1.0",

                //        ZeroX = 1921,
                //        ZeroY = 257,
                //        Scale = 1354.0 / 24000.0
                //    };
                //    unitOfWork.ScenarioRepository.Insert(scenario);
                //    unitOfWork.Save();
                //}
                //Test test = new Test()
                //{
                //    Subject = subject,
                //    Scenario = scenario,
                //    Time = DateTime.Now,
                //    Title = "Testrun DEBUG - " + DateTime.Now,
                //    Location = "PLS 5"
                //};
                //unitOfWork.TestRepository.Insert(test);

                #endregion

                InitPositionTracking(unrealSocket);
                Logger.Info("Unreal Engine listener socket thread active!");
                bool keepRunning = true;
                while (IsAlive && keepRunning)
                {
                    unrealSocketReader.Receive();

                    while (unrealSocketReader.BytesAvailable > 0)
                    {
                        String cmd = unrealSocketReader.ReadString();
                        Logger.Info("Received command with Length: {0} - {1}", cmd.Length, cmd);

                        Vector3D pos;
                        Vector3D agl;

                        switch (cmd.ToLower())
                        {
                            case "posagl":
                                float x = unrealSocketReader.ReadFloat();
                                float y = unrealSocketReader.ReadFloat();
                                float z = unrealSocketReader.ReadFloat();

                                pos = new Vector3D() { X = x, Y = y, Z = z };

                                float roll = unrealSocketReader.ReadFloat();
                                float pitch = unrealSocketReader.ReadFloat();
                                float yaw = unrealSocketReader.ReadFloat();

                                agl = new Vector3D() { X = roll, Y = pitch, Z = yaw };
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

                                //test.PositionData.Add(pd);
                                //unitOfWork.PositionDataRepository.Insert(pd);
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
                                Logger.Error("Unkown Command: {0}", cmd);
                                break;
                        }
                        Logger.Debug("Received command: {0}", cmd);
                    }
                }

                //Test x = unitOfWork.TestRepository.Get(test => test.Scenario.Name == "irgendwas").FirstOrDefault();
                //Logger.Debug(x?.Scenario.Name + "; " + x?.Subject.FirstName);

                if (!IsAlive)
                    disableUnrealEngineCallback();
                try
                {
                    socket?.Close();
                    Logger.Info("Socket successfully closed!");
                }
                catch (Exception e)
                {
                    //Logger.Warn("Socket stop failed!", e);
                    Logger.Warn(e, "Socket stop failed!");
                }
                finally
                {
                    // Save PositionData from test
                    unitOfWork.Save();

                    this.sockets.Remove(unrealSocket);
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

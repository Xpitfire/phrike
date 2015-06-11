using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NLog;
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

        public UnrealEngineModel()
        {
            TcpListener socketListener = new TcpListener(IPAddress.Any, UnrealEngineSocketPort);
            socketListener.Start();
            Logger.Info("Unreal Engine socket connection established on port {0} and waiting for connections...", UnrealEngineSocketPort);
            socket = socketListener.AcceptSocket();
            unrealSocketWriter = new SocketWriter(socket);
            unrealSocketReader = new SocketReader(socket);
        }

        public void Close()
        {
            try
            {
                unrealSocketWriter.WriteString("end");
                unrealSocketWriter.Send();
            }
            catch (Exception e)
            {
                Logger.Error("Could not send Unreal Engine exit command!");
            }
            finally
            {
                socket.Close();
                Logger.Info("Successfully closed socket connection!");
            }
        }

    }
}

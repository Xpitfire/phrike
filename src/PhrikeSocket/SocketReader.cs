// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocketReader.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SocketReader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using NLog;

namespace Phrike.PhrikeSocket
{
    using System.IO;
    using System.Net.Sockets;

    /// <summary>
    /// SocketReader to read data in a specific way from a given Socket.
    /// Used for communication between UE and GC.
    /// </summary>
    public class SocketReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private byte[] buffer;
        private Socket socket;
        private MemoryStream ms;
        private int length;
        private int bytesRead;

        private bool readAble;
        private bool receiveAble;
        private bool connected = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketReader"/> class.
        /// </summary>
        /// <param name="socket">
        /// The socket to receive data from.
        /// </param>
        public SocketReader(Socket socket)
        {
            this.socket = socket;

            this.receiveAble = true;
        }

        /// <summary>
        /// Gets a value indicating whether the reader is ready to receive data from the Socket.
        /// </summary>
        public bool CanReceive
        {
            get
            {
                return this.receiveAble;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the reader is ready to read data from a received byte-stream.
        /// </summary>
        public bool CanRead
        {
            get
            {
                return this.readAble;
            }
        }

        /// <summary>
        /// Gets the amount of Bytes available for reading on the Input-Buffer
        /// </summary>
        public int BytesAvailable
        {
            get
            {
                if (CanReceive && connected)
                    return this.length - this.bytesRead;
                return -1;
            }
        }

        /// <summary>
        /// Reads a String from the already received Byte-Stream.
        /// </summary>
        /// <exception cref="Exception">Throws an exception when the Reader is not ready to read.</exception>
        /// <returns>The next String that can be read from the buffer</returns>
        public string ReadString()
        {
            try
            {
                if (!connected)
                {
                    throw new Exception("Socket is not connected");
                }
                if (!this.CanRead)
                {
                    throw new Exception("Reader not ready to read!");
                }

                int strlen = this.ms.ReadByte();

                byte[] buf2 = new byte[strlen];
                this.ms.Read(buf2, 0, strlen);
                this.bytesRead += strlen + 1;

                string ret = Encoding.UTF8.GetString(buf2, 0, strlen);
                return ret;
            }
            catch (Exception e)
            {
                Logger.Warn(e, "Could not read command!");
                //return "end";
                throw;
            }
        }

        /// <summary>
        /// Reads a float from the already received Byte-Stream.
        /// </summary>
        /// <exception cref="Exception">Throws an exception when the Reader is not ready to read.</exception>
        /// <returns>The next float that can be read from the buffer</returns>
        public float ReadFloat()
        {
            if (!this.CanRead)
            {
                throw new Exception("Reader not ready to read!");
            }

            byte[] buf2 = new byte[4];
            this.ms.Read(buf2, 0, 4);
            this.bytesRead += 4;

            float ret = BitConverter.ToSingle(buf2, 0);
            return ret;
        }

        /// <summary>
        /// Receives a new byte-sausage from the Socket and readies the Reader for reading. Waits for &lt;timeout&gt; ms.
        /// If timeout == -1 than it blocks until it reads something
        /// </summary>
        public void Receive(int timeout)
        {
            try
            {
                if (!connected)
                {
                    throw new Exception("Socket is not connected");
                }
                if (!this.CanReceive)
                {
                    throw new Exception("Reader not ready to receive!");
                }
                this.socket.ReceiveTimeout = timeout;

                this.buffer = new byte[100];
                this.bytesRead = 0;
                this.length = this.socket.Receive(this.buffer);
                this.ms = new MemoryStream(this.buffer, 0, this.length);
                this.readAble = true;
            }
            catch (SocketException sex)
            {
                Logger.Warn("Lost client connection!", sex);
                this.connected = false;
            }
        }

        /// <summary>
        /// Receives a new byte-sausage from the Socket and readies the Reader for reading.
        /// </summary>
        public void Receive()
        {
            Receive(-1);
        }
    }
}

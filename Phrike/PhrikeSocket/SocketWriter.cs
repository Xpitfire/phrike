// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SocketWriter.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SocketWriter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;

namespace Phrike.PhrikeSocket
{
    using System.IO;
    using System.Net.Sockets;

    /// <summary>
    /// SocketWriter to write data in a specific way to a given Socket.
    /// Used for communication between UE and GC.
    /// </summary>
    public class SocketWriter
    {
        private byte[] buffer;
        private Socket socket;
        private MemoryStream ms;

        private bool writeAble;
        private bool sendAble;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketWriter"/> class. 
        /// </summary>
        /// <param name="socket">
        /// The Socket to send data to.
        /// </param>
        public SocketWriter(Socket socket)
        {
            this.Init();
            this.socket = socket;
        }

        /// <summary>
        /// Gets a value indicating whether the writer is ready to write data to a the buffer. 
        /// </summary>
        public bool CanWrite
        {
            get
            {
                return this.writeAble;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the writer is ready to write the buffer to the socket.
        /// </summary>
        public bool CanSend
        {
            get
            {
                return this.sendAble;
            }
        }

        /// <summary>
        /// Writes a String to the buffer.
        /// </summary>
        /// <param name="text">
        /// The string to write to the buffer.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception when the Reader is not ready to write.
        /// </exception>
        public void WriteString(string text)
        {
            if (!this.CanWrite)
            {
                throw new Exception("Writer not ready to Write!");
            }

            byte[] byteStr = Encoding.UTF8.GetBytes(text);

            this.ms.WriteByte((byte)byteStr.Length);
            this.ms.Write(byteStr, 0, byteStr.Length);

            if (!this.sendAble)
            {
                this.sendAble = true;
            }
        }

        /// <summary>
        /// Writes a float to the buffer.
        /// </summary>
        /// <param name="value">
        /// The float to write to the buffer.
        /// </param>
        /// <exception cref="Exception">
        /// Throws an exception when the Writer is not ready to write.
        /// </exception>
        public void WriteFloat(float value)
        {
            if (!this.CanWrite)
            {
                throw new Exception("Writer not ready to Write!");
            }

            byte[] bval = BitConverter.GetBytes(value);

            this.ms.Write(bval, 0, bval.Length);

            if (!this.sendAble)
            {
                this.sendAble = true;
            }
        }

        /// <summary>
        /// Sends the buffer as a byte-sausage to the Socket and reinitialises the Buffer.
        /// </summary>
        public void Send()
        {
            if (!this.CanSend)
            {
                throw new Exception("Writer not ready to send!");
            }

            this.socket.Send(this.buffer, Convert.ToInt32(this.ms.Position), SocketFlags.None);
            this.ms.Close();

            this.sendAble = false;
            this.Init();
        }

        /// <summary>
        /// (Re)initialisies the buffer and the MemoryStream.
        /// </summary>
        private void Init()
        {
            this.buffer = new byte[100];
            this.ms = new MemoryStream(this.buffer);
            this.writeAble = true;
        }
    }
}

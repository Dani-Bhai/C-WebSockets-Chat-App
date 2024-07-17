using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;

namespace ChattingApp.Net.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream _input;
        public PacketReader(NetworkStream input) : base(input)
        {
            _input = input;
        }

        public string ReadMessage()
        {
            byte[] msgBuffer;
            var length = ReadInt32();

            msgBuffer = new byte[length];
            _input.Read(msgBuffer, 0, length);

            var msg = Encoding.ASCII.GetString(msgBuffer);

            return msg;
        }
    }
}

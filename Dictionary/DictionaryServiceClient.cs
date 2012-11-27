using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace BluePrint.Dictionary
{
    public class DictionaryServiceClient:IDisposable
    {
        const char SOH = (char)0x01;
        EndPoint serverAddress;
        Socket socket;
        public DictionaryServiceClient()
        {
        }

        public void Connect(string ip, int port)
        {
            serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            this.socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            this.socket.Connect(serverAddress);
        }
        public void Connect(EndPoint serverAddress)
        {
            this.serverAddress = serverAddress;
            if(socket==null)
                this.socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            if(!socket.Connected)
                this.socket.Connect(serverAddress);
        }
        public void Disconnect()
        {
            this.socket.Disconnect(true);
        }

        internal string SendCommand(string command)
        {
            int buffersize = 4096;
            byte[] tempbuffer = new byte[buffersize];

            socket.Send(Encoding.UTF8.GetBytes(command), SocketFlags.None);

            //receive command returns
            int read = 0;
            MemoryStream bufferms = new MemoryStream();

            while ((read = socket.Receive(tempbuffer, 0, tempbuffer.Length, SocketFlags.None)) == buffersize)
            {
                bufferms.Write(tempbuffer, 0, buffersize);
            }
            bufferms.Write(tempbuffer, 0, read);

            string responseText = Encoding.UTF8.GetString(bufferms.ToArray());
            return responseText;
        }
        public TrieTreeResult ExactMatch(string word)
        {
            return ExactMatch(word, 0);
        }
        public TrieTreeResult ExactMatch(string word, int pos)
        {
            string commandText = string.Format("ExactMatch {0}" + Environment.NewLine, word);
            if (pos>0)
                commandText = string.Format("ExactMatch {1}{0}{2}" + Environment.NewLine, SOH, word, (int)pos);
            return SendAndHandleResponse(commandText);
        }
        private TrieTreeResult SendAndHandleResponse(string commandText)
        {
            string responseText = SendCommand(commandText);
            if (responseText.StartsWith("Server side error"))
                throw new InvalidOperationException("server side error");

            string[] data = responseText.Split(new char[] { SOH });
            if (data.Length <= 2)
                return null;
            else
            {
                TrieTreeResult ttr = new TrieTreeResult(data[1], Int32.Parse(data[3]), Int32.Parse(data[2]));
                return ttr;
            }
        }
        public TrieTreeResult MaximumMatch(string word)
        {
            return MaximumMatch(word, 0);
        }
        public TrieTreeResult MaximumMatch(string word, int pos)
        {
            string commandText = string.Format("MMFetch {0}" + Environment.NewLine, word);
            if (pos > 0)
                commandText = string.Format("MMFetch {1}{0}{2}" + Environment.NewLine, SOH, word, (int)pos);
            return SendAndHandleResponse(commandText);
        }
        public TrieTreeResult ReverseMaximumMatch(string word)
        {
            return ReverseMaximumMatch(word, 0);
        }
        public TrieTreeResult ReverseMaximumMatch(string word, int pos)
        {
            string commandText = string.Format("RMMFetch {0}" + Environment.NewLine, word);
            if (pos > 0)
                commandText = string.Format("RMMFetch {1}{0}{2}" + Environment.NewLine, SOH, word, (int)pos);
            return SendAndHandleResponse(commandText);
        }

        public void Dispose()
        {
            if (this.socket != null)
                this.socket.Disconnect(false);
        }
    }
}

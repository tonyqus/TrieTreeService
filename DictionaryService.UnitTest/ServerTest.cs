using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Sockets;
using BluePrint.SegmentFramework;
using BluePrint.Dictionary;

namespace DictionaryService.UnitTest
{
    [TestClass]
    public class ServerTest
    {
        [TestMethod]
        public void TestServerConnection()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (Socket socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(serverAddress);
                //byte[] temp = new byte[5000];
                //int read = socket.Receive(temp, 0, 5000, SocketFlags.None);

                //byte[] response = new byte[read];
                //Array.Copy(temp, 0, response, 0, read);
                //string responseText = Encoding.UTF8.GetString(response);
                //Assert.AreEqual("Welcome to EchoServer! 测试\r\n", responseText);

            }

        }

        int port = 7010;
        string ip = "127.0.0.1";
        const char SOH = (char)0x01;

        void AssertServerResponse(string response, bool expectedBoolean)
        {
            AssertServerResponse(response, expectedBoolean, null);
        }
        void AssertServerResponse(string response, bool expectedBoolean, string expectedWord)
        {
            AssertServerResponse(response, expectedBoolean, expectedWord, POSType.UNKNOWN);
        }
        void AssertServerResponse(string response, bool expectedBoolean, string expectedWord, POSType expectedPOS)
        {
            if (response.StartsWith("Server side error"))
                Assert.Fail("server error");
            string[] data = response.Split(new char[] { SOH });
            if (data.Length <= 2)
            {
                if (expectedBoolean == true)
                    Assert.Fail("expected 0 but was 1");
            }
            else
            {
                if (expectedBoolean == false)
                    Assert.Fail("expected 1 but was 0");

                if(expectedPOS!= POSType.UNKNOWN)
                    Assert.AreEqual(expectedPOS, (POSType)Int32.Parse(data[3]));
                Assert.AreEqual(expectedWord, data[1]);
            }   
        }
        string SendCommand(Socket socket, string command)
        {
            byte[] temp = new byte[5000];

            socket.Send(Encoding.UTF8.GetBytes(command), SocketFlags.None);
            int read = socket.Receive(temp, 0, 5000, SocketFlags.None); //receive command returns

            byte[] response = new byte[read];
            Array.Copy(temp, 0, response, 0, read);
            string responseText = Encoding.UTF8.GetString(response);
            return responseText;
        }
        [TestMethod]
        public void TestServerCommand_MMFetch()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (DictionaryServiceClient dsc = new DictionaryServiceClient())
            {
                dsc.Connect(serverAddress);
                string responseText = dsc.SendCommand("MMFetch 测试" + Environment.NewLine);
                AssertServerResponse(responseText, true, "测试");

                responseText = dsc.SendCommand("MMFetch 我们一起去吧" + Environment.NewLine);
                AssertServerResponse(responseText, true, "我们");
            }
        }
        [TestMethod]
        public void TestServerCommand_MMFetch_WithPOSType()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (Socket socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(serverAddress);

                string responseText = SendCommand(socket, string.Format("MMFetch {1}{0}{2}"+Environment.NewLine, SOH, "测试", (int)POSType.D_N));
                AssertServerResponse(responseText, true, "测试", POSType.D_N);

                responseText = SendCommand(socket, string.Format("MMFetch {1}{0}{2}" + Environment.NewLine, SOH, "我们一起去吧", (int)POSType.D_R));
                AssertServerResponse(responseText, true, "我们", POSType.D_R);
            }

        }
        [TestMethod]
        public void TestServerCommand_MMFetch_Null()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (Socket socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(serverAddress);

                string responseText = SendCommand(socket, "MMFetch abcdefg" + Environment.NewLine);
                AssertServerResponse(responseText, false);
            }
        }
        [TestMethod]
        public void TestServerCommand_RMMFetch()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (Socket socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(serverAddress);

                string responseText = SendCommand(socket, "RMMFetch 测试" + Environment.NewLine);
                AssertServerResponse(responseText, true, "测试");

                responseText = SendCommand(socket, "RMMFetch 这里是我们的地盘" + Environment.NewLine);
                AssertServerResponse(responseText, true, "地盘");                
            }

        }
        [TestMethod]
        public void TestServerCommand_RMMFetch_Null()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (Socket socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(serverAddress);
                string responseText = SendCommand(socket, "RMMFetch abcdefg" + Environment.NewLine);
                AssertServerResponse(responseText, false);
            }
        }
        
        [TestMethod]
        public void TestMMFetchWrapper()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (DictionaryServiceClient dsc = new DictionaryServiceClient())
            {
                dsc.Connect(serverAddress);
                string responseText = dsc.MaximumMatch("测试").Word;
                Assert.AreEqual("测试", responseText);

                responseText = dsc.MaximumMatch("我们一起去吧").Word;
                Assert.AreEqual("我们", responseText);
            }
        }
        [TestMethod]
        public void TestRMMFetchWrapper()
        {
            EndPoint serverAddress = new IPEndPoint(IPAddress.Parse(ip), port);
            using (DictionaryServiceClient dsc = new DictionaryServiceClient())
            {
                dsc.Connect(serverAddress);
                string responseText = dsc.ReverseMaximumMatch("测试").Word;
                Assert.AreEqual("测试", responseText);

                responseText = dsc.ReverseMaximumMatch("请你相信我们").Word;
                Assert.AreEqual("我们", responseText);
            }
        }
    }
}

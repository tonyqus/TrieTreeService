using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

namespace BluePrint.DictionaryService
{
    public class TrieTreeSession : AppSession<TrieTreeSession>
    {
        public override void StartSession()
        {
            //SendResponse("Welcome to EchoServer! 测试");
        }

        public override void HandleExceptionalError(Exception e)
        {
            SendResponse("Server side error occurred!");
        }
    }
}

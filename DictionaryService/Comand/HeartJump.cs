using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;

namespace BluePrint.DictionaryService.Comand
{
    public class HeartJump : StringCommandBase<TrieTreeSession>
    {
        public override void ExecuteCommand(TrieTreeSession session, StringCommandInfo commandInfo)
        {
            Console.WriteLine("receive command HeartJump");
            session.SendResponse("1");
            Console.WriteLine("HeartJump returns: 1");
        }
    }
}

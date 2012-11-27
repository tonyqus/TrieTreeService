using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using BluePrint.SegmentFramework;
using BluePrint.NLPCore;
using System.Diagnostics;
using log4net;

namespace BluePrint.DictionaryService.Comand
{
    public class ExactMatch : StringCommandBase<TrieTreeSession>
    {
        static readonly ILog logger = LogManager.GetLogger(typeof(ExactMatch));
        const char SOH = (char)0x01;
        public override void ExecuteCommand(TrieTreeSession session, StringCommandInfo commandInfo)
        {
            logger.Info("receive command ExactMatch, argument: " + commandInfo.Data);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var tt = TrieTree.GetInstance();
            string[] splits = commandInfo.Data.Split(new char[] { SOH });
            string text = splits[0];
            int pos = 0;
            if (splits.Length > 1)
            {
                pos = Int32.Parse(splits[1]);
            }
            TrieTreeNode node = null;
            node = tt.GetNode(text, pos);
            sw.Stop();

            logger.InfoFormat("timer: {0} ms",sw.ElapsedMilliseconds);

            string result = null;
            if (node == null)
                result = string.Format("0{0}", SOH);
            else
                result = string.Format("1{0}{1}{0}{2}{0}{3}{0}", SOH, node.Word, node.Frequency, node.POSValue);

            if (node != null)
                 logger.InfoFormat("return result: {0}, {1}",  node.Word ,(POSType)node.POSValue);
            else
                logger.InfoFormat("return result: not found");
            session.SendResponse(result);            
        }
    }
}

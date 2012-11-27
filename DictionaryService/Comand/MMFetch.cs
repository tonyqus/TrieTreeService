using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Command;
using BluePrint.NLPCore;
using System.Diagnostics;
using BluePrint.SegmentFramework;
using log4net;

namespace BluePrint.DictionaryService.Command
{
    /// <summary>
    /// Maximum Match
    /// </summary>
    public class MMFetch : StringCommandBase<TrieTreeSession>
    {
        const char SOH = (char)0x01;
        static readonly ILog logger = LogManager.GetLogger(typeof(MMFetch));

        #region CommandBase<TrieTreeSession> Members

        public override void ExecuteCommand(TrieTreeSession session, StringCommandInfo commandInfo)
        {
            logger.Info("receive command MMFetch, argument: " + commandInfo.Data);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var tt = TrieTree.GetInstance();
            string[] splits = commandInfo.Data.Split(new char[]{SOH});
            int maxlength = 0;
            string text = splits[0];
            int pos = 0;
            if (splits.Length> 1)
            {
                pos = Int32.Parse(splits[1]);
            }
            maxlength = text.Length;    
            string temp = null;
            TrieTreeNode node=null;
            for (int j = maxlength; j > 0; j--)
            {
                temp = text.Substring(0, j> text.Length ? text.Length : j);
                node = tt.GetNode(temp, pos);
                if (node != null)
                    break;
            }
            sw.Stop();
            logger.InfoFormat("timer: {0} ms", sw.ElapsedMilliseconds);
            
            string result = null;
            if (node == null)
                result = string.Format("0{0}", SOH);
            else
                result = string.Format("1{0}{1}{0}{2}{0}{3}{0}", SOH, node.Word, node.Frequency, node.POSValue);

            if (node != null)
                logger.InfoFormat("return result: {0}, {1}", node.Word, (POSType)node.POSValue);
            else
                logger.InfoFormat("return result: not found");
            session.SendResponse(result);            
        }

        #endregion
    }
}

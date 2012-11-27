using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using SuperSocket.SocketEngine.Configuration;

namespace BluePrint.DictionaryService
{
    partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var serverConfig = ConfigurationManager.GetSection("socketServer") as SocketServiceConfig;
            if (!SocketServerManager.Initialize(serverConfig))
                return;

            if (!SocketServerManager.Start())
                SocketServerManager.Stop();

            Program.Init(); //init trietree data
        }

        protected override void OnStop()
        {
            SocketServerManager.Stop();
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            SocketServerManager.Stop();
            base.OnShutdown();
        }
    }
}

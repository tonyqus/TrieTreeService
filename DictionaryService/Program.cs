using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BluePrint.Dictionary;
using BluePrint.NLPCore;
using System.Diagnostics;
using SuperSocket.SocketEngine;
using SuperSocket.SocketEngine.Configuration;
using System.ServiceProcess;
using SuperSocket.Common;
using System.Reflection;
using System.IO;
using log4net;

namespace BluePrint.DictionaryService
{
    class Program
    {
        static readonly ILog logger = LogManager.GetLogger(typeof(Program));
        internal static void Init()
        {

            DataProviderSettings providerSettings = ConfigurationManager.GetSection("dictionaryService")
                            as DataProviderSettings;

            var providers = providerSettings.Instances;
            foreach (DataProviderSetting setting in providers)
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();    
                IDataProvider dataProvider = null;
                try
                {
                    dataProvider = (IDataProvider)Activator.CreateInstance(Type.GetType(setting.ProviderType, true), new object[] { setting });
                    TrieFactory.LoadFromDataProvider(dataProvider);
                }
                catch (FileNotFoundException e)
                {
                    logger.Error("Invalid provider setting: " + setting.ProviderType);
                }catch(TypeLoadException e)
                {
                    logger.Error("Invalid provider setting: " + setting.ProviderType);
                }
                timer.Stop();
                logger.InfoFormat("{0}: {1:0.00} seconds", setting.Name, timer.ElapsedMilliseconds / 1000.0);
            }

        }

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            LogUtil.Setup();

            if (args != null && args.Length > 0)
            {
                if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                {
                    SelfInstaller.InstallMe();
                    return;
                }
                else if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                {
                    SelfInstaller.UninstallMe();
                    return;
                }
                else if (args[0].Equals("-c", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Press any key to start server!");
                    Console.ReadKey();
                    Console.WriteLine();
                    RunAsConsole();
                }
                else
                {
                    Console.WriteLine(args[0]);
                }
            }
            else
            {
                RunAsService();
            }

            
            Console.Read();
        }

        static void RunAsConsole()
        {
            SocketServiceConfig serverConfig = ConfigurationManager.GetSection("socketServer") as SocketServiceConfig;
            if (!SocketServerManager.Initialize(serverConfig))
            {
                logger.Error("Failed to initialize SuperSocket server! Please check error log for more information!");
                return;
            }

            if (!SocketServerManager.Start())
            {
                logger.Error("Failed to initialize SuperSocket server! Please check error log for more information!"); 
                SocketServerManager.Stop();
                return;
            }
            Console.WriteLine("The server has been started! Press key 'q' to stop the server."); 

            Program.Init(); //init trietree data

            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                Console.WriteLine();
                continue;
            }

            SocketServerManager.Stop();

            Console.WriteLine();
            Console.WriteLine("The server has been stopped!"); 
        }

        static void RunAsService()
        {
            ServiceBase[] servicesToRun;

            servicesToRun = new ServiceBase[] { new MainService() };

            ServiceBase.Run(servicesToRun);
        }
    }
}

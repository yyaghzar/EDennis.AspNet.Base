﻿using EDennis.AspNet.Base.Launcher;
using ME = Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using IS = EDennis.AspNetIdentityServer;
using PApi = Hr.PersonApi;

namespace Hr.Launcher {
    public class Program : LauncherBase {
        
        public Program(ME.ILogger logger) : base(logger) { }

        static void Main(string[] args) {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            var logger = new SerilogLoggerProvider(Log.Logger).CreateLogger(typeof(LauncherBase).Name);


            new Program(logger).Launch(args, true, true);
            Console.WriteLine("Press any key to stop the servers.");
            Console.ReadKey(); //block with Console
        }


        /// <summary>
        /// When invoking with Launch method directly, include as an argument the following:
        /// ewhAllSuspend={SOME_GUID_FOR_SUSPENDING}
        /// These settings will be used as NamedEventWaitHandle names.  The caller should
        /// set their NamedEventWaitHandle (or EventWaitHandle) with the same names and with
        /// EventResetMode.ManualReset (to allow all threads to unblock).  Then, the caller
        /// can suspend all launched servers by invoking Set() on the EventWaitHandle that
        /// is associated with ewhAllSuspend.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="logger"></param>
        /// <param name="launchBrowser"></param>
        /// <param name="blockWithConsole"></param>
        public override void Launch(string[] args, bool launchBrowser = false, bool blockWithConsole = false) {
            var launchables = Launch(args, blockWithConsole,
                    IS.Program.Main, 
                    PApi.Program.Main 
                );

            if (launchBrowser == true)
                LaunchBrowsers(launchables);
        }


    }
}

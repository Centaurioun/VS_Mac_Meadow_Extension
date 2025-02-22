﻿using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Pads;

namespace Meadow.Sdks.IdeExtensions.Vs4Mac
{
    public class OutputLogger : ILogger
    {
        static ProgressMonitor monitor;
        static OutputProgressMonitor toolMonitor;

        public OutputLogger()
        {
            if (monitor == null)
            {
                toolMonitor = IdeApp.Workbench.ProgressMonitors.GetToolOutputProgressMonitor(true);
            }
            else
            {
                monitor.Dispose();
                monitor = null;
            }

            // Create/Recreate it
            monitor = IdeApp.Workbench.ProgressMonitors.GetOutputProgressMonitor("Meadow", IconId.Null, true, true, true);
        }

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var msg = formatter(state, exception);

            if(msg.Contains("StdOut"))
            {
                monitor?.Log.WriteLine(msg.Substring(15));
            }
            else
            {
                toolMonitor?.Log.WriteLine(msg);
            }
        }
    }
}
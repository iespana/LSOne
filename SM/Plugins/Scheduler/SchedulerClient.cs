using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using LSOne.DataLayer.BusinessObjects;
using LSRetail.DD.Common;
using IComConfig = LSOne.ViewPlugins.Scheduler.Utils.IComConfig;

namespace LSOne.ViewPlugins.Scheduler
{
    public class SchedulerClient : IDisposable
    {
        private const NetMode defaultNetMode = NetMode.TCP;
        private string host;
        private int port;
        private NetMode netMode;

        private IComConfig comConfig;

        private bool disposed;

        public SchedulerClient(string host)
            : this(host, 0, defaultNetMode)
        {
        }

        public SchedulerClient(string host, NetMode netMode)
            : this(host, 0, netMode)
        {
        }

        public SchedulerClient(string host, int port, NetMode netMode)
        {
            this.host = host;
            this.netMode = netMode;
            if (port > 0)
            {
                this.port = port;
            }
            else
            {
                
                this.port = AppConfig.GetRouterPortByMode((NetMode)this.netMode);
            }
        }

        private void InitializeComConfig()
        {
            EndpointAddress add = new EndpointAddress(DDUtils.GetAddress(host, port, "Cfg", (NetMode)netMode));

            var binding = AppConfig.GetBinding((NetMode)netMode, false);
            var channelFactory = new ChannelFactory<IComConfig>(binding, add);
            comConfig = channelFactory.CreateChannel();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (comConfig != null)
                    {
                        // Make sure that the channel isn't faulted when we dispose it
                        var channel = comConfig as IChannel;
                        if (channel != null && channel.State != CommunicationState.Faulted)
                        {
                            comConfig.Dispose();
                        }
                        comConfig = null;
                    }
                }

                disposed = true;
            }
        }


        public void InvalidateJobTriggers(Guid jobId)
        {
            RunConfigCommand(string.Empty);
        }

        public string RunJob(Guid jobId)
        {
            return RunConfigCommand(jobId.ToString());
        }

        public string RunJob(string command)
        {
            return RunConfigCommand(command);
        }


        private string RunConfigCommand(string configValue)
        {
            if (comConfig == null)
            {
                InitializeComConfig();
            }

            string resultMsg = comConfig.SendConfigCmd(Utils.DDConfigCommands.SchedulerCmd, string.Empty, configValue);
            if (resultMsg.Contains("Error") || String.IsNullOrEmpty(resultMsg))
            {
                throw new SchedulerServiceException(resultMsg);
            }

            return resultMsg;
        }
    }


    [Serializable]
    public class SchedulerServiceException : Exception
    {
        public SchedulerServiceException() { }
        public SchedulerServiceException(string message) : base(message) { }
        public SchedulerServiceException(string message, Exception inner) : base(message, inner) { }
        protected SchedulerServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}

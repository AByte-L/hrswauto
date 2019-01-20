using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
 
namespace ProgramMonitor.Core
{
    public class ListenClient : IListenCall
    {
        private static object syncObject = new object();
        private static ListenClient instance = null;

        private ProgramInfo programInfo = null;
        private string serviceHostAddr = null;
        private int autoReportRunningInterval = 300;

        private DuplexChannelFactory<IListenService> listenServiceFactory = null;
        private IListenService proxyListenService = null;
        private System.Timers.Timer reportRunningTimer = null;

        private ListenClient(ProgramInfo programInfo, string serviceHostAddr = null, int autoReportRunningInterval = 300)
        {
            programInfo.Id = CreateProgramId();

            this.programInfo = programInfo;
            this.serviceHostAddr = serviceHostAddr;
            if (autoReportRunningInterval >= 60) //最低1分钟的间隔
            {
                this.autoReportRunningInterval = autoReportRunningInterval;
            }
            BuildAutoReportRunningTimer();
        }

        private void BuildAutoReportRunningTimer()
        {
            reportRunningTimer = new System.Timers.Timer(autoReportRunningInterval * 1000);
            reportRunningTimer.Elapsed += (s, e) =>
            {
                ReportRunning();
            };
        }

        private void BuildListenClientService()
        {
            if (listenServiceFactory == null)
            {
                if (string.IsNullOrEmpty(serviceHostAddr))
                {
                    serviceHostAddr = System.Configuration.ConfigurationManager.AppSettings["ServiceHostAddr"];
                }
                InstanceContext instanceContext = new InstanceContext(instance);
                NetTcpBinding binding = new NetTcpBinding();
                binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                binding.SendTimeout = new TimeSpan(0, 5, 0);
                Uri baseAddress = new Uri(string.Format("net.tcp://{0}/ListenService", serviceHostAddr));
                listenServiceFactory = new DuplexChannelFactory<IListenService>(instanceContext, binding, new EndpointAddress(baseAddress));
            }
            proxyListenService = listenServiceFactory.CreateChannel();
        }

        public static ListenClient GetInstance(ProgramInfo programInfo, string serviceHostAddr = null, int autoReportRunningInterval = 300)
        {
            if (instance == null)
            {
                lock (syncObject)
                {
                    if (instance == null)
                    {
                        instance = new ListenClient(programInfo, serviceHostAddr, autoReportRunningInterval);
                        instance.BuildListenClientService();
                    }
                }
            }
            return instance;
        }

        public void ReportStart()
        {
            proxyListenService.Start(programInfo);
            reportRunningTimer.Start();
        }

        public void ReportStop()
        {
            proxyListenService.Stop(programInfo.Id);
            reportRunningTimer.Stop();
        }

        public void ReportRunning()
        {
            try
            {
                proxyListenService.ReportRunning(programInfo);
            }
            catch
            {
                BuildListenClientService();
            }
        }

        int IListenCall.Listen(string programId)
        {
            if (programInfo.Id.Equals(programId, StringComparison.OrdinalIgnoreCase))
            {
                if (programInfo.RunState >= 0)
                {
                    return 1;
                }
            }
            return -1;
        }


        private string CreateProgramId()
        {

            Process currProcess = Process.GetCurrentProcess();
            int procCount = Process.GetProcessesByName(currProcess.ProcessName).Length;
            string currentProgramPath = currProcess.MainModule.FileName;
            return GetMD5HashFromFile(currentProgramPath) + "_" + procCount;
        }

        private string GetMD5HashFromFile(string fileName)
        {
            try
            {
                byte[] hashData = null;
                using (FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, FileAccess.Read))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    hashData = md5.ComputeHash(fs);
                    fs.Close();
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashData.Length; i++)
                {
                    sb.Append(hashData[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile Error:" + ex.Message);
            }
        }

    }

}


using System;

public class Class1
{
    public static class Common
    {
        public static ConcurrentDictionary<string, ProgramInfo> ProgramInfos = null;

        public static ConcurrentDictionary<string, IListenCall> ListenCalls = null;

        public static ConcurrentBag<string> ManualStopProgramIds = null;

        public static System.Timers.Timer loadTimer = null;

        public static Timer listenTimer = null;

        public static SynchronizationContext SyncContext = null;

        public static Action<ProgramInfo, bool> RefreshListView;

        public static Action<ProgramInfo, bool> RefreshTabControl;

        public static int ClearInterval = 5;

        public static int ListenInterval = 2;

        public static bool Listening = false;

        public static string DbConnString = null;

        public static string[] NoticePhoneNos = null;

        public static string NoticeWxUserIds = null;

        public static ILog Logger = LogManager.GetLogger("ProgramMonitor");

        public const string SqlProviderName = "System.Data.SqlClient";

        public static void SaveProgramStartInfo(ProgramInfo programInfo, IListenCall listenCall)
        {
            programInfo.RunState = 0;
            ProgramInfos.AddOrUpdate(programInfo.Id, programInfo, (key, value) => programInfo);
            ListenCalls.AddOrUpdate(programInfo.Id, listenCall, (key, value) => listenCall);
            RefreshListView(programInfo, false);
            RefreshTabControl(programInfo, true);
            WriteLog(string.Format("程序名：{0}，版本：{1}，已启动运行", programInfo.Name, programInfo.Version), false);
        }

        public static void SaveProgramStopInfo(string programId)
        {
            ProgramInfo programInfo;
            if (ProgramInfos.TryGetValue(programId, out programInfo))
            {
                programInfo.RunState = -1;
                RefreshListView(programInfo, false);

                IListenCall listenCall = null;
                ListenCalls.TryRemove(programId, out listenCall);
                RefreshTabControl(programInfo, true);
            }
            WriteLog(string.Format("程序名：{0}，版本：{1}，已停止运行", programInfo.Name, programInfo.Version), false);
        }

        public static void SaveProgramRunningInfo(ProgramInfo programInfo, IListenCall listenCall)
        {
            if (!ProgramInfos.ContainsKey(programInfo.Id) || !ListenCalls.ContainsKey(programInfo.Id))
            {
                SaveProgramStartInfo(programInfo, listenCall);
            }
            programInfo.RunState = 1;
            RefreshTabControl(programInfo, true);
            WriteLog(string.Format("程序名：{0}，版本：{1}，正在运行中", programInfo.Name, programInfo.Version), false);
        }

        public static void AutoLoadProgramInfos()
        {
            if (loadTimer == null)
            {
                loadTimer = new Timer(1 * 60 * 1000);
                loadTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
                {
                    var timer = sender as Timer;
                    try
                    {
                        timer.Stop();
                        foreach (var item in ProgramInfos)
                        {
                            var programInfo = item.Value;
                            RefreshListView(programInfo, false);
                        }
                    }
                    finally
                    {
                        if (Listening)
                        {
                            timer.Start();
                        }
                    }
                };
            }
            else
            {
                loadTimer.Interval = 1 * 60 * 1000;
            }
            loadTimer.Start();
        }


        public static void AutoListenPrograms()
        {
            if (listenTimer == null)
            {
                listenTimer = new Timer(ListenInterval * 60 * 1000);
                listenTimer.Elapsed += delegate (object sender, ElapsedEventArgs e)
                {
                    var timer = sender as Timer;
                    try
                    {
                        timer.Stop();
                        foreach (var item in ListenCalls)
                        {
                            bool needUpdateStatInfo = false;
                            var listenCall = item.Value;
                            var programInfo = ProgramInfos[item.Key];
                            int oldRunState = programInfo.RunState;
                            try
                            {
                                programInfo.RunState = listenCall.Listen(programInfo.Id);
                            }
                            catch
                            {
                                if (programInfo.RunState != -1)
                                {
                                    programInfo.RunState = -1;
                                    needUpdateStatInfo = true;
                                }
                            }

                            if (programInfo.RunState == -1 && programInfo.StopTime.AddMinutes(5) < DateTime.Now) //如果停了5分钟，则发一次短信
                            {
                                SendNoticeSms(programInfo);
                                SendNoticeWeiXin(programInfo);
                                programInfo.RunState = -1;//重新刷新状态
                            }

                            if (oldRunState != programInfo.RunState)
                            {
                                needUpdateStatInfo = true;
                                WriteLog(string.Format("程序名：{0}，版本：{1}，运行状态变更为：{2}", programInfo.Name, programInfo.Version, programInfo.RunState), false);
                            }

                            RefreshTabControl(programInfo, needUpdateStatInfo);
                        }
                    }
                    finally
                    {
                        if (Listening)
                        {
                            timer.Start();
                        }
                    }
                };
            }
            else
            {
                listenTimer.Interval = ListenInterval * 60 * 1000;
            }

            listenTimer.Start();
        }

        public static void SendNoticeSms(ProgramInfo programInfo)
        {
            if (NoticePhoneNos == null || NoticePhoneNos.Length <= 0) return;

            using (DataAccess da = new DataAccess(Common.DbConnString, Common.SqlProviderName))
            {
                da.UseTransaction();
                foreach (string phoneNo in NoticePhoneNos)
                {
                    var parameters = da.ParameterHelper.AddParameter("@Mbno", phoneNo)
                              .AddParameter("@Msg", string.Format("程序名：{0}，版本：{1}，安装路径：{2}，已停止运行了，请尽快处理！",
                                            programInfo.Name, programInfo.Version, programInfo.InstalledLocation))
                              .AddParameter("@SendTime", DateTime.Now)
                              .AddParameter("@KndType", "监控异常通知")
                              .ToParameterArray();

                    da.ExecuteCommand("insert into OutBox(Mbno,Msg,SendTime,KndType) values(@Mbno,@Msg,@SendTime,@KndType)", paramObjs: parameters);
                }
                da.Commit();
                WriteLog(string.Format("程序名：{0}，版本：{1}，已停止运行超过5分钟，成功发送短信通知到：{2}",
                        programInfo.Name, programInfo.Version, string.Join(",", NoticePhoneNos)), false);
            }

        }

        public static void SendNoticeWeiXin(ProgramInfo programInfo)
        {
            if (string.IsNullOrEmpty(NoticeWxUserIds)) return;

            string msg = string.Format("程序名：{0}，版本：{1}，安装路径：{2}，已停止运行了，请尽快处理！",
                                            programInfo.Name, programInfo.Version, programInfo.InstalledLocation);
            var wx = new WeChat();
            var result = wx.SendMessage(NoticeWxUserIds, msg);

            if (result["errmsg"].ToString().Equals("ok", StringComparison.OrdinalIgnoreCase))
            {
                WriteLog(string.Format("程序名：{0}，版本：{1}，已停止运行超过5分钟，成功发送微信通知到：{2}", programInfo.Name, programInfo.Version, NoticeWxUserIds), false);
            }
        }

        public static void BuildConnectionString(string server, string db, string uid, string pwd)
        {
            SqlConnectionStringBuilder connStrBuilder = new SqlConnectionStringBuilder();
            connStrBuilder.DataSource = server;
            connStrBuilder.InitialCatalog = db;
            connStrBuilder.UserID = uid;
            connStrBuilder.Password = pwd;
            connStrBuilder.IntegratedSecurity = false;
            connStrBuilder.ConnectTimeout = 15;

            DbConnString = connStrBuilder.ToString();
        }

        public static void WriteLog(string msg, bool isError = false)
        {
            if (isError)
            {
                Logger.Error(msg);
            }
            else
            {
                Logger.Info(msg);
            }
        }




    }
}
}

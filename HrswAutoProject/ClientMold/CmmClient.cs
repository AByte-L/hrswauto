using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ICmmServer;
using Gy.HrswAuto.PLCMold;
using Gy.HrswAuto.UICommonTools;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace Gy.HrswAuto.ClientMold
{

    public class CmmClient
    {
        #region 数据成员属性
        private static object syncObj = new object(); // 同步对象
        private static Dictionary<string, int> _partNbSet = new Dictionary<string, int>();

        //private Timer _heartbeatTimer;
        /// <summary>
        /// 服务器端IP和端口配置
        /// </summary>
        public CmmServerConfig CmmSvrConfig { get; set; }
        // 当前检测ID
        public string CurPartId { get; set; }
        public ClientState State { get; set; }
        private ProxyFactory _proxyFactory; // 
                                            // 代理通道
        ICmmControl _cmmCtrl;
        IPartConfigService _partConfigService;
        // 远端服务器是否初始化
        private bool _IsInitialed = false;
        public bool IsInitialed
        {
            get { return _IsInitialed; }
            private set { _IsInitialed = value; }
        }
        // 是否激活可用
        private bool actived = true;
        public bool IsActived
        {
            get { return actived; }
            set { actived = value; }
        }
        // 与远端连接失败
        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            private set { _connected = value; }
        }

        // 结果处理
        public bool IsPass { get; set; } = false;
        private ResultRecord _resultRecord;

        //记录结果文件
        List<ResultRecord> resultRecords = new List<ResultRecord>();
        #endregion

        #region 初始化
        /// <summary>
        /// </summary>
        /// <param name="cmmSvrConfig">服务器配置，包括IP地址，端口号等</param>
        public CmmClient(CmmServerConfig cmmSvrConfig)
        {
            CmmSvrConfig = cmmSvrConfig;
            State = ClientState.CS_None;
            _connected = false;
        }

        public void InitClient()
        {
            try
            {
                _proxyFactory = null;
                _proxyFactory = new ProxyFactory(this);
                _cmmCtrl = _proxyFactory.GetCmmControl(CmmSvrConfig);
                _partConfigService = _proxyFactory.GetPartConfigService(CmmSvrConfig);
                _IsInitialed = _cmmCtrl.IsInitialed(); // 返回服务器端是否初始化
                _connected = true;
                if (_IsInitialed)
                {
                    State = ClientState.CS_Idle;
                    string cmmError = string.Format($"三坐标{CmmSvrConfig.ServerID} 初始化成功");
                    ClientUICommon.RefreshCmmEventLog(cmmError);
                }
                else
                {
                    State = ClientState.CS_InitError;  // 
                    string cmmError = string.Format($"三坐标{CmmSvrConfig.ServerID}没有初始化");
                    ClientUICommon.RefreshCmmEventLog(cmmError);
                }
            }
            catch (Exception ex)
            {
                _IsInitialed = false; // 初始化不成功
                                      //string cmmError = string.Format($"三坐标{CmmSvrConfig.ServerID} 连接错误");
                                      //_connected = false;
                                      //ClientUICommon.RefreshCmmEventLog(cmmError);
                                      //State = ClientState.CS_ConnectError;
                ReportConnectError(ex);
            }
        }

        #endregion

        #region 本地方法
        /// <summary>
        /// 确定是否存在工件ID，如果不存在工件
        /// 则向PLC设置未找到工件标示位
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public bool FindPart(string partId)
        {
            return PartConfigManager.Instance.Exists(partId);
        }

        /// <summary>
        /// 确认本地文件是否完整
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        private bool VerifyLocalFiles(string partId)
        {
            PartConfig partConfig = PartConfigManager.Instance.GetPartConfig(partId);
            if (!File.Exists(PathManager.Instance.GetPartProgramPath(partConfig)) ||
                !File.Exists(PathManager.Instance.GetPartFlvPath(partConfig)) ||
                !File.Exists(PathManager.Instance.GetPartNomPath(partConfig)) ||
                !File.Exists(PathManager.Instance.GetPartTolPath(partConfig)))
            {
                return false;
            }
            return true;
        }

        // 继续上件
        // todo 抓取工件后，机器人将工件放置到槽中，返回槽号，刷新结果界面
        public void Continue()
        {
            // 送抓料取料命令
            //SendPlaceAndGripRequest(CurPartRecord.IsPass);
            State = ClientState.CS_Busy;
            SendGripRequest(IsPass);
        }

        internal void StartMeasureWork()
        {
            StartMeasureWorkFlow(CurPartId);
        }

        public void StartWorkFlow()
        {
            //State = ClientState.CS_Busy;
            SendCmmReadyMessage();
        }


        #endregion

        #region Remote方法
        /// <summary>
        /// 启动cmm测量工作流
        /// </summary>
        /// <param name="partId"></param>
        public void StartMeasureWorkFlow(string partId)
        {
            //CurPartId = partId;
            //if (!FindPart(partId))
            //{
            //    // 提示未发现工件
            //    return;
            //}
            // 添加工件标识Dictionary到记录相同工件个数
            lock (syncObj)
            {
                if (!_partNbSet.ContainsKey(partId))
                {
                    _partNbSet.Add(partId, 0);
                }
            }

            try
            {
                bool ok = SetServerPartConfig(partId);

                if (!ok)
                {
                    Trace.Write("文件部署失败，请检查");
                    //State = State | ClientState.Error;
                    return;
                }

                if (!_cmmCtrl.IsInitialed())
                {
                    Trace.Write("PCDMIS未初始化");
                    return;
                }

                // 启动测量
                _cmmCtrl.MeasurePart(partId);
            }
            catch (Exception ex)
	        {
                ReportConnectError(ex);
            }
        }



        /// <summary>
        /// 设置服务器工件配置
        /// </summary>
        /// <param name="partId">工件标识</param>
        /// <returns></returns>
        private bool SetServerPartConfig(string partId)
        {

            // 发现远端存在工件配置
            if (_partConfigService.FindPart(partId))
            {
                return true;
            }

            if (!VerifyLocalFiles(partId))
            {
                Trace.Write("文件缺失");
                return false;
            }
            PartConfig partConfig = PartConfigManager.Instance.GetPartConfig(partId);
            string[] pfileNames = Directory.GetFiles(PathManager.Instance.GetProgsFullPath());
            string progFileWithoutExt = Path.GetFileNameWithoutExtension(partConfig.ProgFileName);
            bool IsSuccessUpLoad = false;
            // 程序文件和CAD文件集中在一个目录里，可以公用
            foreach (string item in pfileNames)
            {
                // 上传progs目录中的所有相关文件
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(item);
                string fileExt = Path.GetExtension(item);
                if (string.Compare(progFileWithoutExt, fileNameWithoutExt, true) == 0 /*&& string.Compare(fileExt, ".PRG^", true) != 0 && string.Compare(fileExt, ".PRG", true) == 0*/)
                {
                    IsSuccessUpLoad = UpFileToServer1(partConfig.PartID, item, 0/*filePath*/);
                    if (!IsSuccessUpLoad)
                    {
                        return false;
                    }
                }
            }
            // 上传Blades文件 
            // root\blades\partid\...
            string bladeFilesPath = PathManager.Instance.GetBladesFullPath(partConfig.PartID);
            string[] bfileNames = Directory.GetFiles(bladeFilesPath);
            IsSuccessUpLoad = false;
            // blades文件进行工件标识分类
            foreach (string item in bfileNames)
            {
                // 上传progs目录中的所有相关文件
                IsSuccessUpLoad = UpFileToServer1(partConfig.PartID, item, 1/*filePath*/); // 放入服务器的blades文件目录中
                if (!IsSuccessUpLoad)
                {
                    return false;
                }
            }
            // 上传完毕添加工件配置
            _partConfigService.AddPartConfig(partConfig);
            return true;
            //catch (Exception ex) // 处理
            //{
            //    string cmmError = string.Format($"三坐标{CmmSvrConfig.ServerID} 文件部署错误， 异常信息：{ex.Message}");
            //    _connected = false;
            //    ClientUICommon.RefreshCmmEventLog(cmmError);
            //    State = ClientState.CS_Error;
            //    throw ex
            //}
            //return false;

        }

        public void QueryCmmServer()
        {
            try
            {
                _connected = _cmmCtrl.QueryCmmServer();
            }
            catch (Exception)
            {
                _connected = false;
                //try
                //{
                //    _cmmCtrl = _proxyFactory.GetCmmControl(CmmSvrConfig);
                //    _partConfigService = _proxyFactory.GetPartConfigService(CmmSvrConfig);
                //    _connected = true;
                //}
                //catch
                //{
                //    _connected = false;
                //}
            }
        }

        public bool CmmIsInitialed()
        {
            try
            {
                return  _cmmCtrl.IsInitialed();
            }
            catch (Exception ex)
            {
                ReportConnectError(ex);
            }
            return false;
        }

        private void ReportConnectError(Exception ex)
        {
            string cmmError = string.Format($"三坐标{CmmSvrConfig.ServerID} 连接异常：{ex.Message}");
            _connected = false;
            ClientUICommon.RefreshCmmEventLog(cmmError);
            State = ClientState.CS_ConnectError;
        }

        /// <summary>
        /// 上传单个文件到服务器
        /// </summary>
        /// <param name="partId">工件标识</param>
        /// <param name="filefullPath">文件全路径名</param>
        /// <param name="filePath">相对路径，baldes\partId或者programs</param>
        /// <returns></returns>
        private bool UpFileToServer(string partId, string filefullPath, string filePath)
        {
            using (Stream fs = new FileStream(filefullPath, FileMode.Open, FileAccess.Read))
            {
                UpFile fileData = new UpFile();
                fileData.FileName = Path.GetFileName(filefullPath);
                fileData.FileSize = fs.Length;
                fileData.FilePath = filePath;
                fileData.FileStream = fs;
                fileData.PartId = partId;
                UpFileResult ures = _partConfigService.UpLoadFile(fileData);
                //if (ures.IsSuccess)
                //{
                //    Debug.WriteLine("File up ok");
                //}
                return ures.IsSuccess;
            }
        }

        /// <summary>
        /// 上传单个文件到服务器
        /// </summary>
        /// <param name="partId">工件标识</param>
        /// <param name="filefullPath">文件全路径名</param>
        /// <param name="selPath">相对路径，baldes\partId或者programs</param>
        /// <returns></returns>
        private bool UpFileToServer1(string partId, string filefullPath, int selPath)
        {
            using (Stream fs = new FileStream(filefullPath, FileMode.Open, FileAccess.Read))
            {
                UpFile1 fileData = new UpFile1();
                fileData.FileName = Path.GetFileName(filefullPath);
                fileData.FileSize = fs.Length;
                fileData.selPath = selPath;
                fileData.FileStream = fs;
                fileData.PartId = partId;
                UpFileResult ures = _partConfigService.UpLoadFile1(fileData);
                //if (ures.IsSuccess)
                //{
                //    Debug.WriteLine("File up ok");
                //}
                return ures.IsSuccess;
            }
        }
        public void PullReport() 
        {
            //State = ClientState.CS_Busy;
            // 获取当前的cmm和rpt报告文件
            int partCount;
            lock (syncObj)
            {
                partCount = _partNbSet[CurPartId]; // 获得标识的工件个数
            }
            _resultRecord = null;
            _resultRecord = new ResultRecord();
            _resultRecord.PartID = CurPartId;
            _resultRecord.IsPass = IsPass;
            _resultRecord.ServerID = CmmSvrConfig.ServerID;
            _resultRecord.MeasDateTime = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");
            _resultRecord.PartNumber = ++partCount;
            bool ok = DownFileFromServer("cmm");
            if (!ok)
            {
                // 更新用户界面
                State = ClientState.CS_Error;
                //ClientUICommon.RefreshCmmViewState(CmmSvrConfig.ServerID, State);
                ClientUICommon.RefreshCmmEventLog("下载结果文件出错");
                return;
            }
            ok = DownFileFromServer("rpt");
            if (!ok)
            {
                // 更新用户界面
                State = ClientState.CS_Error;
                //ClientUICommon.RefreshCmmViewState(CmmSvrConfig.ServerID, State);
                ClientUICommon.RefreshCmmEventLog("下载结果文件出错");
                return;
            }
            // 添加报告记录，并且更改成可继续测量状态
            resultRecords.Add(_resultRecord);
            //ClientUICommon.AddPartResult(_resultRecord);
            State = ClientState.CS_Continue;
        }

        private bool DownFileFromServer(string v)
        {
            DownFile df = new DownFile();
            df.FileType = v;
            DownFileResult res = _partConfigService.DownLoadFile(df);
            if (res.IsSuccess)
            {
                string path = Path.GetDirectoryName(res.Message); // 在客户端建立相同的目录
                                                                  // 记录结果信息
                _resultRecord.FilePath = path;
                if (v.Equals("cmm", StringComparison.CurrentCultureIgnoreCase))
                {
                    _resultRecord.CmmFileName = Path.GetFileName(res.Message);
                }
                else
                {
                    _resultRecord.RptFileName = Path.GetFileName(res.Message);
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                byte[] buffer = new byte[res.FileSize];
                using (FileStream fs = new FileStream(res.Message, FileMode.Create, FileAccess.Write))
                {
                    int count = 0;
                    while ((count = res.FileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fs.Write(buffer, 0, count);
                    }
                    fs.Flush();
                    fs.Close();
                }
            }
            return res.IsSuccess;
        }
        #endregion

        #region PLC方法

        private RobotAction _robotAction;

        public void ChangeState(RobotAction state)
        {
            _robotAction = state;
        }

        public void OnPerformPlcRequest()
        {
            _robotAction.Perform();
        }

        public void SendCmmReadyMessage()
        {
            _robotAction = new CmmInitReady(this);
            _robotAction.Perform();
        }
        /// <summary>
        /// 发送上料请求
        /// </summary>
        public void SendPlaceRequest()
        {
            _robotAction = new RobotPlaceRequest(this);
            _robotAction.Perform();
        }

        /// <summary>
        /// 发送下料请求
        /// </summary>
        /// <param name="isPassed">当前测量工件是否合格</param>
        public void SendGripRequest(bool isPass)
        {
            _robotAction = new RobotGripRequest(this, isPass);
            _robotAction.Perform();
        }
        /// <summary>
        /// 响应下料完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnGripCompleted(object sender, EventArgs e)
        {
            // todo 返回料架的事件不定，需要一直监控料架
            PartRack.Instance.RefreshSlots(_resultRecord);
            // 继续上料
            // todo 定义标志可以停止流程, 比如改变State为Stop状态
            State = ClientState.CS_Continue; // 
        }

        /// <summary>
        /// 响应上料完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnPlaceCompleted(object sender, CompletedEventArgs e)
        {
            if (!FindPart(e.PartID))
            {
                //SendPartIDErrorSign(e.ClientID);
                //Trace.Write("工件标识错误");
                string str = "三坐标" + CmmSvrConfig.ServerID.ToString() + ":" + "读取工件标识错误";
                ClientUICommon.RefreshCmmEventLog(str);
                State = ClientState.CS_Error; // 设置客户端为错误状态
                return;
            }
            State = ClientState.CS_PlaceCompleted;
            CurPartId = e.PartID;
            //StartMeasureWorkFlow(e.PartID);
        }

        #endregion

        #region 测试方法
        Timer timer;
        public int runCount { get; set; } = 0;
        public string ErrorInfo { get; internal set; }

        public void WorkContinue()
        {
            timer = new Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            StartMeasureWorkFlow("TestPart");
            ++runCount;
            timer.Dispose();
        }


        #endregion
    }
}


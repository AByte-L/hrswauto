using Gy.HrswAuto.BladeMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ICmmServer;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.CmmServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class MeasureServiceContext : ICmmControl, IDisposable
    {
        /// <summary>
        /// 事件回调代理接口
        /// </summary>
        IWorkflowNotify _eventNotify;

        private PCDmisService _pcdmisCore; // PCDmis测量核心
        private BladeMeasAssist _bladeMeasAssist; // 工件测量辅助
        private BladeContext _bladeContext; // blade分析
        private PartConfig _part;
        public bool IsBladeMeasure { get; set; }

        public PartConfig CurPart
        {
            get { return _part; }
            private set { _part = value; }
        }

        #region 本地功能方法
        public MeasureServiceContext()
        {
            _pcdmisCore = new PCDmisService();
            _bladeMeasAssist = new BladeMeasAssist();
            _bladeContext = new BladeContext();
        }

        public void Initialize()
        {
            _pcdmisCore.InitialPCDmis();
            //_pcdmisCore.PCDmisMeasureEvent += _pcdmisCore_PCDmisMeasureEvent;
        }

        /// <summary>
        /// 通过工件标识获得零件检测程序
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        private string FindProgFile(string partId)
        {
            // 从工件配置管理器中获得工件列表
            PartConfig pc = PartConfigManager.Instance.GetPartConfig(partId);
            if (pc == null)
            {
                return string.Empty;
            }
            string filePath = Path.Combine(PathManager.Instance.Configration.ProgFilePath, pc.ProgFileName);
            //Debug.Assert(string.IsNullOrEmpty(filePath)); // 正常情况FilePath不是空字符串
            return filePath;
        }

        /// <summary>
        /// PCDMIS测量完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void _pcdmisCore_PCDmisMeasureEvent(object sender, PCDmisEventArgs e)
        {
            if (!e.IsCompleted)
            {
                Debug.WriteLine("PCDMIS没有完成执行");
                return;
            }
            // 开启Blade异步分析
            string bladeExe = @"C:\Program Files (x86)\Hexagon\PC-DMIS Blade 5.0 (Release)\Blade.exe";
            string RptFileName = Path.Combine(PathManager.Instance.Configration.ReportFilePath, _part.PartID, Path.GetFileNameWithoutExtension(_pcdmisCore.RtfFileName));
            RptFileName = RptFileName + DateTime.Now.ToString("MMddhhmmss") + ".rpt";
            bool ok = await Task.Run(() =>
            {
                _bladeMeasAssist.PCDmisRtfToBladeRpt(); // 转换rtf到rpt文件
                return _bladeContext.StartBlade(bladeExe, RptFileName);
            });
            if (ok)
            {
                // 执行结果分析

            }
        } 
        #endregion

        #region 通信接口实现
        public void ConnectWFEvents()
        {
            _eventNotify = OperationContext.Current.GetCallbackChannel<IWorkflowNotify>();
        }

        public void DisconnectWFEvents()
        {
            throw new NotImplementedException();
        }

        public bool IsInitialed()
        {
            return _pcdmisCore._IsInitialed;
        }

        public void MeasurePart(string partId)
        {
            _part = PartConfigManager.Instance.GetPartConfig(partId);
            Debug.Assert(_part != null);
            string partProgFileName = Path.Combine(PathManager.Instance.Configration.RootPath, "blades", partId, _part.ProgFileName);
            _pcdmisCore.OpenPartProgram(partProgFileName);
            _pcdmisCore.GetProgramCommandParameters(); // 获得测尖直径和输出文件
            if (_pcdmisCore.HasOutputFile) // 如果程序找到输出文件，则设置blade测量辅助
            {
                _bladeMeasAssist.Part = _part;
                _bladeMeasAssist.ProbeDiam = _pcdmisCore.ProbeDiam;
                _bladeMeasAssist.RtfFileName = _pcdmisCore.RtfFileName;
                if (IsBladeMeasure) // 
                {
                    _bladeMeasAssist.CreateBladeTxtFromNominal();
                }
            }
            // 创建Blade.txt文件
            if (!_pcdmisCore.ExecutePartProgram()) // 执行程序
            {
                Debug.WriteLine("pcdmis没有启动执行");
                return;
            }
        }

        public void Dispose()
        {
            _pcdmisCore.Dispose();
        }
        #endregion
    }
}

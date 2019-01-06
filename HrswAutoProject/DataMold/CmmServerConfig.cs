using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    public class CmmServerConfig
    {
        private string _hostIPAddress;
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string HostIPAddress
        {
            get { return _hostIPAddress; }
            set { _hostIPAddress = value; }
        }

        private int _cmmCtrlPost;
        /// <summary>
        /// 三坐标控制服务端口号
        /// </summary>
        public int CmmCtrlPost
        {
            get { return _cmmCtrlPost; }
            set { _cmmCtrlPost = value; }
        }

        private int _partFilesTransPost;
        /// <summary>
        /// 零件文件集传输服务端口号
        /// </summary>
        public int PartFilesTransPost
        {
            get { return _partFilesTransPost; }
            set { _partFilesTransPost = value; }
        }
    }
}

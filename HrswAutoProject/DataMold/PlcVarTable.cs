using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    /// <summary>
    /// PLC在内存中为每一台三坐标创建了一个数据块，用于控制命令及数据的交换
    /// 每个数据块都有唯一块号对应一台三坐标服务器
    /// </summary>
    public class PlcVarTable
    {
        /// <summary>
        /// 上料请求标志
        /// </summary>
        public bool PlaceRequest { get; set; }
        /// <summary>
        /// 上料完成标志
        /// </summary>
        public bool PlaceOk { get; set; }
        /// <summary>
        /// 下料请求标志
        /// </summary>
        public bool GripRequest { get; set; }
        /// <summary>
        /// 下料完成
        /// </summary>
        public bool GripOk { get; set; }
        /// <summary>
        /// 确认零件ID存在
        /// </summary>
        public bool VerifyPartId { get; set; }
        /// <summary>
        /// 当前零件ID
        /// </summary>
        public string CurrentPartId { get; set; }
    }
}

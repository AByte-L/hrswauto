using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{
    [Serializable]
    public enum ClientState
    {
        /// <summary>
        /// 初始状态
        /// </summary>
        CS_Idle,
        /// <summary>
        /// 测量完成
        /// </summary>
        CS_Completed,
        /// <summary>
        /// 三坐标忙碌
        /// </summary>
        CS_Busy,
        /// <summary>
        /// 测量过程错误
        /// </summary>
        CS_Error,
        /// <summary>
        /// 等待继续测量
        /// </summary>
        CS_Continue,
        /// <summary>
        /// 初始化错误
        /// </summary>
        CS_InitError
    }

}

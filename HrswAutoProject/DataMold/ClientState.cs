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
        CS_MeasCompleted,
        /// <summary>
        /// 等待继续测量
        /// </summary>
        CS_Continue,
        /// <summary>
        /// 下料完成状态
        /// </summary>
        CS_GripCompleted,
        /// <summary>
        /// 上料完成状态
        /// </summary>
        CS_PlaceCompleted,
        /// <summary>
        /// 三坐标忙碌
        /// </summary>
        CS_Busy,
        /// <summary>
        /// 上料过程
        /// </summary>
        CS_Place,
        /// <summary>
        /// 下料过程
        /// </summary>
        CS_Grip,
        /// <summary>
        /// 测量过程错误
        /// </summary>
        CS_Error,
        /// <summary>
        /// 初始化错误
        /// </summary>
        CS_InitError,
        /// <summary>
        /// 测量机连接错误
        /// </summary>
        CS_ConnectError,
        /// <summary>
        /// 下料错误
        /// </summary>
        CS_RobotGripError,
        /// <summary>
        /// 上料错误
        /// </summary>
        CS_RobotPlaceError,
        /// <summary>
        /// 无
        /// </summary>
        CS_None,
        CS_PlcConnectError
    }

}

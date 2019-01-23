using System;

namespace Gy.HrswAuto.ClientMold
{
    public class FeedRequestArg : EventArgs
    {
        /// <summary>
        /// 客户号，对应PLC存储器块号
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// 请求类型，上料下料
        /// </summary>
        public RequestType RqtType { get; set; }
        /// <summary>
        /// 合格标志，当RequestType为上料时没有作用
        /// </summary>
        public bool IsPassed { get; set; } 
    }
}
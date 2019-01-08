using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.PLCMold
{
    public class PlcClient
    {
        // S7 
        public void ResponseGripRequest(int clientId, bool IsPassed)
        {

        }

        public void ResponsePlaceRequest(int clientId)
        {

        }

        #region 单例实现
        private PlcClient()
        { }

        private static PlcClient _plcClient;

        public static PlcClient Instance
        {
            get
            {
                if (_plcClient == null)
                {
                    _plcClient = new PlcClient();
                }
                return _plcClient;
            }
        } 
        #endregion
    }
}

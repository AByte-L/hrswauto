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
        CS_Idle,
        CS_Busy,
        CS_Error,
        CS_Continue
    }

}

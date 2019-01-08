using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{
    [Serializable]
    [Flags]
    public enum ClientState
    {
        Client_IsActived = 0x1,
        Client_IsInitialized = 0x2,
        Client_Error = 0x3,
    }
}

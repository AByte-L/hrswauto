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
        Actived = 0x1,
        Initialized = 0x2,
        Error = 0x3,
        Continue = 0x4
    }
}

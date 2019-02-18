﻿using Gy.HrswAuto.DataMold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.UICommonTools
{
    public class ClientUICommon
    {
        public static SynchronizationContext syncContext; // 同步上下文

        public static Action<CmmServerConfig, bool> AddCmmToView;

        public static void UpdateCmmView(CmmServerConfig conf, bool state)
        {
            AddCmmToView(conf, state);
        }
    }
}

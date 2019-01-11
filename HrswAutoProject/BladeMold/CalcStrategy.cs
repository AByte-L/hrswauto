using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gy.HrswAuto.BladeMold
{
    public interface CalcStrategy
    {
        Tuple<int, int> CalcNoseTail(List<PointF> points);
    }
}
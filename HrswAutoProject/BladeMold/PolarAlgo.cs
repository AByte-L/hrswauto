using System;
using System.Collections.Generic;
using System.Drawing;

namespace Gy.HrswAuto.BladeMold
{
    public class PolarAlgo : CalcStrategy
    {
        public Tuple<int, int> CalcNoseTail(List<PointF> points)
        {
            float maxNx = 0; // x负值计算最大值
            int nx = 0; // nose标号
            float maxPx = 0; // x正值计算最大值
            int px = 0; // tail标号
            for (int i = 0; i < points.Count; i++)
            {
                float curPr = calcPolar(points[i]);
                if (points[i].X < 0) // 负轴
                {
                    if (curPr > maxNx)
                    {
                        maxNx = curPr;
                        nx = i;
                    }
                }
                else                // 正轴
                {
                    if (curPr > maxPx)
                    {
                        maxPx = curPr;
                        px = i;
                    }
                }
            }
            Tuple<int, int> result = new Tuple<int, int>(nx, px);
            return result;
        }

        private float calcPolar(PointF pointF)
        {
            return (float)Math.Sqrt(pointF.X * pointF.X + pointF.Y * pointF.Y);
        }
    }
}
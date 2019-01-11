using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gy.HrswAuto.BladeMold
{
    public class NomSection
    {
        #region 段数据成员
        public string _name { get; set; }
        private int _pointCount { get; set; }
        public List<string> _sectionStrings { get; set; } = new List<string>();
        private List<PointF> _sectionPoints { get; set; } = new List<PointF>();// 2d点，计算nose, tail
        public int _noseId { get; set; } = 0; // nose点标号
        public int _tailId { get; set; } = 0; // tail点标号
        private double _sectionZd { get; set; } // 截面Z值高度 
        #endregion
        /// <summary>
        /// 从流中读取截面段
        /// </summary>
        /// <param name="reader">数据流</param>
        public void ReadSection(StreamReader reader)
        {
            string line = "";
            Regex secReg = new Regex(@"^(SECTION)(\s\S+)(\s\d+)(\s\d+(\.\d+)?)");
            string pntStart = @"^(\s?-?\d+(\.\d+)?)";
            //string pntPattern = @"(-?\d+(\.\d+)?)";
            Regex pointReg = new Regex(pntStart);
            // 查找 段首标志
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (!secReg.IsMatch(line))
                {
                    continue;
                }
                var mac = secReg.Matches(line);
                _name = mac[0].Groups[2].Value.Trim();
                _pointCount = Convert.ToInt32(mac[0].Groups[3].Value.Trim());
                _sectionZd = Convert.ToDouble(mac[0].Groups[4].Value.Trim());
                break;
            }
            if (_pointCount == 0)
            {
                System.Windows.Forms.MessageBox.Show("没有查到到截面信息行");
                return;
            }

            // 当前读到 SECTION 一行
            while (!reader.EndOfStream && !pointReg.IsMatch(line))
            {
                line = reader.ReadLine();
                if (line.Contains("NOSE"))
                {
                    var s = line.Split(' ');
                    _noseId = int.Parse(s[1]);
                    continue;
                }
                if (line.Contains("TAIL"))
                {
                    var s = line.Split(' ');
                    _tailId = int.Parse(s[1]);
                    continue;
                }
            }

            // 当前读到 第一点
            int rCount = 0;
            while (!reader.EndOfStream && rCount < _pointCount)
            {
                // 分析第一点
                ++rCount;
                _sectionStrings.Add(line); // 添加原始字符串
                var pointStrings = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                PointF newP = new PointF();
                newP.X = float.Parse(pointStrings[0]);
                newP.Y = float.Parse(pointStrings[1]);
                _sectionPoints.Add(newP);
                // 读取下一个点
                if (rCount < _pointCount)
                {
                    line = reader.ReadLine();
                }
            }

            // 当前可能是流结尾或者是当前段最后一个点
            if (rCount < _pointCount)
            {
                System.Windows.Forms.MessageBox.Show("理论文件点集个数不够");
            }
            // 查找头缘尾缘点
            FindNoseTail();
        }
        /// <summary>
        /// 寻找前缘尾缘点
        /// </summary>
        private void FindNoseTail()
        {
            if (_noseId != 0)
            {
                if (_tailId == 0)
                {
                    _tailId = CalcEdgePoint(_noseId);
                }
            }
            else if (_tailId != 0)
            {
                _noseId = CalcEdgePoint(_tailId);
            }
            else
            {
                CalcNoseTail();
            }
        }
        /// <summary>
        /// 可以更换不同的策略，当前使用极径法
        /// </summary>
        private void CalcNoseTail()
        {
            // 最大距离算法得到的结果与实际不符
            CalcStrategy findNT = new PolarAlgo(); // 最大极径法
            Tuple<int, int> nt = findNT.CalcNoseTail(_sectionPoints);
            _noseId = nt.Item1;
            _tailId = nt.Item2;
        }

        /// <summary>
        /// 通过最远距离查找边缘极点
        /// </summary>
        /// <param name="startId">起始极点</param>
        /// <returns></returns>
        private int CalcEdgePoint(int startId)
        {
            PointF nose = _sectionPoints[startId - 1];
            float maxDis = 0;
            int pntId = 0;
            int result = -1;
            PointF s;
            _sectionPoints.ForEach(pnt =>
            {
                float tdis = Distance(nose, pnt);
                if (tdis > maxDis)
                {
                    maxDis = tdis;
                    result = pntId;
                    s = pnt;
                }
                pntId++;
            });
            return result;
        }

        private float Distance(PointF nose, PointF pnt)
        {
            return (float)Math.Sqrt(Math.Pow((nose.X - pnt.X), 2) + Math.Pow((nose.Y - pnt.Y), 2));
        }
    }
}

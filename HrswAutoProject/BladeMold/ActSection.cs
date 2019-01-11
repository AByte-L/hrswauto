using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gy.HrswAuto.BladeMold
{
    public class ActSection
    {
        public int _pointCount { get { return _sectionPoints.Count; } }
        public string _sectionName { get; set; }
        public List<string> _sectionPoints { get; set; } = new List<string>();
        public void ReadSection(StreamReader reader)
        {
            // 当前reader在段头部位置
            string line = "";
            int endFlag = 0; // 段结尾标志
            while (!reader.EndOfStream && endFlag != 2)
            {
                line = reader.ReadLine();

                if (line.Contains("DIM"))
                {
                    // 读取4行到第一点行，承认尺寸有4行
                    for (int i = 0; i < 4; i++)
                    {
                        line = reader.ReadLine();
                    }
                    // 判断第一点 不重复判断第一点增加性能
                    string[] pointStrings = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    // 字符串个数不够表示第一点下移一行
                    if (pointStrings.Count() < 8)
                    {
                        //
                        _sectionName = line.TrimEnd(" ".ToCharArray()).TrimStart(" ".ToCharArray());
                    }
                    else // 第一点处理
                    {
                        int pos = line.LastIndexOf(']');
                        _sectionName = line.Substring(0, pos + 1);
                        string[] pntStrs = line.Substring(pos + 1).Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        var pntStrList = pntStrs.ToList();
                        pntStrList.RemoveAt(0);
                        if (pntStrs.Last().Contains("MAX") || pntStrs.Last().Contains("MIN"))
                        {
                            pntStrList.RemoveAt(pntStrList.Count - 1);
                        }
                        string pntStr = string.Join(" ", pntStrList.ToArray());
                        _sectionPoints.Add(pntStr);
                    }
                    // 处理其他点
                    while (!reader.EndOfStream && endFlag != 2)
                    {
                        line = reader.ReadLine();
                        // 结尾处理
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            ++endFlag;
                            continue;
                        }
                        else
                        {
                            if (endFlag == 1)
                            {
                                --endFlag;
                            }
                        }

                        string[] pntStrs = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        var pntStrList = pntStrs.ToList();
                        pntStrList.RemoveAt(0);
                        if (pntStrs.Last().Contains("MAX") || pntStrs.Last().Contains("MIN"))
                        {
                            pntStrList.RemoveAt(pntStrList.Count - 1);
                        }
                        string pntStr = string.Join(" ", pntStrList.ToArray());
                        _sectionPoints.Add(pntStr);
                    }
                }
            }
        }
    }
}
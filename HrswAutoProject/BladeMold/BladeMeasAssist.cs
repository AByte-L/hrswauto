using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gy.HrswAuto.BladeMold
{
    public class BladeMeasAssist
    {
        #region 属性字段
        private PartConfig _partConfig;
        public PartConfig Part
        {
            get
            {
                return _partConfig;
            }

            set
            {
                _partConfig = value;
            }
        }

        // 由PCDmis测量程序得到
        private double _probeDiam; // 写入Rpt文件中
        public double ProbeDiam
        {
            get
            {
                return _probeDiam;
            }

            set
            {
                _probeDiam = value;
            }
        }

        private int _sectionNum;
        public int SectionNum
        {
            get
            {
                return _sectionNum;
            }
        }

        private List<string> _sectionNames = new List<string>();
        public List<string> SectionNames
        {
            get { return _sectionNames; }
        }

        // 由PCDmis程序中得到，或者固定一个文件名(参考BladeRunning)
        private string _rtfFileName;
        public string RtfFileName
        {
            get { return _rtfFileName; }
            set { _rtfFileName = value; }
        }

        // blade分析源文件
        private string _rptFileName;
        public string RptFileName
        {
            get { return _rptFileName; }
            set { _rptFileName = value; }
        }
        #endregion

        #region PCDmis测量输出结果文件Rtf转换成Blade分析源文件Rpt
        public void PCDmisRtfToBladeRpt()
        {
            if (!File.Exists(_rtfFileName))
            {
                Debug.WriteLine("rtf文件不存在");
                return;
            }
            // 设置rpt文件名
            _rptFileName = Path.Combine(PathManager.Instance.Configration.ReportFilePath, $"{_partConfig.PartID}", Path.GetFileNameWithoutExtension(_rtfFileName));

            // 转换RTF文件为TXT文件
            RichTextBox rtBox = new RichTextBox();
            string rtfText = File.ReadAllText(_rtfFileName);
            rtBox.Rtf = rtfText;
            string plainText = rtBox.Text.Replace("\n", "\r\n");
            string tmpFile = Path.Combine(PathManager.Instance.Configration.TempFilePath, (Path.GetFileNameWithoutExtension(_rtfFileName) + ".txt"));
            File.WriteAllText(tmpFile, plainText);

            // 读取截面扫描数据
            List<ActSection> sections = new List<ActSection>();
            using (StreamReader reader = File.OpenText(tmpFile))
            {
                for (int i = 0; i < _sectionNum; i++)
                {
                    ActSection cvSection = new ActSection();
                    cvSection.ReadSection(reader);
                    ActSection ccSection = new ActSection();
                    ccSection.ReadSection(reader);
                    sections.Add(cvSection);
                    sections.Add(ccSection);
                }
            }

            CreateBladeRpt(sections);
        }

        private void CreateBladeRpt(List<ActSection> sections)
        {
            if (_probeDiam < 0.001)
            {
                Debug.WriteLine("测尖直径不正确");
                return;
            }
            //string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\parts\" + part.PartID;
            //string path = @"C:\Users\Administrator\Desktop\AutoMeasure\ServerMainMod\bin\Debug" + @"\parts\" + part.PartID;
            //DateTime time = DateTime.Now;
            //int tcode = time.GetHashCode();
            //_rptName = part.PartID + "_" + tcode.ToString() + ".rpt";
            //if (part.config.ContainsKey(".rpt"))
            //{
            //    part.config[".rpt"] = _rptName;
            //}
            //else
            //{
            //    part.config.Add(".rpt", _rptName);
            //}
            //_fullRptName = Path.Combine(path, _rptName);
            StringBuilder strBuilder = new StringBuilder();
            using (FileStream stream = File.OpenWrite(_rptFileName))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    #region 写入文件头
                    // 文件头样例
                    //FLAVOR C:\BladeRunner\blades\blade5\blade5.flv
                    //PART blade5
                    //DATE 12 / 02 / 18 18:06:49
                    //NOMINAL C:\BladeRunner\blades\blade5\blade5.nom
                    //TOLERANCE C:\BladeRunner\blades\blade5\blade5.tol
                    //RADIUS 2.00000
                    //TEXT SERNO = 1
                    //TEXT OPERATION = MesBlade
                    //TEXT CMM = SC - 201807031426
                    //TEXT PARTCOUNT = 1
                    //AUTOPRINT OFF
                    //SAVEDP OFF
                    //NUM_SECT  1
                    //SECTION A-A  896   448   448
                    string path = PathManager.Instance.Configration.BladeFilePath + $"{_partConfig.PartID}";
                    string timeFormat = @"MM\\dd\\yy HH:mm:ss";
                    strBuilder.AppendLine("FLAVOR " + Path.Combine(path, _partConfig.FlvFileName));
                    strBuilder.AppendLine("PART " + _partConfig.PartID);
                    strBuilder.AppendLine("DATE " + DateTime.Now.ToString(timeFormat));
                    strBuilder.AppendLine("NOMINAL " + Path.Combine(path, _partConfig.NormFileName));
                    strBuilder.AppendLine("TOLERANCE " + Path.Combine(path, _partConfig.TolFileName));
                    strBuilder.AppendLine("RADIUS " + string.Format($"{_probeDiam / 2}"));
                    strBuilder.AppendLine("TEXT SERNO = 1");
                    strBuilder.AppendLine("TEXT OPERATION = MesBlade");
                    strBuilder.AppendLine($"TEXT CMM = {System.Environment.MachineName}");
                    strBuilder.AppendLine("TEXT PARTCOUNT = 1");
                    strBuilder.AppendLine("AUTOPRINT OFF");
                    strBuilder.AppendLine("SAVEDP OFF");
                    strBuilder.AppendLine($"NUM_SECT {_sectionNum}");
                    writer.Write(strBuilder.ToString());
                    #endregion
                    // 循环写入段数据
                    for (int i = 0; i < _sectionNum; i++)
                    {
                        int total = sections[i * 2]._pointCount + sections[i * 2 + 1]._pointCount;
                        writer.WriteLine($"SECTION {_sectionNames[i]}   {total}   {sections[i * 2]._pointCount}   {sections[i * 2 + 1]._pointCount}");
                        writer.Write(string.Join("\r\n", sections[i * 2]._sectionPoints));
                        writer.WriteLine();
                        writer.Write(string.Join("\r\n", sections[i * 2 + 1]._sectionPoints));
                        writer.WriteLine();
                    }
                }
            }
        } 
        #endregion

        #region 创建用于PCDMIS测量叶片的Blade.txt文件
        public void CreateBladeTxtFromNominal()
        {
            // 工件工作目录
            //string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\parts\" + part.PartID;
            //string path = @"C:\Users\Administrator\Desktop\AutoMeasure\ServerMainMod\bin\Debug" + @"\parts\" + part.PartID;
            //if (!part.config.ContainsKey(".nom"))
            //{
            //    MessageBox.Show("工件配置中没有找到理论文件名，配置不完整");
            //    return;
            //}
            //string nomFileName = Path.Combine(path, part.config[".nom"]);
            //if (!File.Exists(nomFileName))
            //{
            //    MessageBox.Show("理论文件不存在");
            //    return;
            //}
            // 
            string path = Path.Combine(PathManager.Instance.Configration.BladeFilePath, $"{_partConfig.PartID}", _partConfig.NormFileName);
            Debug.Assert(File.Exists(path)); // 理论文件应该存在
            CreateBladeTxt(path);
        }

        private void CreateBladeTxt(string nomFileName)
        {
            List<NomSection> sections = MakeSectionsFromFile(nomFileName);
            _sectionNum = sections.Count;
            sections.ForEach(section => _sectionNames.Add(section._name));
            if (sections != null)
            {
                // todo pcdmis程序调用的blade.txt文件位置
                string bladeRunFileName = Path.Combine(@"C:\BladeRunner", "blade.txt");
                CreateBladeRunFile(sections, bladeRunFileName);
            }
        }

        private void CreateBladeRunFile(List<NomSection> sections, string bladeRunFileName)
        {
            if (File.Exists(bladeRunFileName))
            {
                File.Delete(bladeRunFileName);
            }
            using (StreamWriter writer = new StreamWriter(File.OpenWrite(bladeRunFileName)))
            {
                // blade.txt 文件头
                // 写入工件号
                writer.WriteLine($"PART {_partConfig.PartID}");
                // 写入固定夹具 
                writer.WriteLine("FIXTURE_LOCATION 1");
                // 写入形状检测
                writer.WriteLine("PLATFORM_CHECK 0");
                // 写入截面数
                writer.WriteLine($"NUMBER_OF_SECTIONS {sections.Count}");
                // 循环写入前缘尾缘点
                foreach (var section in sections)
                {
                    //writer.WriteLine(section._sectionStrings[section._noseId - 1]);
                    //writer.WriteLine(section._sectionStrings[section._tailId - 1]);
                    string[] nosePnt = section._sectionStrings[section._noseId - 1].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var formatNosePnt = string.Format($"{nosePnt[0]}   {nosePnt[1]}   {nosePnt[2]}   {nosePnt[3]}   {nosePnt[4]}   {nosePnt[5]}   FULL");
                    writer.WriteLine(formatNosePnt);
                    string[] tailPnt = section._sectionStrings[section._tailId - 1].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var formatTailPnt = string.Format($"{tailPnt[0]}   {tailPnt[1]}   {tailPnt[2]}   {tailPnt[3]}   {tailPnt[4]}   {tailPnt[5]}   FULL");
                    writer.WriteLine(formatTailPnt);
                }
            }
        }

        private List<NomSection> MakeSectionsFromFile(string nomFileName)
        {
            List<NomSection> sections = null;
            using (StreamReader reader = File.OpenText(nomFileName))
            {
                Regex reg = new Regex(@"\b(-?\d+)(\.\d+)?\b"); // 数值
                Regex numreg = new Regex(@"^(NUM_SECT)(\s\d+)"); // 截面数
                //Regex secreg = new Regex(@"^(SECTION)(\s\S+)(\s\d+)"); // 截面点数

                int linenum = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    linenum++;
                    if (!numreg.IsMatch(line))
                    {
                        continue;
                    }
                    MatchCollection mac = numreg.Matches(line);
                    int sectCount = Convert.ToInt32(mac[0].Groups[2].Value); // 获得截面数
                    sections = new List<NomSection>();
                    for (int i = 0; i < sectCount; i++)
                    {
                        // 创建截面并写入blade.txt文件
                        NomSection sect = new NomSection();
                        sect.ReadSection(reader);
                        sections.Add(sect);
                    }
                }
            }
            return sections;
        } 
        #endregion
    }
}

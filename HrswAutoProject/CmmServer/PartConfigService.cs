using Gy.HrswAuto.ICmmServer;
using System;
using System.IO;
using System.ServiceModel;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.Utilities;

namespace Gy.HrswAuto.CmmServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class PartConfigService : IPartConfigService
    {
        public bool AddPartConfig(PartConfig partConfig)
        {
            return PartConfigManager.Instance.AddPartConfig(partConfig);
        }


        public bool FindPart(string partId)
        {
            return PartConfigManager.Instance.Exists(partId);
        }

        #region 上传文件实现
        /// <summary>
        /// 上传单个文件, 客户端需要多次调用
        /// 客户端控制文件保存路径
        /// </summary>
        /// <param name="filedata">文件信息及stream</param>
        /// <returns></returns>
        public UpFileResult UpLoadFile(UpFile filedata)
        {
            UpFileResult result = new UpFileResult();

            // etc ..\blades\partId
            string path = Path.Combine(PathManager.Instance.RootPath, filedata.FilePath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            byte[] buffer = new byte[filedata.FileSize];
            string fileFullPath = Path.Combine(path, filedata.FileName);
            FileStream fs = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write);

            int count = 0;
            while ((count = filedata.FileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, count);
            }
            //清空缓冲区
            fs.Flush();
            //关闭流
            fs.Close();

            result.IsSuccess = true;

            return result;
        }

        public UpFileResult UpLoadFile1(UpFile1 filedata)
        {
            UpFileResult result = new UpFileResult();

            //// etc ..\blades\partId
            //string path = Path.Combine(PathManager.Instance.Configration.RootPath, filedata.FilePath);
            string path;
            if (filedata.selPath == 0) // 程序类文件
            {
                path = PathManager.Instance.GetProgsFullPath();
            }
            else if (filedata.selPath == 1) // Blades类文件
            {
                path = PathManager.Instance.GetBladesFullPath(filedata.PartId);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "文件目录不确定";
                return result;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            byte[] buffer = new byte[filedata.FileSize];
            string fileFullPath = Path.Combine(path, filedata.FileName);
            string fileext = Path.GetExtension(filedata.FileName);
            if (File.Exists(fileFullPath) && (string.Compare(fileext, ".PRG^", true) == 0 || string.Compare(fileext, ".PRG~", true) == 0))
            {
                File.Delete(fileFullPath); // 删除PCDmis自动生成的隐藏文件
            }
            FileStream fs = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write);

            int count = 0;
            while ((count = filedata.FileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fs.Write(buffer, 0, count);
            }
            //清空缓冲区
            fs.Flush();
            //关闭流
            fs.Close();

            result.IsSuccess = true;

            return result;
        }
        #endregion

        /// <summary>
        /// 下载文件服务实现
        /// </summary>
        /// <param name="downfile">表示文件类型</param>
        /// <returns>文件流</returns>
        public DownFileResult DownLoadFile(DownFile downfile)
        {
            DownFileResult result = new DownFileResult();

            //string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\service\" + filedata.FileName;
            string path = ""; 
            if (downfile.FileType.Equals("cmm", StringComparison.CurrentCultureIgnoreCase))
            {
                path = PathManager.Instance.ReportsPath;
            }
            else if (downfile.FileType.Equals("rpt", StringComparison.CurrentCultureIgnoreCase))
            {
                path = PathManager.Instance.RptFilePath;
            }
            else
            {
                path = string.Empty;
            }

            if (!File.Exists(path))
            {
                result.IsSuccess = false;
                result.FileSize = 0;
                result.Message = "服务器不存在此文件";
                result.FileStream = new MemoryStream();
                return result;
            }
            Stream ms = new MemoryStream();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.CopyTo(ms);
            ms.Position = 0;  //重要，不为0的话，客户端读取有问题
            result.IsSuccess = true;
            result.FileSize = ms.Length;
            result.Message = path; // 文件在服务器上的路径
            result.FileStream = ms;

            fs.Flush();
            fs.Close();
            return result;
        }

    }
}

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
        public void AddPartConfig(PartConfig partConfig)
        {
            PartConfigManager.Instance.AddPartConfig(partConfig);
        }

        public bool FindPart(string partId)
        {
            return PartConfigManager.Instance.Exists(partId);
        }

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
            string path = Path.Combine(PathManager.Instance.Configration.RootPath, filedata.FilePath);

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
            string path = PathManager.Instance.Configration.RootPath;
            if (filedata.selPath == 0) // 程序类文件
            {
                path = Path.Combine(path, PathManager.Instance.Configration.ProgFilePath);
            }
            else if (filedata.selPath == 1) // Blades类文件
            {
                path = Path.Combine(path, PathManager.Instance.Configration.BladeFilePath, filedata.PartId);
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
    }
}

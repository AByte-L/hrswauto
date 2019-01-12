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
        /// </summary>
        /// <param name="filedata">文件信息及stream</param>
        /// <returns></returns>
        public UpFileResult UpLoadFile(UpFile filedata)
        {
            UpFileResult result = new UpFileResult();

            // etc ..\blades\partId
            string path = Path.Combine(PathManager.Instance.Configration.RootPath, "blades",  filedata.PartId);

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

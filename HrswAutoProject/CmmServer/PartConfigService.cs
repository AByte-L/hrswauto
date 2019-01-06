using Gy.HrswAuto.ICmmServer;
using System;
using System.IO;
using System.ServiceModel;

namespace CmmServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class PartConfigService : IPartConfigService
    {
        public bool FindPart(string partId)
        {
            return false;
        }

        /// <summary>
        /// 上传单个文件
        /// </summary>
        /// <param name="filedata">文件信息及stream</param>
        /// <returns></returns>
        public UpFileResult UpLoadFile(UpFile filedata)
        {
            UpFileResult result = new UpFileResult();

            string path = AppDomain.CurrentDomain.BaseDirectory + @"\parts\" + filedata.PartId;

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

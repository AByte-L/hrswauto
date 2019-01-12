using Gy.HrswAuto.DataMold;
using System.IO;
using System.ServiceModel;

namespace Gy.HrswAuto.ICmmServer
{
    [ServiceContract]
    public interface IPartConfigService
    {
        /// <summary>
        /// 上传测量零件需要用到的文件集
        /// </summary>
        /// <param name="filestream">文件内容</param>
        /// <returns></returns>
        [OperationContract]
        UpFileResult UpLoadFile(UpFile filestream);
        /// <summary>
        /// 上传测量零件需要用到的文件集
        /// </summary>
        /// <param name="filestream">文件内容</param>
        /// <param name="selPath">目录选择</param>
        /// <returns></returns>
        [OperationContract]
        UpFileResult UpLoadFile1(UpFile1 filestream);
        /// <summary>
        /// 到服务器端查找零件是否存在
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [OperationContract]
        bool FindPart(string partId);

        /// <summary>
        /// 在服务器中添加工件配置信息
        /// </summary>
        /// <param name="partConfig"></param>
        [OperationContract]
        void AddPartConfig(PartConfig partConfig);
        
        ////下载文件
        //[OperationContract]
        //DownFileResult DownLoadFile(DownFile downfile);
    }

    [MessageContract]
    public class UpFileResult
    {
        [MessageHeader]
        public bool IsSuccess { get; set; }
        [MessageHeader]
        public string Message { get; set; }
    }

    [MessageContract]
    public class UpFile
    {
        [MessageHeader]
        public long FileSize { get; set; }
        [MessageHeader]
        public string FileName { get; set; }
        [MessageHeader]
        public string FilePath { get; set; } // blades或者progs
        [MessageHeader]
        public string PartId { get; set; }
        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }

    [MessageContract]
    public class UpFile1
    {
        [MessageHeader]
        public long FileSize { get; set; }
        [MessageHeader]
        public int  selPath { get; set; } // blades或者progs
        [MessageHeader]
        public string FileName { get; set; }
        [MessageHeader]
        public string PartId { get; set; }
        [MessageBodyMember]
        public Stream FileStream { get; set; }
    }
    //[MessageContract]
    //public class DownFileResult
    //{
    //    [MessageHeader]
    //    public long FileSize { get; set; }
    //    [MessageHeader]
    //    public bool IsSuccess { get; set; }
    //    [MessageHeader]
    //    public string Message { get; set; }
    //    [MessageBodyMember]
    //    public Stream FileStream { get; set; }
    //}

    //[MessageContract]
    //public class DownFile
    //{
    //    [MessageHeader]
    //    public string FileName { get; set; }
    //}
}

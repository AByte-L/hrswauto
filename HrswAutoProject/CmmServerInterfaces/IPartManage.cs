using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.CmmServerInterfaces
{
    [ServiceContract]
    public interface IPartManage
    {
        /// <summary>
        /// 上传测量零件需要用到的文件集
        /// </summary>
        /// <param name="filestream">文件内容</param>
        /// <returns></returns>
        [OperationContract]
        UpFileResult UpLoadFile(UpFile filestream);

        /// <summary>
        /// 到服务器端查找零件是否存在
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [OperationContract]
        bool FindPart(string partId);

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

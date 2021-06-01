using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Content.Ftp
{
    public interface IFtpservice: IDisposable
    {
        bool IsConnected { get; }
        Task Connection();
        Task DisConnection();
        Task<FtpStatus> UploadFile(string localPath, string remotePath);
        Task<FtpStatus> DownLoadFile(string localPath, string remotePath);
        Task<long> DirectorySize(string remotePath);
        Task<bool> IsEqual(string localFile, string remoteFile);
        Task DeleteDirectory(string remotePath);
        Task DeleteFile(string filePath);
        Task<FtpListItem[]> DirectoryList(string target);
    }
}

using FluentFTP;
using Test.Content.Ftp;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Test.Content.Ftp
{
    public class FtpService : IFtpservice
    {
        protected readonly FtpClient client;
        public bool IsConnected => this.client.IsConnected;
        public FtpService(string host, string account, string pwd)
        {
            client = new FtpClient
            {
                Host = host,
                Credentials = new System.Net.NetworkCredential(account, pwd)
            };
        }
        public Task Connection()
        {
            return client.ConnectAsync();
        }
        public Task DisConnection()
        {
            return client.DisconnectAsync();
        }
        public virtual Task<FtpStatus> UploadFile(string localPath, string remotePath)
        {
            if (!IsConnected)
            {
                throw new Exception("Not Connected");
            }
            return client.UploadFileAsync(localPath, remotePath, FtpRemoteExists.Overwrite, true, FtpVerify.Retry);
        }
        public virtual Task<FtpStatus> DownLoadFile(string localPath, string remotePath)
        {
            if (!IsConnected)
            {
                throw new Exception("Not Connected");
            }
            return client.DownloadFileAsync(localPath, remotePath, FtpLocalExists.Overwrite, FtpVerify.Retry);
        }
        public virtual async Task<long> DirectorySize(string remotePath)
        {
            if (!IsConnected)
            {
                throw new Exception("Not Connected");
            }
            long n = 0;
            long size = await RecursionDirectorySize(n, remotePath);
            return size;
        }
        public virtual async Task<bool> IsEqual(string localFile, string remoteFile)
        {
            if (!IsConnected)
            {
                throw new Exception("Not Connected");
            }
            var local = new FileInfo(localFile).Length;
            var remote = await client.GetFileSizeAsync(remoteFile);
            return local == remote;
        }
        public virtual Task DeleteDirectory(string remotePath)
        {
            if (!IsConnected)
            {
                throw new Exception("Not Connected");
            }
            return client.DeleteDirectoryAsync(remotePath);
        }
        public virtual Task DeleteFile(string filePath)
        {
            if (!IsConnected)
            {
                throw new Exception("Not Connected");
            }
            return client.DeleteFileAsync(filePath);
        }
        public virtual Task<FtpListItem[]> DirectoryList(string target)
        {
            if (!IsConnected) throw new Exception("Not Connected");


            return this.client.GetListingAsync(target, FtpListOption.Recursive);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
        protected async Task<long> RecursionDirectorySize(long size, string path)
        {
            var list = await client.GetListingAsync(path);
            foreach (var item in list)
            {
                if (item.Type == FtpFileSystemObjectType.Directory)
                {
                    size += await RecursionDirectorySize(size, item.FullName);
                }
                else
                {
                    size += item.Size;
                }
            }
            return size;
        }


    }
}

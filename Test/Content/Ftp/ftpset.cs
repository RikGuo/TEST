//using FluentFTP;
//using MimeKit.IO;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Homework2021.Content.Ftp
//{
//    public class ftpset
//    {
//        static void Main(string[] args)
//        {
//            UploadFile().Wait();
//        }
//        private static FtpClient CreateFtpClient()
//        {
//            return new FtpClient("localhost", new System.Net.NetworkCredential { UserName = "test", Password = "test" });
//        }
//        private static async Task UploadFile()
//        {
//            const string FileToUpload = "C:\\Users\\rik guo\\Downloads\\新文字文件.txt";
//            using (FtpClient ftp = CreateFtpClient())
//            {
//                using (FileStream fs = File.OpenRead(FileToUpload))
//                {
//                    await ftp.UploadAsync(fs, Path.GetFileName(FileToUpload));
//                }
//            }
//        }
//        private static async Task FtpStuff()
//        {
//            using (FtpClient ftp = new FtpClient("localhost", new System.Net.NetworkCredential { UserName = "test", Password = "test" }))
//            {
//                FtpListItem[] listing = await ftp.GetListingAsync();
//                foreach (FtpListItem ftpitem in listing)
//                {
//                    if (ftpitem.Type != FtpFileSystemObjectType.File)
//                        continue;
//                    using (MemoryStream ms = new MemoryStream())
//                    {
//                        await ftp.DownloadAsync(ms, ftpitem.Name);
//                        ms.Position = 0;
//                        using (StreamReader sr = new StreamReader(ms))
//                        {
//                            string fileContents = await sr.ReadToEndAsync();
//                            if (string.IsNullOrEmpty(fileContents))
//                                throw new Exception("資料夾無資料");
//                        }
//                        await ftp.MoveFileAsync(ftpitem.Name, Path.Combine("Processed", ftpitem.Name));
//                    }
//                }
//            }
//        }
//    }
//}

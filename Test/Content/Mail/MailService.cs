using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;
using Test.Content;
using Test.Logic.Interface;
using Test.Model;

namespace Test.Content.Mail
{
    public class MailService : IMailService
    {
        EF_MailSettings _mailSettings = null;
        public MailService(IOptions<EF_MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public bool SendEmailAsync(RS_MailRequest mailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                smtp.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Nlogger.WriteLog(Nlogger.NType.Error, ex.Message, ex);
                //Log Exception Details
                return false;
            }
        }

        public RS_ModifyResult SendEmailAsync(bool sendemail)
        {
            throw new NotImplementedException();
        }
    }
}

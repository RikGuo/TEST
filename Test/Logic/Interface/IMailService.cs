using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Logic.Interface
{
    public interface IMailService
    {
        bool SendEmailAsync(RS_MailRequest mailRequest);
        RS_ModifyResult SendEmailAsync(bool sendemail);
    }
}

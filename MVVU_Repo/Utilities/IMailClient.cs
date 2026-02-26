using MVVU_Model;
using MVVU_Data.Entities;
using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MVVU_Repo.Utilities
{
    public interface IMailClient
    {
        

        bool SendMail(string to, string subject, string body);
        bool SendMulpMail(string to, string subject, string body);
        

    }
}
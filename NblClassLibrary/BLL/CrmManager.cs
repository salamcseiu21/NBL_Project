using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.BLL
{
    class CrmManager
    {
        public bool SendMail(MailMessage mailMessage)
        {
            using (var smtp = new SmtpClient())
            {

                smtp.Send(mailMessage);
                return true;
            }
        }
    }
}

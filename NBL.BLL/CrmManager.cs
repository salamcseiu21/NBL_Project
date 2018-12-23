using System.Net.Mail;

namespace NBL.BLL
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

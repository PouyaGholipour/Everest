using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using System.Web;

namespace TopLearn.Core.Senders
{
    public class SendEmail
    {
        //public static void Send(string to, string subject, string body)
        //{
        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        //    mail.From = new MailAddress("pouya.goliour1376@gmail.com", "تاپ لرن");
        //    mail.To.Add(to);
        //    mail.Subject = subject;
        //    mail.Body = body;
        //    mail.IsBodyHtml = true;
        //    SmtpServer.EnableSsl = true;

        //    //System.Net.Mail.Attachment attachment;
        //    //attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
        //    //mail.Attachments.Add(attachment);

        //    SmtpServer.Port = 587;
        //    //SmtpServer.UseDefaultCredentials = false;

        //    //SmtpServer.Credentials = new System.Net.NetworkCredential("TopLearn.com@gmail.com", "****");
        //    SmtpServer.Credentials = new System.Net.NetworkCredential("pouya.goliour1376@gmail.com", "pg1362222321");
        //    SmtpServer.EnableSsl = true;

        //    SmtpServer.Send(mail);

        //}




        public static async Task Send(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("Pouya.goliour1376@gmail.com", "KopolArt | کپـل آرت");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            smtpServer.UseDefaultCredentials = false;
            smtpServer.Port = 645;
            smtpServer.Credentials = new System.Net.NetworkCredential("Pouya.goliour1376@gmail.com", "1362222321");
            smtpServer.EnableSsl = true;
            await smtpServer.SendMailAsync(mail);
            smtpServer.Dispose();
        }





        //private readonly EmailSetting _emailSetting;

        //public SendEmail(IOptions<EmailSetting> emailSetting)
        //{
        //    _emailSetting = emailSetting.Value;
        //}

        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    try
        //    {
        //        // Credentials
        //        var credentials = new NetworkCredential(_emailSetting.Sender, _emailSetting.Password);

        //        // Mail message
        //        var mail = new MailMessage
        //        {
        //            From = new MailAddress(_emailSetting.Sender, _emailSetting.SenderName),
        //            Subject = subject,
        //            Body = message,
        //            IsBodyHtml = true
        //        };

        //        mail.To.Add(new MailAddress(email));

        //        // Smtp client
        //        var client = new SmtpClient
        //        {
        //            Port = _emailSetting.MailPort,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false,
        //            Host = _emailSetting.MailServer,
        //            EnableSsl = _emailSetting.Sender.Contains("gmail"),
        //            Credentials = credentials
        //        };

        //        // Send it...         
        //        await client.SendMailAsync(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: handle exception

        //        throw new InvalidOperationException(ex.Message);
        //    }
        //}
    }
}
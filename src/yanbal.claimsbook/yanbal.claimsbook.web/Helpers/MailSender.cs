using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace yanbal.claimsbook.web.Helpers
{
    public class MailSender
    {
        #region Properties
        public string Host { get; set; }
        public string Port { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentPath { get; set; }
        public string AttachmentContentId { get; set; }
        #endregion

        public void Send()
        {
            try
            {
                string htmlBody = Body;
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                Logger.Write("Punto 1");

                // Create a LinkedResource object for each embedded image 
                LinkedResource pic1 = new LinkedResource(AttachmentPath, MediaTypeNames.Image.Jpeg);

                var img = System.IO.File.ReadAllText(AttachmentPath);
                Logger.Write(img != null ? "img leída correctamente" : string.Empty);

                pic1.ContentId = AttachmentContentId;
                avHtml.LinkedResources.Add(pic1);

                Logger.Write("Punto 2");

                // Add the alternate views instead of using MailMessage.Body 
                MailMessage m = new MailMessage();
                m.AlternateViews.Add(avHtml);

                Logger.Write("Punto 3");

                // Address and send the message 
                m.From = new MailAddress(From, Name);
                To.Split(',').ToList().ForEach(x => m.To.Add(new MailAddress(x)));
                if (!string.IsNullOrEmpty(Cc)) Cc.Split(',').ToList().ForEach(x => m.CC.Add(new MailAddress(x)));
                m.Subject = Subject;
                SmtpClient client = new SmtpClient(Host, int.Parse(Port));

                Logger.Write("Se preparó el objeto correctamente");

                client.Send(m);

                Logger.Write("Se envió el mensaje correctamente");

            }
            catch (Exception ex)
            {
                Logger.Write(string.Format("Error (client.Send): {0}", ex.Message));
                throw ex;
            }
        }
    }
}

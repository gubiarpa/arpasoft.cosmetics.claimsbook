using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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

        public void Send(string urlPdf, string storageFile, string logPath = null)
        {
            try
            {
                string htmlBody = Body;
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                Logger.Write(logPath, "Punto 1");

                // Create a LinkedResource object for each embedded image 
                LinkedResource pic1 = new LinkedResource(AttachmentPath, MediaTypeNames.Image.Jpeg);

                var img = System.IO.File.ReadAllText(AttachmentPath);
                Logger.Write(logPath, img != null ? "img leída correctamente" : string.Empty);

                pic1.ContentId = AttachmentContentId;
                avHtml.LinkedResources.Add(pic1);

                Logger.Write(logPath, "Punto 2");

                // Add the alternate views instead of using MailMessage.Body 
                MailMessage m = new MailMessage();
                m.AlternateViews.Add(avHtml);

                // Attachment File
                m.AddPdfUrl(urlPdf, storageFile);

                Logger.Write(logPath, "Punto 3");

                // Address and send the message 
                m.From = new MailAddress(From, Name);
                To.Split(',').ToList().ForEach(x => m.To.Add(new MailAddress(x)));
                if (!string.IsNullOrEmpty(Cc)) Cc.Split(',').ToList().ForEach(x => m.CC.Add(new MailAddress(x)));
                if (!string.IsNullOrEmpty(Bcc)) Bcc.Split(',').ToList().ForEach(x => m.Bcc.Add(new MailAddress(x)));
                m.Subject = Subject;
                SmtpClient client = new SmtpClient(Host, int.Parse(Port));

                Logger.Write(logPath, "Se preparó el objeto correctamente");
                client.Send(m);
                Logger.Write(logPath, "Se envió el mensaje correctamente");

            }
            catch (Exception ex)
            {
                Logger.Write(logPath, string.Format("Error (client.Send): {0}", ex.Message));
                throw ex;
            }
        }
    }
}

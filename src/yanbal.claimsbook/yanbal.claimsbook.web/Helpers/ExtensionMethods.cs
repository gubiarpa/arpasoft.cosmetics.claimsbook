using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace yanbal.claimsbook.web.Helpers
{
    public static class ExtensionMethods
    {
        public static string Stringify(this object obj, bool indented = false, bool camelCase = true)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = indented ? Formatting.Indented : Formatting.None,
                    ContractResolver = camelCase ? new CamelCasePropertyNamesContractResolver() : null
                };
                return JsonConvert.SerializeObject(obj, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Stream GenerateStream(this string s)
        {
            var stream = new System.IO.MemoryStream();
            var writer = new System.IO.StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream GenerateStream(this byte[] b)
        {
            return GenerateStream(System.Text.Encoding.UTF8.GetString(b));
        }

        public static void AddPdfUrl(this MailMessage m, string urlPdf, string storagePath)
        {
            using (WebClient webClient = new WebClient())
            {
                File.WriteAllBytes(file, data); // saves the file in 'storagePath'
                var data = webClient.DownloadData(urlPdf.Replace('\\', '/'));
                File.WriteAllBytes(storagePath, data);
                m.Attachments.Add(new Attachment(storagePath)); // attaches the file
            }
        }
    }
}

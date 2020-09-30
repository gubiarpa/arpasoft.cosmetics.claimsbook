using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yanbal.claimsbook.web.Helpers
{
    public static class Logger
    {
        public static void Write(string message)
        {
            try
            {
                System.IO.File.AppendAllText(
                            "D:\\Logs\\LibroReclamaciones\\LogLibroReclamaciones.log",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + '\t' + message + '\n');
            }
            catch
            {
            }
        }
    }
}

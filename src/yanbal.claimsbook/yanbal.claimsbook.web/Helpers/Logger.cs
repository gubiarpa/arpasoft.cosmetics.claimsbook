using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yanbal.claimsbook.web.Helpers
{
    public static class Logger
    {
        public static void Write(string path, string message)
        {
            try
            {
                System.IO.File.AppendAllText(path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + '\t' + message + '\n');
            }
            catch
            {
            }
        }
    }
}

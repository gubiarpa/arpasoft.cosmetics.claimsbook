using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yanbal.claimsbook.web.Helpers
{
    public static class ExtensionMethods
    {
        public static string Stringify(this object obj, bool indented = false, bool camelCase = true)
        {
            try
            {
                //JsonSerializerSettings settings;

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
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreServices.Utility.Extension
{
    public static class StringExtension
    {
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if((text.StartsWith("{") && (text.EndsWith("}"))) || (text.StartsWith("[") && (text.EndsWith("]"))))
            {
                try
                {
                    var obj = JToken.Parse(text);
                    return true;
                }
                catch(JsonReaderException)
                {
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

    }
}

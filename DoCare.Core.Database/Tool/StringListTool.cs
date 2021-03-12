using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoCare.Core.Database.Tool
{
    public static class StringListTool
    {
        public static List<string> DoCareToList(this string src, char separator = ',')
        {
            var result = !string.IsNullOrEmpty(src) ? src.Split(separator).ToList() : new List<string>();

            return result.Where(t => !string.IsNullOrEmpty(t)).ToList();

        }

        public static string DoCareToString(this List<string> src, string separator = ",", bool needWrap = false)
        {
            if (src == null || src.Count == 0)
            {
                return "";
            }
            else
            {
               var result = string.Join(separator, src);
               if (needWrap)
               {
                   return $"{separator}{result}{separator}";
               }

               return result;

            }

        }
    }
}

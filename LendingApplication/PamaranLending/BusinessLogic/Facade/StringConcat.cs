using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace BusinessLogic
{
    public class StringConcatUtility
    {
        public static string Build(string separtor, params string[] parts)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string part in parts)
            {
                if (string.IsNullOrWhiteSpace(part) == false)
                {
                    builder.Append(part + separtor);
                }
            }

            builder.Remove(builder.Length - separtor.Length, separtor.Length);

            return builder.ToString();
        }
    }
}
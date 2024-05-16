using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    /**
    Defense in Depth:
        Defense in depth is a security principle where multiple layers of security controls are implemented to protect data. Even if the data is sanitized before saving, re-sanitizing it upon retrieval provides an additional layer of security, which can protect against unforeseen issues or mistakes.

    Data Integrity Assurance:
        Over time, the database might accumulate data from various sources. If there’s ever a breach or a bug that allows unsanitized content to be saved, sanitizing on retrieval helps ensure that only safe content is rendered.

    Consistency:
        Changes in the sanitization logic or updates to the sanitizer library might mean that older content in the database doesn’t comply with newer security standards. Re-sanitizing ensures consistency with the latest security practices.

    Untrusted Sources:
        If data might be imported from or synced with external, untrusted sources, additional sanitization ensures that any potentially unsafe HTML is caught before rendering.
     **/
    public static class SanitizationHelper
    {
        /// <summary>
        ///  Sanitizes an object using reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static void SanitizeObject<T>(T obj)
        {
            if (obj == null)
            {
                return;
            }

            HtmlSanitizer sanitizer = new HtmlSanitizer();
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach(var property in properties) 
            {
                if (property.PropertyType == typeof(string) && property.CanRead && property.CanWrite)
                {
                    string currentValue = (string)property.GetValue(obj);
                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        string sanitizedValue = sanitizer.Sanitize(currentValue);
                        property.SetValue(obj, sanitizedValue);
                    }
                }
            }
        }
    }
}

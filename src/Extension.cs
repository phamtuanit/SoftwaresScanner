using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwaresAnalyze
{
    using Microsoft.Win32;

    public static class Extension
    {
        /// <summary>
        /// Gets the value ex.
        /// </summary>
        /// <param name="reKey">The re key.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetValueEx(this RegistryKey reKey, string valueName, string defaultValue = null)
        {
            try
            {
                return reKey?.GetValue(valueName).ToString();
            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(defaultValue))
                {
                    return defaultValue;
                }

                return string.Empty;
            }
        }
    }
}

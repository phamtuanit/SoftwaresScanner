#region Reference

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;
// ReSharper disable StringIndexOfIsCultureSpecific.1

#endregion

namespace SoftwaresAnalyze
{
    using System.Windows.Media.Animation;

    internal class DataProvider
    {
        /// <summary>
        /// The default path
        /// </summary>
        private const string PATHDEFAULT = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        /// <summary>
        /// The path x86
        /// </summary>
        private const string PATHX86 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

        /// <summary>
        ///     Gets the data.
        /// </summary>
        /// <returns></returns>
        public List<AppModel> GetData()
        {
            // Try get data
            var finalResult1 = this.GetSWListFromUninstall(PATHDEFAULT, AppType.X64);

            // If 64bit OS
            var finalResult2 = this.GetSWListFromUninstall(PATHX86, AppType.X86);
            
            return this.MergeLists(finalResult1, finalResult2);
        }

        /// <summary>
        ///     Merge2s the string.
        /// </summary>
        /// <param name="inAndOut">The in and out.</param>
        /// <param name="finalResult2">The final result2.</param>
        protected List<AppModel> MergeLists(List<AppModel> inAndOut, List<AppModel> finalResult2)
        {
            foreach (var app in finalResult2)
            {
                var existedAp = inAndOut.FirstOrDefault(app1 => app1.DisplayName == app.DisplayName);
                if (existedAp == null)
                {
                    inAndOut.Add(app);
                }
            }

            // Sorting
            return (from element in inAndOut
                    orderby element.DisplayName
                    select element).ToList();
        }

        /// <summary>
        /// Gets the sw list from un-install.
        /// </summary>
        /// <param name="keyPath">The key path.</param>
        /// <param name="pathType">Type of the path.</param>
        /// <returns></returns>
        protected List<AppModel> GetSWListFromUninstall(string keyPath, AppType pathType)
        {
            var result = new List<AppModel>();

            var uninstallKey = Registry.LocalMachine.OpenSubKey(keyPath, true);
            var subKeyNames = uninstallKey?.GetSubKeyNames().ToList();
            subKeyNames = (from element in subKeyNames
                           orderby element
                           select element).ToList();

            foreach (var subRegistry in subKeyNames)
            {
                var registryKey = uninstallKey.OpenSubKey(subRegistry, true);
                try
                {
                    string displayName = registryKey.GetValueEx("DisplayName");
                    if (!string.IsNullOrWhiteSpace(displayName))
                    {
                        var publisher = registryKey.GetValueEx("Publisher");

                        result.Add(new AppModel
                        {
                            DisplayName = displayName.Trim(),
                            DisplayVersion = registryKey.GetValueEx("DisplayVersion"),
                            Publisher = publisher,
                            RegistryKey = registryKey,
                            AppType = pathType
                        });
                    }
                }
                catch
                {
                }
            }

            uninstallKey?.Close();

            // Sorting
            return result;
        }
    }
}
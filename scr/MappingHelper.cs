using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwaresAnalyze
{
    using System.IO;

    public class MappingHelper
    {
        /// <summary>
        /// The ignore key
        /// </summary>
        const string IgnoreKey = "//";

        /// <summary>
        /// The Separator
        /// </summary>
        const char Separator = '=';

        /// <summary>
        /// The default mapping file name
        /// </summary>
        const string DefaultMappingFileName = @"Mapping.data";

        /// <summary>
        /// The mapping dictionary
        /// </summary>
        private Dictionary<string, Mapping> MappingDict = new Dictionary<string, Mapping>();

        /// <summary>
        /// Gets the <see cref="Mapping"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="Mapping"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public Mapping this[string key]
        {
            get
            {
                if (this.MappingDict.ContainsKey(key))
                {
                    return this.MappingDict[key];
                }

                return null;
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = DefaultMappingFileName;
            }

            using (var writer = new StreamWriter(fileName, false))
            {
                foreach (var mapping in this.MappingDict)
                {
                    writer.WriteLine($"{mapping.Value.Native}{Separator}{mapping.Value.Map}");
                }
            }
        }

        /// <summary>
        /// Adds the specified native name.
        /// </summary>
        /// <param name="nativeName">Name of the native.</param>
        /// <param name="newName">The new name.</param>
        /// <returns></returns>
        public bool Add(string nativeName, string newName = null)
        {
            if (newName == null)
            {
                newName = this.BuildNewMappingName(nativeName);
            }

            if (this.MappingDict.ContainsKey(nativeName))
            {
                return false;
            }

            this.MappingDict.Add(nativeName, new Mapping
            {
                Native = nativeName,
                Map = newName
            });

            return true;
        }

        /// <summary>
        /// Builds the new name of the mapping.
        /// </summary>
        /// <param name="nativeName">Name of the native.</param>
        /// <returns></returns>
        private string BuildNewMappingName(string nativeName)
        {
            var rand = new Random();
            var shortName = this.BuildShortName(nativeName);
            return $"Notepad++ v{rand.Next()%2}.{rand.Next()%3}.{rand.Next()%4}.{rand.Next()%5} {shortName}";
        }

        /// <summary>
        /// Builds the short name.
        /// </summary>
        /// <param name="nativeName">Name of the native.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string BuildShortName(string nativeName)
        {
            var names = nativeName.Split(' ');
            return names.Aggregate(string.Empty, (current, name) => current + name[0].ToString().ToUpper());
        }

        /// <summary>
        /// Loads the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void Load(string fileName = null)
        {

            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = DefaultMappingFileName;
            }

            if (!File.Exists(fileName))
            {
                throw new AggregateException($"The [{fileName}] is not existed.");
            }

            // Read line by line
            this.MappingDict.Clear();
            using (var reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(IgnoreKey) &&
                        line.Contains(Separator))
                    {
                        var keyValue = line.Split(Separator);
                        var key = keyValue[0].Trim();
                        var value = string.Empty;
                        if (line.Length >= 2)
                        {
                            value = keyValue[1].Trim();
                        }

                        this.MappingDict.Add(key, new Mapping
                        {
                            Native = key,
                            Map = value
                        });
                    }
                }
            }
        }
    }
}

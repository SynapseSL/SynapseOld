using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Synapse
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Translation
    {
        private Dictionary<string, string> _translation = new Dictionary<string, string>();
        private string _translationPath;
        internal Plugin Plugin;

        public void CreateTranslations(Dictionary<string,string> translations)
        {
            _translationPath = Path.Combine(PluginManager.ServerConfigDirectory, Plugin.GetName + "-translation.txt");
            if (!File.Exists(_translationPath))
                File.Create(_translationPath).Close();
            var dictionary = new Dictionary<string, string>();
            var lines = File.ReadAllLines(_translationPath);
            var newlines = new List<string>();
            var position = 0;

            foreach (var rawpair in translations.ToList())
            {
                var pair = new KeyValuePair<string,string>(rawpair.Key,rawpair.Value.Replace("\n", "\\n"));

                if (lines.Length > position)
                {
                    if (string.IsNullOrEmpty(lines[position]))
                    {
                        dictionary.Add(pair.Key, pair.Value);
                        newlines.Add(pair.Value);
                    }
                    else
                    {
                        dictionary.Add(pair.Key, lines[position]);
                        newlines.Add(lines[position]);
                    }
                }
                else
                {
                    dictionary.Add(pair.Key, pair.Value);
                    newlines.Add(pair.Value);
                }

                position++;
                File.WriteAllLines(_translationPath, newlines.ToArray());
            }

            _translation = dictionary;
        }

        public string GetTranslation(string translationName)
        {
            try
            {
                var trans = _translation.FirstOrDefault(x => x.Key == translationName).Value;
                return trans.Replace("\\n","\n");
            }
            catch
            {
                return "Invalid Translations Name";
            }
        }
    }
}

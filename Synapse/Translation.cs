using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Synapse
{
    public class Translation
    {
        private Dictionary<string, string> translation = new Dictionary<string, string>();
        private string translationpath;
        internal Plugin plugin;

        public void CreateTranslations(Dictionary<string,string> translations)
        {
            translationpath = Path.Combine(PluginManager.ServerConfigDirectory, plugin.GetName + "-translation.txt");
            if (!File.Exists(translationpath))
                File.Create(translationpath).Close();
            var dictionary = new Dictionary<string, string>();
            string[] lines = File.ReadAllLines(translationpath);
            List<string> newlines = new List<string>();
            int position = 0;

            foreach (var pair in translations.ToList())
            {
                if (lines.Count() > position)
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
                File.WriteAllLines(translationpath, newlines.ToArray());
            }

            translation = dictionary;
        }

        public string GetTranslation(string translationName)
        {
            try
            {
                string trans = translation.FirstOrDefault(x => x.Key == translationName).Value;
                return trans;
            }
            catch
            {
                return $"Invalid Translations Name";
            }
        }
    }
}

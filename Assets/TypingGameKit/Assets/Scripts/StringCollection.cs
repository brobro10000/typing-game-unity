using System.Collections.Generic;
using System.Linq;
using TypingGameKit.Util;
using UnityEngine;

namespace TypingGameKit
{
    /// <summary>
    /// StringCollection provides a way to aggregate text strings.
    /// </summary>
    [CreateAssetMenu(fileName = "New String Collection", menuName = "Create a New String Collection")]
    public class StringCollection : ScriptableObject
    {
        private Dictionary<char, List<string>> stringDict;

        [SerializeField]
        [Tooltip("An array of text files. Each line in each text file is parsed as a string.")]
        private TextAsset[] textSources = new TextAsset[] { };

        private string[] strings;

        /// <summary>
        /// Dictionary that maps leading chars in lower case to corresponding strings in the StringCollection.
        /// </summary>
        public Dictionary<char, List<string>> StringDict
        {
            get { return stringDict; }
        }

        /// <summary>
        /// Array containing all strings in the collection.
        /// </summary>
        public string[] Strings
        {
            get { return strings; }
        }

        /// <summary>
        /// Reevaluates the strings and dictionary.
        /// </summary>
        public void EvaluateStrings()
        {
            HashSet<string> stringSet = new HashSet<string>();
            foreach (var listData in textSources.Where(t => t != null).Select(t => t.text))
            {
                stringSet.UnionWith(ParseListData(listData));
            }
            strings = stringSet.ToArray();

            SetupStringDict();
        }

        /// <summary>
        /// Picks a string from the collection randomly.
        /// </summary>
        public string PickRandomString()
        {
            return strings.PickRandom();
        }

        private static IEnumerable<string> ParseListData(string listData)
        {
            return listData
                .Split('\n')
                .Select(w => w.Trim())
                .Where(s => s.Length > 0);
        }

        private void OnEnable()
        {
            EvaluateStrings();
        }

        private void OnValidate()
        {
            EvaluateStrings();
        }

        private void SetupStringDict()
        {
            stringDict = new Dictionary<char, List<string>>();
            foreach (var str in strings)
            {
                var key = str.ToLower()[0];
                if (stringDict.ContainsKey(key))
                {
                    stringDict[key].Add(str);
                }
                else
                {
                    stringDict[key] = new List<string>() { str };
                }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TypingGameKit
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(StringCollection))]
    public class StringCollectionEditor : Editor
    {
        private StringCollection stringCollection;
        private string randomSample = "";
        private int sampleLength = 16;
        private string leadingChars;

        private void OnEnable()
        {
            stringCollection = (StringCollection)target;
            UpdateExamples();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Unique leading chars: " + stringCollection.StringDict.Count);
            EditorGUILayout.LabelField(leadingChars);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Sample:");
            EditorGUILayout.TextArea(randomSample);
            if (GUILayout.Button("Update random sample"))
            {
                UpdateExamples();
            }

            if (GUI.changed)
            {
                UpdateExamples();
            }
        }

        private void UpdateExamples()
        {
            stringCollection.EvaluateStrings();
            UpdateSample();
            UpdateLeadingChars();
        }

        private void UpdateLeadingChars()
        {
            leadingChars = string.Join(" ", stringCollection
                .StringDict.Keys
                .OrderBy(c => c)
                .Select(c => c.ToString())
                .ToArray()
                );
        }

        private void UpdateSample()
        {
            var strings = new List<string>();

            for (int i = 0; i < sampleLength; i++)
            {
                strings.Add(stringCollection.PickRandomString());
            }

            randomSample = string.Join("\n", strings.ToArray());
        }
    }
}
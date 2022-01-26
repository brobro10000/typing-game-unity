using UnityEditor;
using UnityEngine;

namespace TypingGameKit
{
    public class VirtualKeyboard : MonoBehaviour
    {
        [SerializeField] private bool scaleToMinimalWidth = true;
        [SerializeField] private VirtualKeyboardKey keyPrefab = null;
        [SerializeField] private string keysToGenerate = null;

#if UNITY_EDITOR

        [ContextMenu("Generate Keys")]
        private void GenerateKeys()
        {
            foreach (var key in keysToGenerate)
            {
                var buttonObj = (VirtualKeyboardKey)PrefabUtility.InstantiatePrefab(keyPrefab, transform);
                buttonObj.Initialize(this, key);
            }
        }

#endif

        private void LateUpdate()
        {
            UpdateScale();
        }

        private void UpdateScale()
        {
            if (scaleToMinimalWidth == false)
            {
                return;
            }

            float width = ((RectTransform)transform).rect.width;
            if (width == 0)
            {
                return;
            }
            float shortestSide = Mathf.Min(Screen.width, Screen.height);
            transform.localScale = Vector3.one * shortestSide / width;
        }

        public void AddInput(string input)
        {
            SequenceManager.AddInput(input);
        }

        public void ToggleKeyboard()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
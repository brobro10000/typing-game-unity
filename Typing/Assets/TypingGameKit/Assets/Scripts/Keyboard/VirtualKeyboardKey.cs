using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TypingGameKit
{
    public class VirtualKeyboardKey : Button
    {
        [SerializeField] private VirtualKeyboard keyboard;
        [SerializeField] private char key;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            keyboard.AddInput(key.ToString());
        }

        internal void Initialize(VirtualKeyboard virtualKeyboard, char key)
        {
            keyboard = virtualKeyboard;
            this.key = key;
            name = $"Key: {key}";
            GetComponentInChildren<Text>().text = key.ToString();
        }
    }
}
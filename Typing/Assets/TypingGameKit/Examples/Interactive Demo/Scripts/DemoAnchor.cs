using TypingGameKit.Util;
using UnityEngine;

namespace TypingGameKit.Demo
{
    public class DemoAnchor : MonoBehaviour
    {
        [SerializeField] Color[] colors = null;

        static int spawnCount = 0;

        private void Start()
        {
            var renderer = GetComponent<MeshRenderer>();
            renderer.material.color = colors[spawnCount % colors.Length];
            spawnCount += 1;
        }
    }
}
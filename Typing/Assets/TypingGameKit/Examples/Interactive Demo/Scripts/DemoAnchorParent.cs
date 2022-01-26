using UnityEngine;

namespace TypingGameKit.Demo
{
    public class DemoAnchorParent : MonoBehaviour
    {
        [SerializeField] private float angleRotatedPerSecond = 10;

        public float AngleRotatedPerSecond
        {
            get { return angleRotatedPerSecond; }
            set { angleRotatedPerSecond = value; }
        }

        private void Update()
        {
            transform.RotateAround(Vector3.zero, Vector3.up, Time.deltaTime * angleRotatedPerSecond);
        }
    }
}
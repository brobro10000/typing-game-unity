using UnityEngine;

namespace TypingGameKit.AsteroidTyping
{
    /// <summary>
    /// Asteroid that threatens the player ship
    /// </summary>
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float maxRotation = 0.1f;
        [SerializeField] private float velocity = 0.1f;
        [SerializeField] private GameObject remains = null;

        /// <summary>
        /// Makes the asteroid blow up.
        /// </summary>
        public void Explode()
        {
            Destroy(gameObject);
            SpawnRemains();
        }

        /// <summary>
        /// Pushes the asteroid toward a target.
        /// </summary>
        public void PushTowards(Vector3 target)
        {
            Vector2 direction = (target - transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = direction * velocity;
        }

        private void Start()
        {
            SetRandomAngularVelocity();
        }

        private void SetRandomAngularVelocity()
        {
            GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-maxRotation, maxRotation);
        }

        private void SpawnRemains()
        {
            Instantiate(remains, transform.position, transform.rotation);
        }
    }
}
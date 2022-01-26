using UnityEngine;

namespace TypingGameKit.AsteroidTyping
{
    /// <summary>
    /// Player ship that shoots at asteroids.
    /// </summary>
    public class PlayerShip : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField] private float laserDuration = 0.1f;
        [SerializeField] private float laserRandomSpread = 1f;
        [SerializeField] private LineRenderer laserRenderer = null;
        [SerializeField] private AudioClip laserSound = null;

        [SerializeField] private GameObject remains = null;

        private float lastShotTime;

        /// <summary>
        /// Blows up the ship.
        /// </summary>
        public void Explode()
        {
            gameObject.SetActive(false);
            Instantiate(remains, transform.position, transform.rotation);
        }

        /// <summary>
        /// Shoots at the given asteroid
        /// </summary>
        public void FireAt(Asteroid asteroid)
        {
            lastShotTime = Time.time;
            Vector3 target = asteroid.transform.position;

            LookAtPosition(target);
            DisplayLaser(transform.position, target);
            audioSource.PlayOneShot(laserSound);
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            FindObjectOfType<GameManager>().GameOver();
        }

        private void DisplayLaser(Vector3 pos, Vector3 targetPos)
        {
            laserRenderer.enabled = true;
            Vector3 randomSpread = Random.insideUnitCircle * laserRandomSpread;
            laserRenderer.SetPositions(new Vector3[] { pos, targetPos + randomSpread });
        }

        private void Update()
        {
            if (laserRenderer.enabled && Time.time - lastShotTime > laserDuration)
            {
                laserRenderer.enabled = false;
            }
        }

        private void LookAtPosition(Vector3 target)
        {
            transform.up = target - transform.position;
        }
    }
}
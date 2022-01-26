using TypingGameKit.Util;
using UnityEngine;

namespace TypingGameKit.AsteroidTyping
{
    /// <summary>
    /// Manages the 2D example game
    /// </summary>
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerShip player = null;
        [SerializeField] private SequenceManager sequenceManager = null;
        [SerializeField] private StringCollection words = null;

        [SerializeField] private Asteroid asteroidPrefab = null;
        [SerializeField] private float spawnRadius = 1f;
        [SerializeField] private float wordsPerMinute = 30;
        [SerializeField] private float wpmIncreasePerSecond = 0.5f;

        private float lastSpawned = 0;

        private void Update()
        {
            if (GameManager.Instance.IsRunning)
            {
                UpdateSpawning();
            }
        }

        private void UpdateSpawning()
        {
            // spawn asteroids at the current wpm pace
            if (Time.time - lastSpawned > 60 / wordsPerMinute)
            {
                lastSpawned = Time.time;
                SpawnAsteroidWithSequence();
            }

            // increase words spawned per minute over time
            wordsPerMinute += Time.deltaTime * wpmIncreasePerSecond;
        }

        private void SpawnAsteroidWithSequence()
        {
            AttachSequenceToAsteroid(SpawnAsteroid());
        }

        private Asteroid SpawnAsteroid()
        {
            Asteroid asteroid = Instantiate(asteroidPrefab, transform);
            asteroid.transform.position = RandomInitialAsteroidPosition();
            asteroid.PushTowards(player.transform.position);
            return asteroid;
        }

        private void AttachSequenceToAsteroid(Asteroid asteroid)
        {
            var sequence = sequenceManager.CreateSequence(GetNewSequenceText(), asteroid.transform);
            sequence.OnInputSucceeded += delegate { player.FireAt(asteroid); };
            sequence.OnCompleted += delegate { asteroid.Explode(); };
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }

        private string GetNewSequenceText()
        {
            return sequenceManager.GetUniquelyTargetableString(words) ?? words.Strings.PickRandom();
        }

        private Vector3 RandomInitialAsteroidPosition()
        {
            float value = Random.Range(0, Mathf.PI * 2);
            return transform.position + new Vector3(Mathf.Cos(value), Mathf.Sin(value)) * spawnRadius;
        }
    }
}
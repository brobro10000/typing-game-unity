using UnityEngine;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Manages the visuals and sound of firing the players laser gun.
    /// </summary>
    public class LaserGun : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField] private AudioClip fireSound = null;

        [SerializeField] private LineRenderer laserRenderer = null;
        [SerializeField] private Light laserLight = null;
        [SerializeField] private float laserDuration = 0.1f;
        [SerializeField] private float randomSpread = 0.1f;

        private float lastShot;

        public void FireAt(Vector3 targetPos)
        {
            lastShot = Time.time;
            DisplayLaser(targetPos);
            PlayFireSound();
        }

        private void DisplayLaser(Vector3 targetPos)
        {
            laserRenderer.enabled = true;
            laserLight.enabled = true;
            var spread = Random.insideUnitSphere * randomSpread;
            laserRenderer.SetPositions(new Vector3[] { transform.position, targetPos + spread });
        }

        private void PlayFireSound()
        {
            audioSource.clip = fireSound;
            audioSource.Play();
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (laserRenderer.enabled && Time.time - lastShot > laserDuration)
            {
                laserRenderer.enabled = false;
                laserLight.enabled = false;
            }
        }
    }
}
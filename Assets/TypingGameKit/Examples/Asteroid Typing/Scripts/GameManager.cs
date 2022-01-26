using System.Collections;
using UnityEngine;

namespace TypingGameKit.AsteroidTyping
{
    /// <summary>
    /// Manages the 2D example game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerShip player = null;
        [SerializeField] private SequenceManager sequenceManager = null;

        [SerializeField] private ResultsPanel resultsMenu = null;
        [SerializeField] private float resultsMenuDelay = 2f;

        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private AudioClip inputErrorSound = null;

        public static GameManager Instance { get; private set; }
        public bool IsRunning { get; private set; }

        /// <summary>
        /// To be called when the game is over.
        /// </summary>
        public void GameOver()
        {
            sequenceManager.RemoveAllSequences();
            player.Explode();
            IsRunning = false;
            StartCoroutine(ShowResultsMenu());
        }

        private void Awake()
        {
            Instance = this;
            IsRunning = true;
            Unpause();
            sequenceManager.OnInputFailed += delegate { audioSource.PlayOneShot(inputErrorSound); };
            sequenceManager.OnSelectionFailed += delegate { audioSource.PlayOneShot(inputErrorSound); };
        }

        private IEnumerator ShowResultsMenu()
        {
            yield return new WaitForSeconds(resultsMenuDelay);
            Pause();
            resultsMenu.gameObject.SetActive(true);
        }

        private void Unpause()
        {
            Time.timeScale = 1;
        }

        private void Pause()
        {
            Time.timeScale = 0;
        }
    }
}
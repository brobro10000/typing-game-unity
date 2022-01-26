using UnityEngine;
using UnityEngine.SceneManagement;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Manages the game.
    /// </summary>
    public partial class GameManager : MonoBehaviour
    {
        [SerializeField] private SequenceManager sequenceManager = null;
        [SerializeField] private StringCollection words = null;

        [SerializeField] private AudioClip inputErrorSound = null;

        private AudioSource audioSource;

        public event System.Action OnGameOver = delegate () { };

        public event System.Action OnLevelCompleted = delegate () { };

        public static GameManager Instance { get; private set; }

        public void RestartGame()
        {
            Scene loadedLevel = SceneManager.GetActiveScene();
            SceneManager.LoadScene(loadedLevel.buildIndex);
        }

        /// <summary>
        /// Creates a sequence attached to the given transform and with given word length.
        /// </summary>
        public InputSequence CreateSequence(Transform transform, int wordCount)
        {
            InputSequence sequence = sequenceManager.CreateSequence(GetNewSequenceText(wordCount), transform);
            return sequence;
        }

        private void Awake()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            Unpause();

            // listen to sequence manager events
            sequenceManager.OnInputFailed += HandleTypingFailure;
            sequenceManager.OnSelectionFailed += HandleTypingFailure;

            // listen to player events
            Player.Instance.OnHealthChanged += HandlePlayerHealthChange;

            // listen to player path events
            FindObjectOfType<PlayerPath>().OnCompletion += delegate { OnLevelCompleted(); };
        }

        private void HandlePlayerHealthChange(float health)
        {
            if (health <= 0)
            {
                OnGameOver();
            }
        }

        private void HandleTypingFailure(SequenceManager manager)
        {
            audioSource.PlayOneShot(inputErrorSound);
        }

        private string GetNewSequenceText(int wordCount)
        {
            string sequenceText = sequenceManager.GetUniquelyTargetableString(words) ?? words.PickRandomString();

            for (int i = 0; i < wordCount - 1; i++)
            {
                sequenceText += " " + words.PickRandomString();
            }

            return sequenceText;
        }

        public void Pause()
        {
            Time.timeScale = 0f;
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
        }
    }
}
using UnityEngine;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Tracks the player score through the level.
    /// </summary>
    public class ScoreTracker : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Text scoreDisplay = null;
        [SerializeField] private UnityEngine.UI.Text accuracyDisplay = null;

        private float completedSequences;
        private int correctInput;
        private int highScore;
        private int incorrectInput;

        public float CurrentAccuracy
        {
            get
            {
                float sum = correctInput + incorrectInput;
                return sum == 0 ? 1 : correctInput / sum;
            }
        }

        public int CurrentScore
        {
            get
            {
                return (int)(completedSequences * CurrentAccuracy * Player.Instance.Health * 100);
            }
        }

        private void Start()
        {
            UpdateDisplay();

            // set up sequence manger
            SequenceManager sequenceManager = FindObjectOfType<SequenceManager>();
            sequenceManager.OnInputSucceeded += HandleCorrectInput;
            sequenceManager.OnInputFailed += HandleIncorrectInput;
            sequenceManager.OnSelectionFailed += HandleIncorrectInput;
            sequenceManager.OnSequenceCompleted += HandleSequenceCompleted;

            Player.Instance.OnHealthChanged += delegate { UpdateDisplay(); };
        }

        private void HandleSequenceCompleted(SequenceManager manager)
        {
            completedSequences++;
            UpdateDisplay();
        }

        private void HandleCorrectInput(SequenceManager manager)
        {
            correctInput++;
            UpdateDisplay();
        }

        private void HandleIncorrectInput(SequenceManager manager)
        {
            incorrectInput++;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            scoreDisplay.text = string.Format("Score: {0}", CurrentScore);
            accuracyDisplay.text = string.Format("Accuracy: {0:P0}", CurrentAccuracy);
        }
    }
}
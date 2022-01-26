using UnityEngine;

namespace TypingGameKit.AsteroidTyping
{
    /// <summary>
    /// Keeps track of the score during the game session.
    /// </summary>
    public class ScoreTracker : MonoBehaviour
    {
        [SerializeField] private SequenceManager sequenceManager = null;

        [SerializeField] private UnityEngine.UI.Text highScoreDisplay = null;
        [SerializeField] private UnityEngine.UI.Text scoreDisplay = null;

        private int currentHighScore;
        private int correctInput;
        private int incorrectInput;

        public float CurrentAccuracy
        {
            get
            {
                float sum = correctInput + incorrectInput;
                return sum == 0 ? 0 : correctInput / sum;
            }
        }

        public int CurrentScore
        {
            get { return (int)(correctInput * CurrentAccuracy); }
        }

        private void Awake()
        {
            UpdateHighScoreDisplay();
            UpdateScoreDisplay();
            ListenToInputEvents();
        }

        private void ListenToInputEvents()
        {
            sequenceManager.OnInputSucceeded += CorrectInputReceived;
            sequenceManager.OnInputFailed += IncorrectInputReceived;
            sequenceManager.OnSelectionFailed += IncorrectInputReceived;
        }

        private void CorrectInputReceived(SequenceManager _)
        {
            correctInput++;
            UpdateScoreDisplay();
        }

        private void IncorrectInputReceived(SequenceManager _)
        {
            incorrectInput++;
            UpdateScoreDisplay();
        }

        private void UpdateHighScoreDisplay()
        {
            highScoreDisplay.text = string.Format("High Score: {0}", HighScore.GetHighScore());
        }

        private void UpdateScoreDisplay()
        {
            scoreDisplay.text = string.Format("Score: {0}", CurrentScore);
        }
    }
}
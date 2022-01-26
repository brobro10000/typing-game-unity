using UnityEngine;
using UnityEngine.SceneManagement;

namespace TypingGameKit.AsteroidTyping
{
    /// <summary>
    /// Panel showing the results of the game session.
    /// </summary>
    public class ResultsPanel : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Text displayText = null;

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void RestartGame()
        {
            Scene loadedLevel = SceneManager.GetActiveScene();
            SceneManager.LoadScene(loadedLevel.buildIndex);
        }

        private void Awake()
        {
            int currentScore = FindObjectOfType<ScoreTracker>().CurrentScore;
            SetMessage(currentScore);
            UpdateHighScore(currentScore);
        }

        private void SetMessage(int score)
        {
            if (score > HighScore.GetHighScore())
            {
                displayText.text = "New High Score: " + score;
            }
            else
            {
                displayText.text = "Score: " + score;
            }
        }

        private static void UpdateHighScore(int score)
        {
            if (score > HighScore.GetHighScore())
            {
                HighScore.SetHighScore(score);
            }
        }
    }
}
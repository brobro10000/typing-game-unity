using UnityEngine;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Displays the result panel.
    /// </summary>
    public class ResultsPanel : MonoBehaviour
    {
        [SerializeField] private GameObject content = null;

        [SerializeField] private UnityEngine.UI.Text titleText = null;
        [SerializeField] private UnityEngine.UI.Text scoreText = null;
        [SerializeField] private UnityEngine.UI.Text highScoreText = null;

        private void Awake()
        {
            content.SetActive(false);
        }

        private void GameOver()
        {
            GameManager.Instance.Pause();
            titleText.text = "Game Over";
            content.SetActive(true);
            UpdateScore();
        }

        private void LevelCompleted()
        {
            GameManager.Instance.Pause();
            titleText.text = "Level Completed";
            content.SetActive(true);
            UpdateScore();
        }

        private void Start()
        {
            GameManager.Instance.OnGameOver += GameOver;
            GameManager.Instance.OnLevelCompleted += LevelCompleted;
        }

        private void UpdateScore()
        {
            var score = FindObjectOfType<ScoreTracker>().CurrentScore;
            var highScore = HighScore.GetHighScore();

            if (score > highScore)
            {
                HighScore.SetHighScore(score);
                scoreText.text = score.ToString();
                highScoreText.text = "-";
            }
            else
            {
                scoreText.text = score.ToString();
                highScoreText.text = highScore.ToString();
            }
        }
    }
}
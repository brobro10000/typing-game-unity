using UnityEngine;

namespace TypingGameKit.AsteroidTyping
{
    /// <summary>
    /// Manages the score persistence.
    /// </summary>
    public static class HighScore
    {
        private const string highScoreKey = "Asteroid Typing HighScore";

        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(highScoreKey, 0);
        }

        public static void SetHighScore(int value)
        {
            PlayerPrefs.SetInt(highScoreKey, value);
        }
    }
}
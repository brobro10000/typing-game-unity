using UnityEngine;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Handles score persistence.
    /// </summary>
    public static class HighScore
    {
        private const string highScoreKey = "Alien Typing HighScore";

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
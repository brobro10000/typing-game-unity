using UnityEngine;
using UnityEngine.UI;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Shows the health of the player.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image image = null;

        private void Start()
        {
            Player.Instance.OnHealthChanged += UpdateHealth;
        }

        private void UpdateHealth(float value)
        {
            image.fillAmount = value;
        }
    }
}
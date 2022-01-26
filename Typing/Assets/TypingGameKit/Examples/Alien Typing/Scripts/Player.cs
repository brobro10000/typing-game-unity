using UnityEngine;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Handles player's the health, animations, and actions.
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private LaserGun gun = null;
        [SerializeField] private float health = 1f;

        public event System.Action<float> OnHealthChanged = delegate { };

        public static Player Instance { get; private set; }

        public float Health
        {
            get { return health; }
        }

        public LaserGun LaserGun
        {
            get { return gun; }
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            OnHealthChanged(health);
            animator.SetTrigger("OnHit");
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}
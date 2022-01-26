using System;
using UnityEngine;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// A section in the game designates a group of enemies that has to be
    /// disposed of before the player can proceed .
    /// </summary>
    public class Section : MonoBehaviour
    {
        [SerializeField] private float completionDelay = 1f;

        private Action onSectionCompleted;
        private Enemy[] enemies;
        private int enemiesDownCount;

        /// <summary>
        /// Begins the section. The passed callback will be called when the section is complete.
        /// </summary>
        public void Begin(System.Action sectionCompletedCallback)
        {
            onSectionCompleted = sectionCompletedCallback;
            enemies = GetComponentsInChildren<Enemy>();

            if (enemies.Length == 0)
            {
                // skip section if no enemies
                CompleteSection();
                return;
            }

            foreach (var enemy in enemies)
            {
                enemy.enabled = true;
                enemy.OnDeath += HandleEnemyDeath;
            }
        }

        private void CompleteSection()
        {
            onSectionCompleted();
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            enemiesDownCount += 1;

            if (enemies.Length == enemiesDownCount)
            {
                Invoke("CompleteSection", completionDelay);
            }
        }
    }
}
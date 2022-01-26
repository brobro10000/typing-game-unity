using System.Linq;
using UnityEngine;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// Manges the players path through the level.
    /// </summary>
    public class PlayerPath : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private int startSection = 0;

        private int sectionIndex = 0;

        public event System.Action OnCompletion = delegate { };

        private Section[] sections;

        private void Awake()
        {
            // discover the sections of the level
            sections = FindObjectsOfType<Section>().OrderBy(s => s.name).ToArray();
        }

        // called by path animation
        private void PathCompleted()
        {
            OnCompletion();
        }

        // called by path animation
        private void SectionReached()
        {
            SetPathSpeed(0);
            if (sectionIndex < startSection)
            {
                // skip section
                SectionCompleted();
            }
            else
            {
                sections[sectionIndex].Begin(SectionCompleted);
            }
        }

        private void SectionCompleted()
        {
            SetPathSpeed(1);
            sectionIndex += 1;
        }

        private void SetPathSpeed(float value)
        {
            animator.speed = value;
        }
    }
}
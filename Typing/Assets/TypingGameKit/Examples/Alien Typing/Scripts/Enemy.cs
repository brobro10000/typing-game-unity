using TypingGameKit.Util;
using UnityEngine;
using UnityEngine.AI;

namespace TypingGameKit.AlienTyping
{
    /// <summary>
    /// The enemy component manages the enemy AI, sound, and animation.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private GameObject remains = null;

        [SerializeField] private float attackRate = 5f;
        [SerializeField] private float attackStrength = 0.1f;

        [SerializeField] private Transform sequenceAnchor = null;
        [SerializeField] private int sequenceWordCount = 1;

        [SerializeField] private SoundConfig soundConfig = null;

        private NavMeshAgent agent;
        private AudioSource audioSource;

        private bool hasBeenDiscovered = false;
        private float lastAttackTime;

        /// <summary>
        /// Raised when the enemy has been killed.
        /// </summary>
        public event System.Action<Enemy> OnDeath;

        private Vector3 CenterPosition
        {
            get { return transform.position + Vector3.up * agent.height / 2; }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            audioSource = GetComponent<AudioSource>();

            enabled = false;
            hasBeenDiscovered = false;
        }

        private void Update()
        {
            CreateSequenceIfDiscoveredByPlayer();
            UpdateMovement();
            AttackIfPossible();
        }

        private void CreateSequenceIfDiscoveredByPlayer()
        {
            if (hasBeenDiscovered == false && CanBeSeenByPlayer())
            {
                hasBeenDiscovered = true;
                CreateSequence();
            }
        }

        private void UpdateMovement()
        {
            // keep playing movement animation until velocity is zero
            animator.SetBool("IsMoving", agent.velocity != Vector3.zero);

            // keep destination updated
            agent.SetDestination(Player.Instance.transform.position);
        }

        private void AttackIfPossible()
        {
            if (DestinationReached() && Time.time - lastAttackTime > attackRate)
            {
                Attack();
            }
        }

        private void Attack()
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
            PlaySound(soundConfig.AttackSounds.PickRandom());
        }

        private bool CanBeSeenByPlayer()
        {
            Vector3 origin = CenterPosition;
            Vector3 target = Player.Instance.transform.position + Vector3.up;
            Vector3 direction = target - origin;

            RaycastHit hitInfo;
            return Physics.Raycast(origin, direction, out hitInfo) && hitInfo.transform.tag == "Player";
        }

        private void CreateSequence()
        {
            var sequence = GameManager.Instance.CreateSequence(sequenceAnchor, sequenceWordCount);
            sequence.OnInputSucceeded += delegate { Player.Instance.LaserGun.FireAt(CenterPosition); };
            sequence.OnCompleted += delegate { Die(); };
        }

        private bool DestinationReached()
        {
            return agent.pathPending == false &&
                (agent.remainingDistance <= agent.stoppingDistance) &&
                (agent.hasPath == false || agent.velocity == Vector3.zero);
        }

        private void Die()
        {
            OnDeath(this);
            Instantiate(remains, CenterPosition, transform.rotation);
            Destroy(gameObject);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip && audioSource.isPlaying == false)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private void AttackHasLanded()
        {
            Player.Instance.TakeDamage(attackStrength);
        }

        private void StepImpact()
        {
            PlaySound(soundConfig.StepSounds.PickRandom());
        }

        [System.Serializable]
        private class SoundConfig
        {
            public AudioClip[] AttackSounds = null;
            public AudioClip[] StepSounds = null;
        }
    }
}
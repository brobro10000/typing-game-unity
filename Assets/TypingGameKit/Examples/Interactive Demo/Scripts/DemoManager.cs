using TypingGameKit.Util;
using UnityEngine;

namespace TypingGameKit.Demo
{
    /// <summary>
    /// Sets up and manages the interactive demo.
    /// </summary>
    public class DemoManager : MonoBehaviour
    {
        private AudioSource audioSource;

        [SerializeField] private int initualSequenceCount = 10;
        [SerializeField] private GameObject anchorObject = null;
        [SerializeField] private SequenceManager sequenceManager = null;
        [SerializeField] private SequenceSettings sequenceSettings = null;
        [SerializeField] private DemoSounds sounds = null;
        [SerializeField] private StringCollection wordCollection = null;
        [SerializeField] private float positionRange = 10;
        [SerializeField] private Transform anchorParent = null;

        public SequenceManager SequenceManager
        {
            get { return sequenceManager; }
        }

        public SequenceSettings SequenceSettings
        {
            get { return sequenceSettings; }
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            SetupSequenceManager();
            SpawnInitialSequences();
        }

        private Vector3 RandomPosition()
        {
            float x = Random.Range(-positionRange, positionRange);
            float y = Random.Range(-positionRange, positionRange);
            float z = Random.Range(-positionRange, positionRange);
            return new Vector3(x, y, z);
        }

        private void SetupSequenceManager()
        {
            sequenceManager.DefaultSequenceSettings = SequenceSettings;
            sequenceManager.OnInputFailed += delegate { PlayClip(sounds.inputFailedClip); };
            sequenceManager.OnInputSucceeded += delegate { PlayClip(sounds.inputSucceededClip); };
            sequenceManager.OnSequenceCompleted += delegate { PlayClip(sounds.sequenceCompletedClip); };
        }

        private void SpawnInitialSequences()
        {
            for (int i = 0; i < initualSequenceCount; i++)
            {
                SpawnAnchorWithSequence();
            }
        }

        public void SpawnAnchorWithSequence()
        {
            GameObject anchor = Instantiate(anchorObject, anchorParent, true);
            anchor.transform.position = RandomPosition();
            AttachSequence(anchor);
        }

        private void AttachSequence(GameObject obj)
        {
            var sequence = sequenceManager.CreateSequence(GetNewSequenceText(), obj.transform);
            sequence.OnCompleted += delegate { Destroy(obj); };
            sequence.OnRemoval += delegate { Destroy(obj); };
        }

        private string GetNewSequenceText()
        {
            string selectedText = sequenceManager.GetUniquelyTargetableString(wordCollection);
            selectedText = selectedText ?? wordCollection.PickRandomString();

            return sequenceManager.GetUniquelyTargetableString(wordCollection) ?? wordCollection.Strings.PickRandom();
        }

        private void PlayClip(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        [System.Serializable]
        private class DemoSounds
        {
            public AudioClip inputFailedClip = null;
            public AudioClip inputSucceededClip = null;
            public AudioClip sequenceCompletedClip = null;
        }
    }
}
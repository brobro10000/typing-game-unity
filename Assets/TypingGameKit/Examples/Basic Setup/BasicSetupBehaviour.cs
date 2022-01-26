using UnityEngine;

namespace TypingGameKit.BasicSetup
{
    // Manages a simple working game using TypingGameKit
    public class BasicSetupBehaviour : MonoBehaviour
    {
        [SerializeField] private float anchorSpawnRadius = 5;
        [SerializeField] private float concurrentSequenceCount = 10;
        [SerializeField] private SequenceManager sequenceManager = null;
        [SerializeField] private StringCollection stringCollection = null;

        private void Start()
        {
            // let's spawn a new sequence once one been completed
            sequenceManager.OnSequenceCompleted += delegate { SpawnAnchorWithSequence(); };

            // spawn the initial sequences
            for (int i = 0; i < concurrentSequenceCount; i++)
            {
                SpawnAnchorWithSequence();
            }
        }

        // Creates an input sequence and creates and anchor for it.
        private void SpawnAnchorWithSequence()
        {
            // let's create an object to anchor the sequence to.
            GameObject anchorObject = CreateAnchorObject();

            // Select a uniquely targetable string from stringCollection;
            string text = sequenceManager.GetUniquelyTargetableString(stringCollection) ?? stringCollection.PickRandomString();

            // let's create the sequence now
            InputSequence sequence = sequenceManager.CreateSequence(text, anchorObject.transform);

            // let's destroy the anchorObject when the sequence is completed.
            sequence.OnCompleted += delegate { Destroy(anchorObject); };
        }

        // Creates an anchor object somewhere in the scene.
        private GameObject CreateAnchorObject()
        {
            GameObject obj = new GameObject("Anchor Object");
            obj.transform.SetParent(transform);
            obj.transform.position = Random.insideUnitSphere * anchorSpawnRadius;
            return obj;
        }
    }
}
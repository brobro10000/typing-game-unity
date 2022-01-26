using System;
using System.Collections.Generic;
using System.Linq;
using TypingGameKit.Util;
using UnityEngine;
using UnityEngine.Assertions;

namespace TypingGameKit
{
    /// <summary>
    /// Instantiates and manages sequences.
    /// </summary>
    public class SequenceManager : MonoBehaviour
    {

        private ManagedSequence targetedSequence;

        private List<ManagedSequence> sequences = new List<ManagedSequence>();

        private Vector3[] cornerArray = new Vector3[4];

        private static string virtualInput = "";

        private char latestInputChar;

        [SerializeField]
        [Tooltip("Specifies the settings to use for created sequences.")]
        private SequenceSettings sequenceSettings = null;

        [SerializeField]
        [Tooltip("Specifies the display template for created sequences.")]
        private SequenceDisplay displayTemplate = null;

        [SerializeField]
        [Tooltip("If toggled the manager will be case sensitive.")]
        private bool isCaseSensitive = false;

        /// <summary>
        /// Called when input failed to match the targeted sequence
        /// </summary>
        public event System.Action<SequenceManager> OnInputFailed = delegate { };

        /// <summary>
        /// Called when input matched the targeted sequence
        /// </summary>
        public event System.Action<SequenceManager> OnInputSucceeded = delegate { };

        /// <summary>
        /// Called when input was given but no sequence is targeted and no input matches any sequence.
        /// </summary>
        public event System.Action<SequenceManager> OnSelectionFailed = delegate { };

        /// <summary>
        /// Called when active sequence was completed.
        /// </summary>
        public event System.Action<SequenceManager> OnSequenceCompleted = delegate { };

        /// <summary>
        /// The SequenceSetting used for newly created sequences.
        /// </summary>
        public SequenceSettings DefaultSequenceSettings
        {
            get { return sequenceSettings; }
            set { sequenceSettings = value; }
        }

        /// <summary>
        /// Returns all the current input sequences.
        /// </summary>
        public InputSequence[] InputSequences
        {
            get { return sequences.ToArray(); }
        }

        /// <summary>
        /// Determines if case of input is regarded when matching input to required sequence input.
        /// </summary>
        public bool IsCaseSensitive
        {
            get { return isCaseSensitive; }
            set { isCaseSensitive = value; }
        }

        /// <summary>
        /// The latest input handled by the manager.
        /// </summary>
        public char LatestHandledInputChar
        {
            get { return latestInputChar; }
        }

        /// <summary>
        /// Returns the targeted sequence. Returns null if no sequence is targeted.
        /// </summary>
        public InputSequence TargetedSequence
        {
            get { return targetedSequence; }
        }

        /// <summary>
        /// Returns the number of managed sequences.
        /// </summary>
        public int SequenceCount
        {
            get { return sequences.Count(); }
        }

        // Camera to be used for determining the position of the sequences.
        private Camera UsedCamera
        {
            get { return Camera.main; }
        }

        /// <summary>
        /// Creates a new sequence with the provided text and anchor.
        /// </summary>
        public InputSequence CreateSequence(string text, Transform worldAnchor)
        {
            Assert.IsTrue(text.Length > 0);
            Assert.IsNotNull(worldAnchor);
            Assert.IsNotNull(sequenceSettings);

            var sequence = CreateSequenceInstance();
            sequence.Initialize(text, this, worldAnchor, sequenceSettings, displayTemplate);
            sequence.InitializeMover(UsedCamera);

            sequence.OnCompleted += RemoveFromSequences;
            sequence.OnRemoval += RemoveFromSequences;

            return (InputSequence)sequence;
        }

        public static void AddInput(string input)
        {
            virtualInput += input;
        }

        /// <summary>
        /// Clears the targeted sequence.
        /// </summary>
        public void ClearTargetedSequence()
        {
            if (targetedSequence != null)
            {
                targetedSequence.Untarget();
                targetedSequence = null;
            }
        }

        /// <summary>
        /// Returns a string from collection that is uniquely targetable among the manager's
        /// sequences. Returns null if no such string exists.
        /// </summary>
        public string GetUniquelyTargetableString(StringCollection collection)
        {
            foreach (var initialChar in collection.StringDict.Keys.Shuffle())
            {
                if (IsTextUniquelyTargetable(initialChar.ToString()))
                {
                    return collection.StringDict[initialChar].PickRandom();
                }
            }
            return null;
        }

        /// <summary>
        /// Returns true if the text is uniquely targetable among the managers sequences.
        /// </summary>
        public bool IsTextUniquelyTargetable(string text)
        {
            return sequences.Any(c => c.DoesInputMatch(text[0], isCaseSensitive)) == false;
        }

        /// <summary>
        /// Destroys all sequences.
        /// </summary>
        public void RemoveAllSequences()
        {
            foreach (var sequence in sequences.ToArray())
            {
                sequence.Remove();
            }
            sequences.Clear();
            targetedSequence = null;
        }

        private void ApplyDistanceScaling(Vector3 cameraPosition, ManagedSequence sequence)
        {
            if (sequence.Settings.UseDistanceScaling == false)
            {
                return;
            }

            var scaleSettings = sequence.Settings.DistanceScaleSettings;
            var cameraDistance = Vector3.Distance(sequence.WorldAnchor.transform.position, cameraPosition);

            var scale = scaleSettings.baseDistance / Mathf.Max(0.1f, cameraDistance);
            var clampedScale = Mathf.Max(scaleSettings.smallestScale, Mathf.Min(scaleSettings.largestScale, scale));

            sequence.transform.localScale = Vector3.one * clampedScale;
        }

        private ManagedSequence CreateSequenceInstance()
        {
            GameObject sequenceObject = new GameObject("Sequence");
            sequenceObject.transform.SetParent(transform, false);
            ManagedSequence sequence = sequenceObject.AddComponent<ManagedSequence>();
            sequences.Add(sequence);
            return sequence;
        }

        private void AttemptToTargetASequence(char input)
        {
            foreach (ManagedSequence sequence in sequences)
            {
                if (sequence.DoesInputMatch(input, IsCaseSensitive))
                {
                    targetedSequence = sequence;
                    targetedSequence.Target();
                    return;
                }
            }
            OnSelectionFailed(this);
        }

        private void LateUpdate()
        {
            UpdateDrawOrder();
            UpdateDistanceScaling();
            UpdateSequencePositions();
        }

        private void RemoveFromSequences(InputSequence sequence)
        {
            sequences.Remove((ManagedSequence)sequence);
        }

        private void Update()
        {
            UpdateInput();
        }

        private void UpdateDistanceScaling()
        {
            var cameraPosition = UsedCamera.transform.position;
            foreach (var sequence in sequences)
            {
                ApplyDistanceScaling(cameraPosition, sequence);
            }
        }

        private void UpdateDrawOrder()
        {
            // set draw order based on distance to camera
            int order = 0;
            var cameraPosition = UsedCamera.transform.position;
            foreach (var sequence in sequences.OrderByDescending(c => Vector3.Distance(c.WorldAnchor.transform.position, cameraPosition)))
            {
                sequence.transform.SetSiblingIndex(order);
                order += 1;
            }

            // draw the targeted sequence lastly
            if (targetedSequence != null)
            {
                targetedSequence.transform.SetAsLastSibling();
            }
        }

        private void UpdateSequencePositions()
        {
            Camera camera = UsedCamera;
            ((RectTransform)transform).GetWorldCorners(cornerArray);
            foreach (var sequence in sequences)
            {
                sequence.Mover.PrepareForMovementUpdate(cornerArray, camera);
            }

            for (int index = 0; index < sequences.Count; index++)
            {
                sequences[index].Mover.UpdateMovement(sequences);
            }
        }

        private void UpdateInput()
        {
            string input = Input.inputString + virtualInput;

            bool heldDown = virtualInput != "" && Input.anyKeyDown == false && input.Length > 0 && latestInputChar == input[0];
            if (SequenceCount == 0 || heldDown || input == "")
            {
                return;
            }

            virtualInput = "";
            latestInputChar = input[input.Length - 1];

            // go through each input and try to match it to a sequence
            foreach (char inputChar in input)
            {

                if (targetedSequence == null)
                {
                    AttemptToTargetASequence(inputChar);
                }

                if (targetedSequence != null && targetedSequence.IsCompleted == false)
                {
                    bool successful = SendInputToTargeted(inputChar);
                    if (successful == false)
                    {
                        break;
                    }
                }
            }
        }

        private bool SendInputToTargeted(char inputChar)
        {
            bool successful = targetedSequence.HandleInput(inputChar, IsCaseSensitive);

            if (successful)
            {
                OnInputSucceeded(this);

                if (targetedSequence.IsCompleted)
                {
                    CompleteCurrentSequence();
                }
            }
            else
            {
                OnInputFailed(this);
            }

            return successful;
        }

        private void CompleteCurrentSequence()
        {
            OnSequenceCompleted(this);
            RemoveFromSequences(targetedSequence);
            targetedSequence = null;
        }
    }
}
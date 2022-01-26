using UnityEngine;

namespace TypingGameKit
{
    /// <summary>
    /// Sequence object to be managed by the SequenceManager.
    /// </summary>
    public sealed class ManagedSequence : MonoBehaviour, InputSequence
    {
        private int startIndex;

        private string text;

        public event System.Action<InputSequence> OnCompleted = delegate { };

        public event System.Action<InputSequence> OnInputFailed = delegate { };

        public event System.Action<InputSequence> OnInputSucceeded = delegate { };

        public event System.Action<InputSequence> OnRemoval = delegate { };

        public event System.Action<InputSequence> OnTargeted = delegate { };

        /// <summary>
        /// The Associated display object.
        /// </summary>
        public SequenceDisplay Display { get; private set; }

        /// <summary>
        /// Returns true if the sequence has been completed;
        /// </summary>
        public bool IsCompleted
        {
            get { return (startIndex >= text.Length); }
        }

        /// <summary>
        /// The SequenceManaer that instantiated the sequence
        /// </summary>
        public SequenceManager Manager { get; private set; }

        /// <summary>
        /// The associated mover object.
        /// </summary>
        public SequenceMover Mover { get; private set; }

        /// <summary>
        /// The associated Settings.
        /// </summary>
        public SequenceSettings Settings { get; private set; }

        /// <summary>
        /// The text associated with the sequence.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { SetWord(value); }
        }

        /// <summary>
        /// The World Transform associated with the sequence.
        /// </summary>
        public Transform WorldAnchor { get; set; }

        /// <summary>
        /// Returns true if input matches
        /// </summary>
        public bool DoesInputMatch(char input, bool caseSensitive)
        {
            if (caseSensitive)
            {
                return text[startIndex % text.Length] == input;
            }
            else
            {
                return text.ToLower()[startIndex % text.Length] == char.ToLower(input);
            }
        }

        /// <summary>
        /// Handles the given input.
        /// </summary>
        public bool HandleInput(char input, bool caseSensitive)
        {
            bool doesMatch = DoesInputMatch(input, caseSensitive);
            if (doesMatch)
            {
                HandleMatchingInput();
            }
            else
            {
                OnInputFailed(this);
                Display.OnInputFailed();
            }
            return doesMatch;
        }

        /// <summary>
        /// Initializing the ManagedSequence.
        /// </summary>
        public void Initialize(string text, SequenceManager manager, Transform worldAnchor, SequenceSettings settings, SequenceDisplay displayTemplate)
        {
            Manager = manager;
            WorldAnchor = worldAnchor;
            Settings = settings;

            Display = Instantiate<SequenceDisplay>(displayTemplate, transform);
            Display.gameObject.SetActive(true);
            Display.Initialize(settings);

            SetWord(text);
        }

        /// <summary>
        /// Initialize the mover with a default state.
        /// </summary>
        public void InitializeMover(Camera camera)
        {
            Mover = new SequenceMover(this, Settings);
            Mover.InitializePosition(camera);
        }

        /// <summary>
        /// Destroys the sequence.
        /// </summary>
        public void Remove()
        {
            OnRemoval(this);
            Destroy(gameObject);
        }

        /// <summary>
        /// Resetting the progress of the sequence.
        /// </summary>
        public void ResetProgress()
        {
            InitializeText(text);
        }

        /// <summary>
        /// Sets the sequence as targeted.
        /// </summary>
        public void Target()
        {
            OnTargeted(this);
            Display.ShowAsTargeted();
        }

        /// <summary>
        /// Set the sequence as untargeted
        /// </summary>
        public void Untarget()
        {
            Display.ShowAsUntargeted();
        }

        private void HandleMatchingInput()
        {
            startIndex += 1;
            Display.SetText(text, startIndex);

            OnInputSucceeded(this);

            if (IsCompleted)
            {
                OnCompleted(this);
                Display.OnCompleted();
            }
            else
            {
                Display.OnInputSucceeded();
            }
        }

        private void InitializeText(string text)
        {
            this.text = text;
            startIndex = 0;
            Display.SetText(text, startIndex);
        }

        private void SetWord(string word)
        {
            name = "Sequence: " + word;
            InitializeText(word);
        }
    }
}
using System;
using UnityEngine;

namespace TypingGameKit
{
    /// <summary>
    /// An interface used interacting with a created sequence.
    /// </summary>
    public interface InputSequence
    {
        /// <summary>
        /// Raised when the sequence has been completed.
        /// </summary>
        event Action<InputSequence> OnCompleted;

        /// <summary>
        /// Raised when the sequence has been targeted.
        /// </summary>
        event Action<InputSequence> OnTargeted;

        /// <summary>
        /// Raised when the sequence receives input that does not match.
        /// </summary>
        event Action<InputSequence> OnInputFailed;

        /// <summary>
        /// Raised when a sequence is set for removal.
        /// </summary>
        event Action<InputSequence> OnRemoval;

        /// <summary>
        /// Raised when the sequence receives matching input.
        /// </summary>
        event Action<InputSequence> OnInputSucceeded;

        /// <summary>
        /// The sequence's manager.
        /// </summary>
        SequenceManager Manager { get; }

        /// <summary>
        /// Text associated with the sequence. Can be used to change the sequence's required input.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The transform associated with the sequence. Can be used to change the sequence's anchor.
        /// </summary>
        Transform WorldAnchor { get; set; }

        /// <summary>
        /// Removes the sequence.
        /// </summary>
        void Remove();

        /// <summary>
        /// Resets the progress of the sequence.
        /// </summary>
        void ResetProgress();
    }
}
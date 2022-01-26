using UnityEngine;

namespace TypingGameKit
{
    /// <summary>
    /// Determines the visual display of the sequence.
    /// </summary>
    public class SequenceDisplay : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Animator responding to display triggers.")]
        private Animator animator = null;

        [SerializeField]
        [Tooltip("The background image.")]
        private UnityEngine.UI.Image backgroundImage = null;

        [Tooltip("The text display.")]
        [SerializeField]
        private UnityEngine.UI.Text text = null;

        [SerializeField]
        [Tooltip("Seconds the sequences are to remain after completion.")]
        private float secondsToRemainAfterCompletion = 1f;

        private Color depletedColor;

        private int textIndex;

        private string currentText = "";

        private SequenceSettings settings;

        private bool showAsTargeted;

        /// <summary>
        /// Initialize the sequence
        /// </summary>
        public void Initialize(SequenceSettings settings)
        {
            settings.SettingsEditedInEditor += HandleChangedSettings;

            this.settings = settings;
            ShowAsUntargeted();
            OnInitialized();
        }

        public void OnCompleted()
        {
            animator.SetTrigger("Completed");

            if (secondsToRemainAfterCompletion >= 0)
            {
                Destroy(transform.parent.gameObject, secondsToRemainAfterCompletion);
            }
        }

        public void OnInitialized()
        {
            animator.Update(0);
        }

        public void OnInputFailed()
        {
            animator.SetTrigger("Failed");
        }

        public void OnInputSucceeded()
        {
            animator.SetTrigger("Succeeded");
        }

        /// <summary>
        /// Set the displayed text;
        /// </summary>
        /// <param name="text">Text to display.</param>
        /// <param name="startIndex">The start of the active text.</param>
        public void SetText(string text, int startIndex)
        {
            currentText = text;
            textIndex = startIndex;

            if (settings.ReplaceSpaceWithInterpunct)
            {
                text = ReplaceSpace(text, startIndex);
            }

            switch (settings.CompmletedTextDisplayMode)
            {
                case SequenceSettings.CompletedTextDisplay.Remove:
                    SetTextRemove(text, startIndex);
                    break;

                case SequenceSettings.CompletedTextDisplay.Colorize:
                    SetTextWithColor(text, startIndex);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Display as targeted.
        /// </summary>
        public void ShowAsTargeted()
        {
            showAsTargeted = true;
            SetConfiguration(settings.TargetedTheme);
        }

        /// <summary>
        /// Display as untargeted;
        /// </summary>
        public void ShowAsUntargeted()
        {
            showAsTargeted = false;
            SetConfiguration(settings.UntargetedTheme);
        }

        private void HandleChangedSettings()
        {
            if (showAsTargeted)
            {
                ShowAsTargeted();
            }
            else
            {
                ShowAsUntargeted();
            }
        }

        private void OnDestroy()
        {
            if (settings != null)
            {
                settings.SettingsEditedInEditor -= HandleChangedSettings;
            }
        }

        private string ReplaceSpace(string text, int index)
        {
            if (index >= text.Length || text[index] != ' ')
            {
                return text;
            }
            else
            {
                return text.Substring(0, index) + "·" + text.Substring(index + 1);
            }
        }

        private void SetConfiguration(SequenceSettings.DisplayTheme config)
        {
            backgroundImage.sprite = config.backgroundSprite;
            backgroundImage.color = config.backgroundColor;
            depletedColor = config.completedTextColor;
            text.color = config.textColor;

            SetText(currentText, textIndex);
        }

        private void SetTextRemove(string text, int startIndex)
        {
            this.text.text = text.Substring(startIndex);
        }

        private void SetTextWithColor(string text, int startIndex)
        {
            if (startIndex > 0)
            {
                this.text.text = string.Format(
                    "<color=#{0}>{1}</color>{2}",
                    ColorUtility.ToHtmlStringRGB(depletedColor),
                    text.Substring(0, startIndex),
                    text.Substring(startIndex)
                    );
            }
            else
            {
                this.text.text = text.Substring(startIndex);
            }
        }
    }
}
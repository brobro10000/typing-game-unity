using UnityEngine;

namespace TypingGameKit
{
    /// <summary>
    /// Settings defining sequence display.
    /// </summary>
    public partial class SequenceSettings : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private DistanceScalingSettings distanceScaleSettings = new DistanceScalingSettings();

        [SerializeField, HideInInspector]
        private OverlapAvoidanceConfig overlapAvoidanceSetting = new OverlapAvoidanceConfig();

        [SerializeField]
        [Tooltip("If toggled the sequence object is restricted it's manager RectTransform.")]
        private bool restrictToParent = false;

        [SerializeField, HideInInspector]
        private SoftMovementConfig softMovementSettings = new SoftMovementConfig();

        [SerializeField]
        [Tooltip("If toggled the sequence scale is in determined by it's distance to the camera.")]
        private bool useDistanceScaling = false;

        [SerializeField]
        [Tooltip("If toggled the sequence avoid overlapping other sequences")]
        private bool useOverlapAvoidance;

        [SerializeField]
        [Tooltip("If toggled the sequence will moved towards its anchored positions over time.")]
        private bool useSoftMovement;

        /// <summary>
        /// Is called when the settings has been altered in the editor.
        /// </summary>
        public event System.Action SettingsEditedInEditor = delegate () { };

        /// <summary>
        /// Contains the settings for distance scaling.
        /// </summary>
        public DistanceScalingSettings DistanceScaleSettings
        {
            get { return distanceScaleSettings; }
        }

        public OverlapAvoidanceConfig OverlapAvoidanceSetting
        {
            get { return overlapAvoidanceSetting; }
        }

        /// <summary>
        /// Sets the position restriction setting.
        /// </summary>
        public bool RestrictToParent
        {
            get { return restrictToParent; }
            set { restrictToParent = value; }
        }

        /// <summary>
        /// Contains the settings for the 'soft' movement.
        /// </summary>
        public SoftMovementConfig SoftMovementSettings
        {
            get { return softMovementSettings; }
        }

        /// <summary>
        /// Sets the distance scaling setting.
        /// </summary>
        public bool UseDistanceScaling
        {
            get { return useDistanceScaling; }
            set { useDistanceScaling = value; }
        }

        /// <summary>
        /// Sets the overlap avoidance setting.
        /// </summary>
        public bool UseOverlapAvoidance
        {
            get { return useOverlapAvoidance; }
            set { useOverlapAvoidance = value; }
        }

        /// <summary>
        /// Sets the soft movement settings.
        /// </summary>
        public bool UseSoftMovement
        {
            get { return useSoftMovement; }
            set { useSoftMovement = value; }
        }

        private void OnValidate()
        {
            SettingsEditedInEditor();
            if (softMovementSettings != null)
            {
                softMovementSettings.Validate();
            }
            if (distanceScaleSettings != null)
            {
                distanceScaleSettings.Validate();
            }
        }

        /// <summary>
        /// Class containing the distance scale settings.
        /// </summary>
        [System.Serializable]
        public class DistanceScalingSettings
        {
            [Tooltip("Distance at which the scale is default")]
            public float baseDistance = 5;

            [Tooltip("Largest allowable scale.")]
            public float largestScale = 1;

            [Tooltip("Smallest allowable scale.")]
            public float smallestScale = 0.5f;

            public void Validate()
            {
                largestScale = Mathf.Max(0, largestScale);
                smallestScale = Mathf.Max(0, smallestScale);
                baseDistance = Mathf.Max(0, baseDistance);
                smallestScale = Mathf.Min(smallestScale, largestScale);
            }
        }

        /// <summary>
        /// Class containing the overlap avoidance settings.
        /// </summary>
        [System.Serializable]
        public class OverlapAvoidanceConfig
        {
            [Tooltip("Determines the repulsion force when sequences overlap")]
            public float avoidanceStrength = 100f;

            [Range(0, 1)]
            [Tooltip("Determines how much sequences needs to overlap before trying to separate.")]
            public float overlapReluctance = 1f;

            public void Validate()
            {
                overlapReluctance = Mathf.Max(0, overlapReluctance);
                avoidanceStrength = Mathf.Max(0, avoidanceStrength);
            }
        }

        /// <summary>
        /// Class containing the soft movement settings.
        /// </summary>
        [System.Serializable]
        public class SoftMovementConfig
        {
            [Tooltip("Speed at which the sequence is following its anchored transform.")]
            public float moveSpeed = 100f;

            public void Validate()
            {
                moveSpeed = Mathf.Max(0, moveSpeed);
            }
        }
    }

    // Display related settings
    public partial class SequenceSettings : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("This setting determines how the completed portion of the sequence is displayed")]
        private CompletedTextDisplay completedTextDisplayMode = CompletedTextDisplay.Colorize;

        [SerializeField]
        [Tooltip("If this setting is toggled a interpunct will replace the upcoming space character in the sequence.")]
        private bool replaceSpaceWithInterpunct = true;

        [SerializeField]
        private DisplayTheme targetedConfiguration = null;

        [SerializeField]
        private DisplayTheme untargetedConfiguration = null;

        /// <summary>
        /// Enum containing all completed character display variants.
        /// </summary>
        public enum CompletedTextDisplay { Colorize, Remove }

        /// <summary>
        /// The completed char mode.
        /// </summary>
        public CompletedTextDisplay CompmletedTextDisplayMode
        {
            get { return completedTextDisplayMode; }
        }

        /// <summary>
        /// Sets/Gets the replace with interpunct setting.
        /// </summary>
        public bool ReplaceSpaceWithInterpunct
        {
            get { return replaceSpaceWithInterpunct; }
        }

        /// <summary>
        /// The targeted theme.
        /// </summary>
        public DisplayTheme TargetedTheme
        {
            get { return targetedConfiguration; }
        }

        /// <summary>
        /// The untargeted theme.
        /// </summary>
        public DisplayTheme UntargetedTheme
        {
            get { return untargetedConfiguration; }
        }

        /// <summary>
        /// DisplayTheme contains describes the theme of a sequence display.
        /// </summary>
        [System.Serializable]
        public class DisplayTheme
        {
            [Tooltip("Determines the background color of the image.")]
            public Color backgroundColor = Color.white;

            [Tooltip("Specifies the background sprite.")]
            public Sprite backgroundSprite = null;

            [Tooltip("Specifies the completed text color.")]
            public Color completedTextColor = Color.grey;

            [Tooltip("Specifies the default text color.")]
            public Color textColor = Color.black;
        }
    }
}
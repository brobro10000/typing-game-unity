using UnityEngine;

namespace TypingGameKit.Demo
{
    /// <summary>
    /// Handles the demo UI.
    /// </summary>
    public class DemoUI : MonoBehaviour
    {
        private DemoManager demoManager;

        [SerializeField] private UnityEngine.UI.Toggle restrictToParentToggle = null;
        [SerializeField] private UnityEngine.UI.Toggle useDistanceScalingToggle = null;
        [SerializeField] private UnityEngine.UI.Toggle useOverlapAvoidanceToggle = null;
        [SerializeField] private UnityEngine.UI.Toggle useSoftMovementToggle = null;

        public void ClearSequences()
        {
            demoManager.SequenceManager.RemoveAllSequences();
        }

        public void DeselectUI()
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }

        public void SetCamerHorizontalAngle(float angle)
        {
            var oldEuler = Camera.main.transform.eulerAngles;
            Camera.main.transform.eulerAngles = new Vector3(oldEuler.x, angle, oldEuler.z);
        }

        public void SetRotationSpeed(float angle)
        {
            FindObjectOfType<DemoAnchorParent>().AngleRotatedPerSecond = angle;
        }

        private void Awake()
        {
            demoManager = FindObjectOfType<DemoManager>();

            InitializeToggle(useSoftMovementToggle, SetSoftPositioning);
            InitializeToggle(useOverlapAvoidanceToggle, SetOverlapAvoidance);
            InitializeToggle(useDistanceScalingToggle, SetDistanceScaling);
            InitializeToggle(restrictToParentToggle, SetParentRestriction);
        }

        private void InitializeToggle(UnityEngine.UI.Toggle toggle, UnityEngine.Events.UnityAction<bool> valueChangedHandler)
        {
            valueChangedHandler(toggle.isOn);
            toggle.onValueChanged.AddListener(valueChangedHandler);
        }

        public void SetOverlapAvoidance(bool value)
        {
            demoManager.SequenceSettings.UseOverlapAvoidance = value;
        }

        public void SetSoftPositioning(bool value)
        {
            demoManager.SequenceSettings.UseSoftMovement = value;

            // initialize the position for all sequences
            var camera = Camera.main;
            foreach (var sequence in FindObjectsOfType<ManagedSequence>())
            {
                sequence.Mover.InitializePosition(camera);
            }
        }

        private void SetDistanceScaling(bool value)
        {
            demoManager.SequenceSettings.UseDistanceScaling = value;
            if (value)
            {
                return;
            }
            // reset the scale of the sequences
            foreach (var sequence in FindObjectsOfType<ManagedSequence>())
            {
                sequence.transform.localScale = Vector3.one;
            }
        }

        private void SetParentRestriction(bool value)
        {
            demoManager.SequenceSettings.RestrictToParent = value;
        }
    }
}
using UnityEditor;

namespace TypingGameKit
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SequenceSettings))]
    public class SequenceSettingsEditor : Editor
    {
        private SerializedProperty restrictToParentProp;
        private SerializedProperty useSoftMovementProp;
        private SerializedProperty softMovementSettingsProp;
        private SerializedProperty useDistanceScalingProp;
        private SerializedProperty useOverlapAvoidanceProp;
        private SerializedProperty overlapAvoidanceSettingProp;
        private SerializedProperty distanceScalingSettingsProp;

        private SerializedProperty completedCharacterModeProp;
        private SerializedProperty replaceSpaceWithInterpunctProp;
        private SerializedProperty targeteedConfigurationProp;
        private SerializedProperty untargetedConfigurationProp;

        private void OnEnable()
        {
            restrictToParentProp = serializedObject.FindProperty("restrictToParent");
            useSoftMovementProp = serializedObject.FindProperty("useSoftMovement");
            softMovementSettingsProp = serializedObject.FindProperty("softMovementSettings");
            useDistanceScalingProp = serializedObject.FindProperty("useDistanceScaling");
            useOverlapAvoidanceProp = serializedObject.FindProperty("useOverlapAvoidance");
            overlapAvoidanceSettingProp = serializedObject.FindProperty("overlapAvoidanceSetting");
            distanceScalingSettingsProp = serializedObject.FindProperty("distanceScaleSettings");

            completedCharacterModeProp = serializedObject.FindProperty("completedTextDisplayMode");
            replaceSpaceWithInterpunctProp = serializedObject.FindProperty("replaceSpaceWithInterpunct");
            targeteedConfigurationProp = serializedObject.FindProperty("targetedConfiguration");
            untargetedConfigurationProp = serializedObject.FindProperty("untargetedConfiguration");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(restrictToParentProp);

            EditorGUILayout.Separator();

            {
                EditorGUILayout.PropertyField(useSoftMovementProp);
                EditorGUI.BeginDisabledGroup(useSoftMovementProp.boolValue == false);
                ShowPropertyExpanded(softMovementSettingsProp, "moveSpeed");
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();

            {
                EditorGUILayout.PropertyField(useOverlapAvoidanceProp);
                EditorGUI.BeginDisabledGroup(useOverlapAvoidanceProp.boolValue == false);
                ShowPropertyExpanded(overlapAvoidanceSettingProp, "avoidanceStrength", "overlapReluctance");
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();

            {
                EditorGUILayout.PropertyField(useDistanceScalingProp);
                EditorGUI.BeginDisabledGroup(useDistanceScalingProp.boolValue == false);
                ShowPropertyExpanded(distanceScalingSettingsProp, "baseDistance", "largestScale", "smallestScale");
                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.Separator();

            EditorGUILayout.PropertyField(completedCharacterModeProp);
            EditorGUILayout.PropertyField(replaceSpaceWithInterpunctProp);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Targeted Theme");
            ShowPropertyExpanded(targeteedConfigurationProp, "backgroundColor", "backgroundSprite", "completedTextColor", "textColor");

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Untargeted Theme");
            ShowPropertyExpanded(untargetedConfigurationProp, "backgroundColor", "backgroundSprite", "completedTextColor", "textColor");

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowPropertyExpanded(SerializedProperty property, params string[] childProps)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < childProps.Length; i++)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative(childProps[i]));
            }
            EditorGUI.indentLevel--;
        }
    }
}
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        
        SerializedProperty playerModelProp = serializedObject.FindProperty("PlayerModel");
        SerializedProperty playerProp = serializedObject.FindProperty("Player");

        EditorGUILayout.PropertyField(playerProp);

        EditorGUILayout.LabelField("Player Models", EditorStyles.boldLabel);

        for (int i = 0; i < (int)eCharacterState.Max; i++)
        {
            string name = ((eCharacterState)i).ToString();

            if (i < playerModelProp.arraySize)
            {
                SerializedProperty element = playerModelProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent(name));
            }
        }
        
        SerializedProperty playerAbilityTemplateProp = serializedObject.FindProperty("playerAbilityTemplate");
        
        EditorGUILayout.LabelField("Player Ability", EditorStyles.boldLabel);
        for (int i = 0; i < (int)eCharacterState.Max; i++)
        {
            string name = ((eCharacterState)i).ToString();

            if (i < playerAbilityTemplateProp.arraySize)
            {
                SerializedProperty element = playerAbilityTemplateProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent(name));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
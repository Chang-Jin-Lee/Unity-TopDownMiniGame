#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSpawner))]
public class WeaponSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty playerModelProp = serializedObject.FindProperty("weaponModel");
        SerializedProperty playerProp = serializedObject.FindProperty("player");

        EditorGUILayout.PropertyField(playerProp);

        EditorGUILayout.LabelField("Weapon Models", EditorStyles.boldLabel);

        for (int i = 0; i < (int)eCharacterState.Max; i++)
        {
            string name = ((eCharacterState)i).ToString();

            if (i < playerModelProp.arraySize)
            {
                SerializedProperty element = playerModelProp.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent(name));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
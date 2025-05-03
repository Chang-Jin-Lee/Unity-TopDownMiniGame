#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainCanvas))]
public class MainCanvasEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        // live2DGroups 제외하고 전부 수동 렌더링
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            if (prop.name == "live2DGroups") continue;
            if (prop.name == "cursor") continue;
            EditorGUILayout.PropertyField(prop, true);
            enterChildren = false;
        }
        EditorGUILayout.Space(15);

        // 전체 Live2D 배열
        SerializedProperty live2DGroups = serializedObject.FindProperty("live2DGroups");

        for (int i = 0; i < live2DGroups.arraySize; i++)
        {
            var characterName = ((eCharacterState)i).ToString();
            var groupElement = live2DGroups.GetArrayElementAtIndex(i);
            SerializedProperty animArray = groupElement.FindPropertyRelative("live2DAnimations");

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(characterName, EditorStyles.boldLabel);

            for (int j = 0; j < animArray.arraySize; j++)
            {
                var animName = ((eLive2DState)j).ToString();
                var animElement = animArray.GetArrayElementAtIndex(j);
                EditorGUILayout.PropertyField(animElement, new GUIContent(animName));
            }

            EditorGUILayout.EndVertical();
        }
        
        // 전체 cursors texture
        SerializedProperty cursors = serializedObject.FindProperty("cursors");
        EditorGUILayout.PropertyField(cursors);
        EditorGUILayout.LabelField("cursors", EditorStyles.boldLabel);

        for (int i = 0; i < (int)eCharacterState.Max; i++)
        {
            string name = ((eCharacterState)i).ToString();
            if (i < cursors.arraySize)
            {
                SerializedProperty element = cursors.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent(name));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
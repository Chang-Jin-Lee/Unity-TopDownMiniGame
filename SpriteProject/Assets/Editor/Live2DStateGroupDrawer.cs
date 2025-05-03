#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Live2DStateGroup))]
public class Live2DStateGroupDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var arrayProp = property.FindPropertyRelative("states");
        return EditorGUIUtility.singleLineHeight * (arrayProp.arraySize + 1);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var statesProp = property.FindPropertyRelative("states");

        EditorGUI.LabelField(position, label);
        position.y += EditorGUIUtility.singleLineHeight;

        for (int i = 0; i < statesProp.arraySize; i++)
        {
            var stateName = ((eLive2DState)i).ToString();
            var elementRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(elementRect, statesProp.GetArrayElementAtIndex(i), new GUIContent(stateName));
            position.y += EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif
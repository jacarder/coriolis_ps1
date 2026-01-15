using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Skill))]
public class SkillDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		SerializedProperty typeProp = property.FindPropertyRelative("type");
		SerializedProperty valueProp = property.FindPropertyRelative("value");

		float typeWidth = position.width * 0.65f;

		Rect typeRect = new Rect(
			position.x,
			position.y,
			typeWidth,
			EditorGUIUtility.singleLineHeight
		);

		Rect valueRect = new Rect(
			position.x + typeWidth + 4f,
			position.y,
			position.width - typeWidth - 4f,
			EditorGUIUtility.singleLineHeight
		);

		// Skill name (read-only)
		EditorGUI.BeginDisabledGroup(true);
		EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
		EditorGUI.EndDisabledGroup();

		// Skill value
		EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUIUtility.singleLineHeight;
	}
}

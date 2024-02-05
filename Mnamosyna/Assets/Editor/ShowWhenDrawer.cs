using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowWhenAttribute))]
public class ShowWhenDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowWhenAttribute condition = attribute as ShowWhenAttribute;

        SerializedProperty conditionProperty = property.serializedObject.FindProperty(condition.conditionName);

        if (conditionProperty != null)
        {
            bool showProperty = IsConditionMet(conditionProperty, condition.expectedValue);

            if (!showProperty)
                return;
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    private bool IsConditionMet(SerializedProperty property, object expectedValue)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return property.boolValue.Equals(expectedValue);
            case SerializedPropertyType.Enum:
                return property.enumValueIndex.Equals((int)expectedValue);
            default:
                Debug.LogError("Condition type not supported yet: " + property.propertyType);
                return true;
        }
    }
}


using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowWhenAttribute))]
public class ShowWhenDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowWhenAttribute showWhen = attribute as ShowWhenAttribute;

        SerializedProperty condition1 = property.serializedObject.FindProperty(showWhen.conditionField1);
        SerializedProperty condition2 = null;
        if (!string.IsNullOrEmpty(showWhen.conditionField2))
            condition2 = property.serializedObject.FindProperty(showWhen.conditionField2);

        bool showProperty = true;

        if (condition1 != null)
            showProperty &= IsConditionSatisfied(condition1, showWhen.conditionValue1);

        if (condition2 != null)
            showProperty &= IsConditionSatisfied(condition2, showWhen.conditionValue2);

        if (showProperty)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowWhenAttribute showWhen = attribute as ShowWhenAttribute;

        SerializedProperty condition1 = property.serializedObject.FindProperty(showWhen.conditionField1);
        SerializedProperty condition2 = null;
        if (!string.IsNullOrEmpty(showWhen.conditionField2))
            condition2 = property.serializedObject.FindProperty(showWhen.conditionField2);

        bool showProperty = true;

        if (condition1 != null)
            showProperty &= IsConditionSatisfied(condition1, showWhen.conditionValue1);

        if (condition2 != null)
            showProperty &= IsConditionSatisfied(condition2, showWhen.conditionValue2);

        if (showProperty)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return 0f;
        }
    }

    private bool IsConditionSatisfied(SerializedProperty conditionProperty, object conditionValue)
    {
        switch (conditionProperty.propertyType)
        {
            case SerializedPropertyType.Boolean:
                return conditionProperty.boolValue.Equals(conditionValue);
            case SerializedPropertyType.Enum:
                return conditionProperty.enumValueIndex == (int)conditionValue;
            case SerializedPropertyType.Integer:
                return conditionProperty.intValue == (int)conditionValue;
            case SerializedPropertyType.Float:
                return Mathf.Approximately(conditionProperty.floatValue, (float)conditionValue);
            case SerializedPropertyType.String:
                return conditionProperty.stringValue.Equals(conditionValue);
            default:
                return false;
        }
    }
}

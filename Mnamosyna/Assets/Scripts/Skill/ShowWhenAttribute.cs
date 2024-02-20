using UnityEngine;

public class ShowWhenAttribute : PropertyAttribute
{
    public string conditionField1;
    public object conditionValue1;
    public string conditionField2;
    public object conditionValue2;

    public ShowWhenAttribute(string conditionField1, object conditionValue1)
    {
        this.conditionField1 = conditionField1;
        this.conditionValue1 = conditionValue1;
    }

    public ShowWhenAttribute(string conditionField1, object conditionValue1, string conditionField2, object conditionValue2)
    {
        this.conditionField1 = conditionField1;
        this.conditionValue1 = conditionValue1;
        this.conditionField2 = conditionField2;
        this.conditionValue2 = conditionValue2;
    }
}




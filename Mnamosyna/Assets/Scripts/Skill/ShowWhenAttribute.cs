using UnityEngine;

public class ShowWhenAttribute : PropertyAttribute
{
    public string conditionName;
    public object expectedValue;
    public string groupCondition;

    public ShowWhenAttribute(string conditionName, object expectedValue, string groupCondition = "")
    {
        this.conditionName = conditionName;
        this.expectedValue = expectedValue;
        this.groupCondition = groupCondition;
    }
}


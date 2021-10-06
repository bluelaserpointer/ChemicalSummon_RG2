using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[DisallowMultipleComponent]
public class SBA_NumberApproachingText : MonoBehaviour
{
    public int targetValue;
    public float step = 0.25F;
    public float updateSpan = 0.05F;
    public Color incresingColor = Color.green, decresingColor = Color.red, normalColor = Color.white;

    //data
    Text text;
    public Text Text => text ?? (text = GetComponent<Text>());
    float waitTime;
    private void Update()
    {
        if ((waitTime += Time.deltaTime) < updateSpan)
            return;
        waitTime = 0;
        UpdateText();
    }
    public void UpdateText()
    {
        int displayValue = Convert.ToInt32(Text.text);
        if (displayValue < targetValue)
        {
            Text.color = incresingColor;
            Text.text = Approach(displayValue, targetValue, step).ToString();
        }
        else if (displayValue > targetValue)
        {
            Text.color = decresingColor;
            Text.text = Approach(displayValue, targetValue, step).ToString();
        }
        else
        {
            Text.color = normalColor;
        }
    }
    public static int Approach(int src, int dst, float step)
    {
        if (src == dst)
            return src;
        return src + (int)((dst - src) * step) + ((src < dst) ? 1 : -1);
    }
    public void SetValueImmediate(int value)
    {
        targetValue = value;
        Text.text = value.ToString();
    }
    public void SetValueImmediate()
    {
        Text.text = targetValue.ToString();
    }
}

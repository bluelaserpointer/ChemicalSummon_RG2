using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TextAndGauge : MonoBehaviour
{
    [Header("Gauge value range / value")]
    [SerializeField]
    Vector2 gaugeValueRange;
    [SerializeField]
    float gaugeValue;
    [Header("Text")]
    public string textPrefix;
    public string stringFormatOption;
    public string textSuffix;
    [SerializeField]
    bool showValueRateAsSuffix;
    [Header("UI Target")]
    public Slider slider;
    public TextMeshProUGUI text;

    //data
    public float GaugeValue {
        get => gaugeValue;
        set
        {
            if(gaugeValue != value)
            {
                gaugeValue = value;
                UpdateUI();
            }
        }
    }
    public float ValueRate => (gaugeValue - GaugeValueRangeMin) / (GaugeValueRangeMax - GaugeValueRangeMin);
    public float GaugeValueRangeMin {
        get => slider.minValue;
        set => slider.minValue = value;
    }
    public float GaugeValueRangeMax
    {
        get => slider.maxValue;
        set => slider.maxValue = value;
    }

    private void Awake()
    {
        GaugeValueRangeMin = gaugeValueRange.x;
        GaugeValueRangeMax = gaugeValueRange.y;
        UpdateUI();
    }
    public void UpdateUI()
    {
        slider.value = gaugeValue;
        text.text = "";
        if (textPrefix.Length > 0)
            text.text += textPrefix;
        if (stringFormatOption.Length > 0)
            text.text += string.Format(stringFormatOption, gaugeValue);
        else
            text.text += gaugeValue;
        if (textSuffix.Length > 0)
            text.text += textSuffix;
        if (showValueRateAsSuffix)
            text.text += "(" + ValueRate + "%)";
    }
    /// <summary>
    /// add gauge value
    /// </summary>
    /// <param name="value"></param>
    public void AddValue(float value)
    {
        GaugeValue += value;
    }
}

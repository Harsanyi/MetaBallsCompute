using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LabeledSlider : MonoBehaviour
{
    [SerializeField] Slider slider = null;
    [SerializeField] TextMeshProUGUI valuelabel = null;

    public UnityEvent onValueChanged;

    public float value {
        get {
            return slider.value;
        }

        set {
            slider.value = value;
        }
    }

    public void SetValueWithoutNotify(float value) {
        slider.SetValueWithoutNotify(value);
        UpdateValueLabel();
    }

    private void Start()
    {
        UpdateValueLabel();
    }

    public void OnValueChanged() {
        UpdateValueLabel();
        onValueChanged?.Invoke();
    }

    void UpdateValueLabel() {
        valuelabel.text = slider.value.ToString();
    }
}

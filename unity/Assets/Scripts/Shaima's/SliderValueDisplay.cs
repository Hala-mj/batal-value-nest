using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueDisplay : MonoBehaviour
{
    public Slider ageSlider;       // Reference to the Slider
    public TMP_Text valueText;         // Reference to the Text where the number will be displayed

    void Start()
    {
        // Set the initial value for the text based on the slider's starting value
        UpdateSliderValue(ageSlider.value);

        // Add listener to update value when the slider changes
        ageSlider.onValueChanged.AddListener(delegate { UpdateSliderValue(ageSlider.value); });
    }

    // Update the text based on the slider's current value
    void UpdateSliderValue(float value)
    {
        valueText.text = Mathf.RoundToInt(value).ToString();  // Round the value to an integer for display
    }
}

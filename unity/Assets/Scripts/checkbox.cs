using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChangeMultipleButtonImages : MonoBehaviour
{
    [System.Serializable]
    public class ButtonImagePair
    {
        public Button button; // Assign the button in the inspector
        public Sprite newSprite; // Assign the new image in the inspector
        [HideInInspector] public Sprite originalSprite; // Store the original sprite
        public string key; // Unique key for saving the state
    }

    public List<ButtonImagePair> buttonImagePairs; // List of button-image pairs

    private void Start()
    {
        ResetIfNewDay();

        foreach (var pair in buttonImagePairs)
        {
            // Store the original sprite
            pair.originalSprite = pair.button.GetComponent<Image>().sprite;

            // Assign a unique key for each button
            if (string.IsNullOrEmpty(pair.key))
            {
                pair.key = pair.button.name; // Default key is the button name
            }

            // Restore the saved state
            int state = PlayerPrefs.GetInt(pair.key, 0); // Default is unchecked
            pair.button.GetComponent<Image>().sprite = state == 1 ? pair.newSprite : pair.originalSprite;

            // Add the click listener
            pair.button.onClick.AddListener(() => ToggleImage(pair));
        }
    }

    private void ToggleImage(ButtonImagePair pair)
    {
        Image buttonImage = pair.button.GetComponent<Image>();

        if (buttonImage.sprite == pair.originalSprite)
        {
            buttonImage.sprite = pair.newSprite; // Change to the new sprite
            PlayerPrefs.SetInt(pair.key, 1); // Save as checked
        }
        else
        {
            buttonImage.sprite = pair.originalSprite; // Reset to the original sprite
            PlayerPrefs.SetInt(pair.key, 0); // Save as unchecked
        }

        PlayerPrefs.Save();
    }

    private void ResetIfNewDay()
    {
        string lastResetDate = PlayerPrefs.GetString("LastResetDate", "");
        string currentDate = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (lastResetDate != currentDate)
        {
            // Reset all states
            foreach (var pair in buttonImagePairs)
            {
                PlayerPrefs.SetInt(pair.key, 0); // Unchecked state
            }

            // Update the reset date
            PlayerPrefs.SetString("LastResetDate", currentDate);
            PlayerPrefs.Save();
        }
    }
}

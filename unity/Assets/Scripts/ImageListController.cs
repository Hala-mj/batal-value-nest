using UnityEngine;
using UnityEngine.UI;

public class ImageListController : MonoBehaviour
{
    public GameObject imagePrefab;   // Prefab containing Image and Toggle
    public Transform content;        // Parent Content in Scroll View
    public Sprite[] imageSprites;    // Array of Sprites to display
    public ToggleEventHandler toggleEventHandler;  // Reference to the new script (where the window is controlled)

    void Start()
    {
        // Ensure all fields are assigned
        if (imagePrefab == null || content == null || imageSprites == null || toggleEventHandler == null)
        {
            Debug.LogError("Please assign all necessary fields in the Inspector.");
            return;
        }

        // Populate the list with images and toggles
        PopulateList();
    }

    void PopulateList()
    {
        // Remove any previous content in the content area
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Create new image items and toggles
        foreach (Sprite sprite in imageSprites)
        {
            if (sprite == null) continue;

            // Instantiate the image prefab
            GameObject newItem = Instantiate(imagePrefab, content);

            // Set the sprite in the Image component
            Image imgComponent = newItem.GetComponent<Image>();
            if (imgComponent != null)
            {
                imgComponent.sprite = sprite;
            }

            // Find the Toggle component in the prefab and add listener
            Toggle toggleComponent = newItem.GetComponentInChildren<Toggle>();
            if (toggleComponent != null)
            {
                // Add listener to the toggle button
                toggleComponent.onValueChanged.AddListener((isOn) =>
                {
                    // Notify the second script to show or hide the window
                    toggleEventHandler.OnToggleClicked(isOn);
                });
            }
            else
            {
                Debug.LogError("The prefab does not contain a Toggle component.");
            }
        }
    }
}

using UnityEngine;

public class ToggleEventHandler : MonoBehaviour
{
    public GameObject newWindow;   // Reference to the new window to show or hide
    public GameObject darkOverlay; // Reference to the dark overlay to show/hide

    void Start()
    {
        // Ensure the new window and dark overlay are hidden initially
        if (newWindow != null)
        {
            newWindow.SetActive(false);
        }
        else
        {
            Debug.LogError("Please assign the newWindow reference in the Inspector.");
        }

        if (darkOverlay != null)
        {
            darkOverlay.SetActive(false);  // Hide the dark overlay initially
        }
        else
        {
            Debug.LogError("Please assign the darkOverlay reference in the Inspector.");
        }
    }

    // This method is called when a toggle is clicked
    public void OnToggleClicked(bool isOn)
    {
        Debug.Log("OnToggleClicked: " + isOn); // Debug log to check the toggle state

        if (newWindow != null && darkOverlay != null)
        {
            // Show or hide the new window and dark overlay based on the toggle state
            newWindow.SetActive(isOn);
            darkOverlay.SetActive(isOn); // Show the dark overlay when the window appears
        }
    }
}

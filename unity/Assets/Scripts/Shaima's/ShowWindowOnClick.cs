using UnityEngine;
using UnityEngine.UI;

public class ShowWindowOnClick : MonoBehaviour
{
    public Button plusButton;  // Reference to the "+" button
    public GameObject windowPanel;  // Reference to the plane window (Panel) that will be shown

    public void Start()
    {
        windowPanel.SetActive(false);

        // Ensure all fields are assigned
        if (plusButton == null || windowPanel == null)
        {
            Debug.LogError("Please assign the button and window panel in the Inspector.");
            return;
        }

        // Add a listener to the "+" button to show the window when clicked
        plusButton.onClick.AddListener(ShowWindow);
    }

    // Method to show the window (panel)
    void ShowWindow()
    {
        // Enable the window panel (make it visible)
        windowPanel.SetActive(true);
    }
}

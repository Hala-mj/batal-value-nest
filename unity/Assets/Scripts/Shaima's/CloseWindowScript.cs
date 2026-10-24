using UnityEngine;
using UnityEngine.UI;

public class CloseWindowScript : MonoBehaviour
{
    public GameObject Window;         // Reference to the whole panel or window to close
    public Button cancelButton;       // Reference to the Cancel button
    public GameObject buttonCanvas;   // Reference to the bottom buttons canvas

    void Start()
    {
        // Ensure the Cancel button and Window are assigned
        if (cancelButton != null && Window != null)
        {
            cancelButton.onClick.AddListener(CloseWindow); // Add listener to Cancel button
            Debug.Log("Listener added to Cancel button.");
        }
        else
        {
            Debug.LogError("Cancel Button or Window is not assigned!");
        }
    }

    // This method is called when the Cancel button is clicked
    void CloseWindow()
    {
        if (Window != null)
        {
            // Deactivate the entire window (including the Cancel button)
            Window.SetActive(false);
            Debug.Log("Window deactivated.");

            // Reactivate the bottom buttons canvas
            if (buttonCanvas != null)
            {
                buttonCanvas.SetActive(true);
                Debug.Log("Button canvas activated.");
            }
            else
            {
                Debug.LogError("Button Canvas is not assigned!");
            }
        }
        else
        {
            Debug.LogError("Window is not assigned!");
        }
    }
}

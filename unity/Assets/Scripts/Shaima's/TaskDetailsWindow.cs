using UnityEngine;
using UnityEngine.UI;

public class TaskDetailsWindow : MonoBehaviour
{
    public GameObject TaskDetailswindow; // The window to be shown or hidden
    private Toggle toggle;    // The toggle button

    void Start()
    {
        // Get the Toggle component attached to this GameObject
        toggle = GetComponent<Toggle>();

        if (toggle == null)
        {
            Debug.LogError("Toggle component is not attached to this GameObject!");
            return;
        }

        if (TaskDetailswindow == null)
        {
            Debug.LogError("Window GameObject is not assigned!");
            return;
        }

        // Add listener to the Toggle's value changed event
        toggle.onValueChanged.AddListener(OnToggleChanged);

        // Ensure the window starts as hidden
        TaskDetailswindow.SetActive(false);
    }

    void OnToggleChanged(bool isOn)
    {
        // Show the window if toggle is on, hide it if toggle is off
        TaskDetailswindow.SetActive(isOn);
        Debug.Log($"Window is now {(isOn ? "visible" : "hidden")}");
    }
}



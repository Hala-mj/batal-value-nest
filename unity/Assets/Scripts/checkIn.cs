using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WindowManager1 : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField emailField;
    public TMP_InputField phoneNumberField;
    public TMP_InputField passwordField;
    public TMP_InputField lastNameField;
    public TMP_InputField firstNameField;
    public TMP_InputField categoryField;

    [Header("Buttons")]
    public Button signUpButton;

    [Header("Warning Window")]
    public GameObject warningWindow; // Reference to the warning/check window
    public TextMeshProUGUI warningMessage; // TextMeshPro component for the warning message

    void Start()
    {
        // Validate all references
        if (emailField == null || phoneNumberField == null || passwordField == null || lastNameField == null ||
            firstNameField == null || categoryField == null || signUpButton == null || warningWindow == null || warningMessage == null)
        {
            Debug.LogError("Please assign all required references in the Inspector.");
            return;
        }

        // Ensure the warning window is initially hidden
        warningWindow.SetActive(false);

        // Add listener to the sign-up button
        signUpButton.onClick.AddListener(CheckInputFields);
    }

    // Method to check if all input fields are filled
    void CheckInputFields()
    {
        // Check if any field is empty
        if (string.IsNullOrEmpty(emailField.text.Trim()) ||
            string.IsNullOrEmpty(phoneNumberField.text.Trim()) ||
            string.IsNullOrEmpty(passwordField.text.Trim()) ||
            string.IsNullOrEmpty(lastNameField.text.Trim()) ||
            string.IsNullOrEmpty(firstNameField.text.Trim()) ||
            string.IsNullOrEmpty(categoryField.text.Trim()))
        {
            // Show the warning window
            ShowWarning("Please fill out all fields before proceeding.");
        }
        else
        {
            // Proceed with sign-up logic
            Debug.Log("All fields are filled. Proceeding with sign-up...");
            // Add your sign-up logic here
        }
    }

    // Method to show the warning window
    void ShowWarning(string message)
    {
        warningMessage.text = message;
        warningWindow.SetActive(true);
        // Optionally hide the warning window after a delay
        StartCoroutine(HideWarningWindowAfterDelay(3f));
    }

    // Coroutine to hide the warning window after a delay
    System.Collections.IEnumerator HideWarningWindowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningWindow.SetActive(false);
    }
}

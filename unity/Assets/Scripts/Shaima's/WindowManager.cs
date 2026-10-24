using UnityEngine;
using TMPro; // TextMeshPro namespace
using UnityEngine.UI; // For Button and other UI components
using System.Collections; // For coroutine support

public class WindowManager : MonoBehaviour
{
    [Header("Main Window")]
    public Transform textFieldParent;         // Parent object for dynamically added text fields
    public GameObject textFieldPrefab;        // Prefab for TextMeshPro text field
    public Button plusButton;                 // "+" button to open the Input Window

    [Header("Input Window")]
    public GameObject inputWindow;            // Reference to the input window
    public TMP_InputField userInputField;     // TextMeshPro Input Field for the user's value
    public Button doneButton;                 // "Done" button to confirm input
    public Button cancelButton;               // "Cancel" button to close the input window
    public Image warningImage;                // Image to display when input is empty
    public Image maxInputsWarning;  // Text to display when the input limit is reached

    private int inputCount = 0;               // Counter for inputs added
    private const int maxInputs = 3;          // Maximum number of inputs allowed

    void Start()
    {
        // Validate all references
        if (textFieldParent == null || textFieldPrefab == null || plusButton == null || inputWindow == null || userInputField == null || doneButton == null || cancelButton == null || warningImage == null || maxInputsWarning == null)
        {
            Debug.LogError("Please assign all required references in the Inspector.");
            return;
        }

        // Ensure the input window and warning image are initially hidden
        inputWindow.SetActive(false);
        warningImage.gameObject.SetActive(false);
        maxInputsWarning.gameObject.SetActive(false);

        // Add listeners to the buttons
        plusButton.onClick.AddListener(HandlePlusButton);
        doneButton.onClick.AddListener(AddNewInputValue);
        cancelButton.onClick.AddListener(CloseInputWindow);
    }

    // Method to handle the "+" button click
    void HandlePlusButton()
    {
        // Check if the maximum number of inputs has been reached
        if (inputCount >= maxInputs)
        {
            ShowMaxInputsWarning();
            return;
        }

        OpenInputWindow();
    }

    // Method to open the input window
    void OpenInputWindow()
    {
        inputWindow.SetActive(true);           // Show the input window
        userInputField.text = "";              // Clear the input field
        userInputField.ActivateInputField();   // Focus on the input field
        warningImage.gameObject.SetActive(false); // Hide the warning image
    }

    // Method to close the input window
    void CloseInputWindow()
    {
        inputWindow.SetActive(false);          // Hide the input window
        warningImage.gameObject.SetActive(false); // Hide the warning image
    }

    // Method to handle the "Done" button click
    void AddNewInputValue()
    {
        // Check if the maximum number of inputs has been reached
        if (inputCount >= maxInputs)
        {
            ShowMaxInputsWarning();
            return;
        }

        // Retrieve user input
        string enteredValue = userInputField.text.Trim();

        // Check if the input is empty
        if (string.IsNullOrEmpty(enteredValue))
        {
            // Show the warning image
            warningImage.gameObject.SetActive(true);
            // Start coroutine to hide the warning image after 5 seconds
            StartCoroutine(HideWarningImageAfterDelay(5f));
            return;
        }

        // Instantiate a new text field in the main window
        GameObject newTextField = Instantiate(textFieldPrefab, textFieldParent);

        // Set the text of the new TMP_Text component
        TMP_Text tmpTextComponent = newTextField.GetComponent<TMP_Text>();
        if (tmpTextComponent != null)
        {
            tmpTextComponent.text = enteredValue;
        }
        else
        {
            Debug.LogError("TextFieldPrefab does not contain a TMP_Text component.");
            return;
        }

        // Increment the input counter
        inputCount++;

        // Close the input window after adding the value
        CloseInputWindow();
    }

    // Show warning when maximum inputs are reached
    void ShowMaxInputsWarning()
    {
        maxInputsWarning.gameObject.SetActive(true);
        StartCoroutine(HideMaxInputsWarningAfterDelay(5f));
    }

    // Coroutine to hide the warning image after a delay
    IEnumerator HideWarningImageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningImage.gameObject.SetActive(false);
    }

    // Coroutine to hide the max inputs warning after a delay
    IEnumerator HideMaxInputsWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        maxInputsWarning.gameObject.SetActive(false);
    }
}

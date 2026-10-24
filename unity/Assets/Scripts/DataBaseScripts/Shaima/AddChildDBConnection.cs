using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AddChildDBConnection : MonoBehaviour
{
    public TMP_InputField nameInput;  // Input field for child's name
    public Slider ageSlider;          // Slider for selecting age
    public Button[] avatarButtons;    // Avatar selection buttons
    public Button submitButton;       // Submit button

    private int selectedAvatarID = 0;  // Default avatar ID
    private int parentID = 0;          // Dynamic ParentID
    private string url = "http://localhost/BVN/add_child.php";  // URL for PHP backend

    void Start()
    {
        parentID = PlayerPrefs.GetInt("ParentID", 0); // Retrieve ParentID dynamically
        if (parentID == 0)
        {
            Debug.LogError("ParentID is not set!");
            submitButton.interactable = false;
            return;
        }

        // Set up listeners for avatar selection buttons
        for (int i = 0; i < avatarButtons.Length; i++)
        {
            int iconID = i + 1;  // Map button index to IconID in your database (1-based index)
            avatarButtons[i].onClick.AddListener(() => OnAvatarSelected(iconID));
        }

        // Set up listener for submit button
        submitButton.onClick.AddListener(OnSubmitClicked);
    }

    void OnAvatarSelected(int avatarID)
    {
        selectedAvatarID = avatarID; // Assign the selected IconID
        Debug.Log("Selected Avatar ID: " + avatarID);
    }

    void OnSubmitClicked()
    {
        string name = nameInput.text.Trim();
        int age = (int)ageSlider.value;

        if (string.IsNullOrEmpty(name) || age <= 0 || selectedAvatarID <= 0)
        {
            Debug.Log("Please fill in all the fields correctly!");
            return;
        }

        StartCoroutine(SendDataToPHP(name, age, selectedAvatarID));
    }

    IEnumerator SendDataToPHP(string name, int age, int avatarID)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("age", age.ToString());
        form.AddField("avatarID", avatarID.ToString());
        form.AddField("parentID", parentID.ToString());

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Data sent successfully: " + jsonResponse);
            Debug.Log("Child added successfully!");
        }
        else
        {
            Debug.LogError("Error: " + www.error);
            Debug.Log("Failed to add child. Please try again.");
        }
    }
}

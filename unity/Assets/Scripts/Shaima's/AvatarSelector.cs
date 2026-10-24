using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AvatarSelector : MonoBehaviour
{
    public GameObject avatarSelectionPanel;  // The panel with avatars
    public Image selectedAvatarImage;        // The image that shows the selected avatar (the "default avatar")
    public GameObject[] avatarImages;        // Array of avatar Image GameObjects (in the same position as the default avatar)
    public Sprite[] avatarSprites;           // Array of avatar Sprites for the avatars
    public Button cancelButton;
    private int selectedAvatarID = 0;        // Variable to store the selected avatar ID
    private string url = "http://localhost/BVN/db_connect.php";  // URL for the PHP backend

    void Start()
    {
        // Hide all avatars initially
        DisableAllAvatarsExceptDefault();
    }

    // This will be called when the "Change" button is clicked
    public void OpenAvatarSelection()
    {
        avatarSelectionPanel.SetActive(true);  // Show the avatar selection panel
        cancelButton.gameObject.SetActive(true);
        DisableAllAvatarsExceptDefault();      // Disable all avatars except the default
    }

    // This will disable all avatars except the default avatar
    private void DisableAllAvatarsExceptDefault()
    {
        for (int i = 0; i < avatarImages.Length; i++)
        {
            if (i == 0)  // Assuming the default avatar is at index 0
            {
                avatarImages[i].SetActive(true);  // Enable the default avatar
            }
            else
            {
                avatarImages[i].SetActive(false);  // Disable all other avatars
            }
        }
    }

    // This will be called when an avatar is selected
    public void OnAvatarSelected(int avatarID)
    {
        selectedAvatarID = avatarID;  // Store the selected avatar ID
        Debug.Log("Selected Avatar ID: " + avatarID);

        // Disable all avatar images (except the selected one)
        for (int i = 0; i < avatarImages.Length; i++)
        {
            if (i == avatarID)
            {
                avatarImages[i].SetActive(true);  // Enable the selected avatar
                // Update the selected avatar image
                selectedAvatarImage.sprite = avatarSprites[i];  // Update the default avatar image
            }
            else
            {
                avatarImages[i].SetActive(false);  // Disable all other avatars
            }
        }

        CloseAvatarSelection();  // Close the selection panel

        // After selecting the avatar, send the avatarID with other data
        SubmitAvatarSelection(selectedAvatarID);
    }

    // This will close the avatar selection panel
    public void CloseAvatarSelection()
    {
        avatarSelectionPanel.SetActive(false);  // Hide the avatar selection panel
        cancelButton.gameObject.SetActive(false);
    }

    private void SubmitAvatarSelection(int avatarID)
    {
        StartCoroutine(SendDataToPHP(avatarID));
    }

    IEnumerator SendDataToPHP(int avatarID)
    {
        WWWForm form = new WWWForm();
        form.AddField("avatarID", avatarID.ToString());  // Add the selected avatarID

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        // Check if the request was successful
        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Data sent successfully: " + jsonResponse);
            // Handle the response if needed
        }
        else
        {
            Debug.Log("Error sending data: " + www.error);
        }
    }
}

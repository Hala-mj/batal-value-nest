using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class signOut : MonoBehaviour
{
    private string logoutUrl = "http://localhost/BVN/logout.php";
    public TMP_InputField userInputField;
    public UnityEngine.UI.Button logoutButton;
    public GameObject errorImage;
    public GameObject emptyFieldsImage;
    public string nextSceneName;

    void Start()
    {
        errorImage.SetActive(false);
        emptyFieldsImage.SetActive(false);
        logoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }

    void OnLogoutButtonClicked()
    {
        string userInput = userInputField.text.Trim();

        if (string.IsNullOrEmpty(userInput))
        {
            StartCoroutine(ShowEmptyFieldsImage());
            Debug.LogWarning("User field is empty.");
            return;
        }

        StartCoroutine(LogoutUser(userInput));
    }

    IEnumerator LogoutUser(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        UnityWebRequest www = UnityWebRequest.Post(logoutUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text.Trim();
            Debug.Log("Server Response: " + response);

            if (response.Contains("تم تسجيل الخروج بنجاح"))
            {
                Debug.Log("Logout successful!");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("Logout failed: " + response);
                StartCoroutine(ShowErrorImage("Logout failed!"));
            }
        }
        else
        {
            Debug.LogError("Network Error: " + www.error);
            StartCoroutine(ShowErrorImage("Network error!"));
        }
    }

    IEnumerator ShowErrorImage(string message)
    {
        errorImage.SetActive(true);
        Debug.LogError(message);
        yield return new WaitForSeconds(6);
        errorImage.SetActive(false);
    }

    IEnumerator ShowEmptyFieldsImage()
    {
        emptyFieldsImage.SetActive(true);
        yield return new WaitForSeconds(6);
        emptyFieldsImage.SetActive(false);
    }
}

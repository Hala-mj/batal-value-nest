using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class signIn : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public UnityEngine.UI.Button loginButton;
    public GameObject errorImage;
    public GameObject emptyFieldsImage;
    public GameObject invalidEmailImage;
    public GameObject emailNotFoundImage;
    public GameObject incorrectPasswordImage;
    public string nextSceneName = "Parent Account"; // ✅ Ensure scene name is set correctly

    private string loginUrl = "http://localhost/BVN/login.php";
    private string fetchChildrenUrl = "http://localhost/BVN/fetch_children.php";

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        HideAllImages();
    }

    void OnLoginButtonClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            StartCoroutine(ShowImage(emptyFieldsImage));
            Debug.Log("الرجاء إدخال البريد الإلكتروني وكلمة المرور.");
            return;
        }

        StartCoroutine(Login(email, password));
    }

    IEnumerator Login(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(loginUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text.Trim();
            Debug.Log("Response from server: " + response);

            try
            {
                LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);

                if (loginResponse.success)
                {
                    Debug.Log("✅ Login successful!");

                    // Store user info in PlayerPrefs
                    PlayerPrefs.SetString("userEmail", email); // Use the entered email
                    PlayerPrefs.SetInt("ParentID", loginResponse.ParentID);
                    PlayerPrefs.Save();

                    Debug.Log("Stored ParentID: " + loginResponse.ParentID);
                    Debug.Log("Stored email: " + email);

                    // Fetch children before loading next scene
                    StartCoroutine(FetchChildrenAndLoadScene(loginResponse.ParentID));
                }
                else
                {
                    Debug.LogError("❌ Login failed: " + loginResponse.message);
                    HandleLoginError(loginResponse.message);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("❌ JSON Parsing Error: " + ex.Message);
                StartCoroutine(ShowImage(errorImage));
            }
        }
        else
        {
            Debug.LogError("❌ Error: " + www.error);
            StartCoroutine(ShowImage(errorImage));
        }
    }

    IEnumerator FetchChildrenAndLoadScene(int parentID)
    {
        WWWForm form = new WWWForm();
        form.AddField("parentID", parentID);

        UnityWebRequest www = UnityWebRequest.Post(fetchChildrenUrl, form);
        yield return www.SendWebRequest();

        bool hasChildren = false; // ✅ Default to false to prevent blocking login

        if (www.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Children Response: " + jsonResponse);

            try
            {
                ChildrenResponse response = JsonUtility.FromJson<ChildrenResponse>(jsonResponse);
                if (response.success && response.children != null && response.children.Count > 0)
                {
                    Debug.Log("✅ Children fetched successfully: " + response.children.Count);
                    hasChildren = true;
                }
                else
                {
                    Debug.LogWarning("⚠️ No children found. Parent will still access 'Parent Account'.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("❌ JSON Parsing Error: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("❌ Failed to fetch children: " + www.error);
        }

        // ✅ Always load "Parent Account" even if no children exist
        Debug.Log("🔄 Redirecting to Parent Account...");
        yield return new WaitForSeconds(1); // ✅ Small delay for UI updates
        SceneManager.LoadScene(nextSceneName); // ✅ Ensure this always happens
    }

    private void HandleLoginError(string message)
    {
        HideAllImages();

        if (message.Contains("email_not_found"))
        {
            StartCoroutine(ShowImage(emailNotFoundImage));
        }
        else if (message.Contains("incorrect_password"))
        {
            StartCoroutine(ShowImage(incorrectPasswordImage));
        }
        else if (message.Contains("invalid_email"))
        {
            StartCoroutine(ShowImage(invalidEmailImage));
        }
        else
        {
            StartCoroutine(ShowImage(errorImage));
        }
    }

    private void HideAllImages()
    {
        errorImage.SetActive(false);
        emptyFieldsImage.SetActive(false);
        invalidEmailImage.SetActive(false);
        emailNotFoundImage.SetActive(false);
        incorrectPasswordImage.SetActive(false);
    }

    IEnumerator ShowImage(GameObject image)
    {
        image.SetActive(true);
        yield return new WaitForSeconds(6);
        image.SetActive(false);
    }

    [System.Serializable]
    public class LoginResponse
    {
        public bool success;
        public int ParentID;
        public string Fname;
        public string Lname;
        public string PhoneNo;
        public string Email;
        public string Classification;
        public string message;
    }

    [System.Serializable]
    public class ChildrenResponse
    {
        public bool success;
        public string message;
        public List<Child> children;
    }

    [System.Serializable]
    public class Child
    {
        public int ChildID;
        public string Name;
        public string Avatar;
    }
}

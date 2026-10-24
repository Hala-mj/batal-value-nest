using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class FetchUserData : MonoBehaviour
{
    public TMP_Text fnameInput;
    public TMP_Text lnameInput;
    public TMP_Text phoneInput;
    public TMP_Text emailDisplay;
    public TMP_Text classInput;

    private string fetchUrl = "http://localhost/BVN/fetch_users.php";

    void Start()
    {
        // **Log and ensure PlayerPrefs stores the correct email**
        string email = PlayerPrefs.GetString("userEmail", "");
        Debug.Log("Email stored in PlayerPrefs: " + email);

        if (!string.IsNullOrEmpty(email))
        {
            FetchData(email);
        }
        else
        {
            Debug.LogError("Error: No email found in PlayerPrefs!");
        }
    }

    public void FetchData(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            Debug.LogError("Email is empty or null.");
            return;
        }

        StartCoroutine(FetchDataFromDatabase(email));
    }

    IEnumerator FetchDataFromDatabase(string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);

        Debug.Log("Email being sent from Unity: " + email);

        UnityWebRequest www = UnityWebRequest.Post(fetchUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text.Trim();
            Debug.Log("Server Response: " + response);

            try
            {
                FetchResponse fetchResponse = JsonUtility.FromJson<FetchResponse>(response);

                if (fetchResponse == null || fetchResponse.user == null)
                {
                    Debug.LogError("Error: JSON Parsing returned null! Check API response.");
                    yield break;
                }

                if (fetchResponse.success)
                {
                    DisplayUserData(fetchResponse.user); // **Pass only the user object**
                }
                else
                {
                    Debug.LogError("Error: " + fetchResponse.message);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON Parsing Error: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Error fetching data: " + www.error);
        }
    }

    private void DisplayUserData(UserData user)
    {
        if (user == null)
        {
            Debug.LogError("Error: User data is null!");
            return;
        }

        fnameInput.text = user.Fname;
        lnameInput.text = user.Lname;
        phoneInput.text = user.PhoneNo;
        emailDisplay.text = user.Email;
        classInput.text = user.Classification;

        Debug.Log($"User data displayed: {user.Fname} {user.Lname}");
    }

    [System.Serializable]
    public class FetchResponse
    {
        public bool success;
        public string message;
        public UserData user;  // **Fixed nested structure**
    }

    [System.Serializable]
    public class UserData
    {
        public string Fname;
        public string Lname;
        public string Email;
        public string PhoneNo;
        public string Classification;
    }
}

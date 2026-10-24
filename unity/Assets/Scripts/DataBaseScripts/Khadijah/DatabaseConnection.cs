using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class DatabaseConnection : MonoBehaviour
{
    public TMP_InputField fnameInput;
    public TMP_InputField lnameInput;
    public TMP_InputField phoneInput;
    public TMP_InputField emailInput;
    public TMP_InputField passWordInput;
    public TMP_InputField classInput;

    public Button submitButton;
    public GameObject errorImage; // Image for general errors
    public GameObject invalidEmailImage; // Image for invalid email format
    public GameObject emailExistsImage; // Image for duplicate email

    public string nextSceneName; // Scene to load after successful signup

    private string signupUrl = "http://localhost/BVN/signup.php"; // PHP signup endpoint

    void Start()
    {
        // Add listener to the submit button
        submitButton.onClick.AddListener(OnSubmitClicked);

        // Hide all error images at the start
        errorImage.SetActive(false);
        invalidEmailImage.SetActive(false);
        emailExistsImage.SetActive(false);
    }

    void OnSubmitClicked()
    {
        // Collect data from input fields
        string fname = fnameInput.text.Trim();
        string lname = lnameInput.text.Trim();
        string phone = phoneInput.text.Trim();
        string email = emailInput.text.Trim();
        string password = passWordInput.text.Trim();
        string classification = classInput.text.Trim();

        // Validate empty fields
        if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname) || string.IsNullOrEmpty(phone) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(classification))
        {
            StartCoroutine(ShowImage(errorImage));
            Debug.Log("Error: One or more fields are empty.");
            return;
        }

        // Validate email format
        if (!IsValidEmail(email))
        {
            StartCoroutine(ShowImage(invalidEmailImage));
            Debug.Log("Error: Invalid email format.");
            return;
        }

        // Hide all error images before sending the data
        errorImage.SetActive(false);
        invalidEmailImage.SetActive(false);
        emailExistsImage.SetActive(false);

        // Start the signup process
        StartCoroutine(SendDataToPHP(fname, lname, phone, email, password, classification));
    }

    IEnumerator SendDataToPHP(string fname, string lname, string phone, string email, string password, string classification)
    {
        WWWForm form = new WWWForm();
        form.AddField("fname", fname);
        form.AddField("lname", lname);
        form.AddField("phone", phone);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("Classification", classification);

        Debug.Log($"Sending data: fname={fname}, lname={lname}, phone={phone}, email={email}, password={password}, Classification={classification}");

        UnityWebRequest www = UnityWebRequest.Post(signupUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text.Trim();
            Debug.Log("Response: " + response);

            if (response.Contains("email_exists"))
            {
                StartCoroutine(ShowImage(emailExistsImage));
                Debug.Log("Error: Email already exists in the database.");
                yield break;
            }

            if (response.Contains("success"))
            {
                Debug.Log("Signup successful!");
                SignupResponse signupResponse = JsonUtility.FromJson<SignupResponse>(response);
                PlayerPrefs.SetInt("ParentID", signupResponse.ParentID);
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                StartCoroutine(ShowImage(errorImage));
                Debug.Log("Error: Failed to create an account. Response: " + response);
            }
        }
        else
        {
            StartCoroutine(ShowImage(errorImage));
            Debug.LogError("Error: " + www.error);
        }
    }



// Validate email format using a regex
private bool IsValidEmail(string email)
    {
        // تعديل النمط للتحقق من أن المجال يحتوي على "gmail.com"
        string emailPattern = @"^[^@\s]+@gmail\.com$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Show an error image for a specified duration
    IEnumerator ShowImage(GameObject image)
    {
        image.SetActive(true);
        yield return new WaitForSeconds(6);
        image.SetActive(false);
    }

    // Class to parse server response
    [System.Serializable]
    public class SignupResponse
    {
        public bool success;
        public int ParentID;
        public string message;
    }
}

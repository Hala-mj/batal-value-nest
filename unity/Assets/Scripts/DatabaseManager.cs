using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UserDataSender : MonoBehaviour
{
    public string phpUrl = "http://localhost/insert.php"; // Replace with your PHP script URL
    public string username;
    public string email;
    public string password;

    public void SendUserData()
    {
        StartCoroutine(SendDataCoroutine());
    }

    IEnumerator SendDataCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);  // Use proper hashing and security

        using (UnityWebRequest www = UnityWebRequest.Post(phpUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data sent successfully: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChildHome : MonoBehaviour
{
    public TMP_Text childNameText;
    public TMP_Text childAgeText;
    public TMP_Text childCoinsText;
    public Image childAvatarImage;

    private string fetchChildDataUrl = "http://localhost/BVN/child_home.php";

    void Start()
    {
        int childID = PlayerPrefs.GetInt("ChildID", 0);

        if (childID > 0)
        {
            Debug.Log("Retrieved ChildID: " + childID);
            StartCoroutine(FetchChildDetails(childID));
        }
    }

    IEnumerator FetchChildDetails(int childID)
    {
        WWWForm form = new WWWForm();
        form.AddField("childID", childID);

        Debug.Log("Fetching data for ChildID: " + childID);

        UnityWebRequest www = UnityWebRequest.Post(fetchChildDataUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text.Trim();
            Debug.Log("Server Response: " + response);

            try
            {
                ChildResponse childResponse = JsonUtility.FromJson<ChildResponse>(response);

                if (childResponse.success)
                {
                    DisplayChildData(childResponse.child);
                }
                else
                {
                    Debug.LogError("Error: " + childResponse.message);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON Parsing Error: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Error fetching child data: " + www.error);
        }
    }

    private void DisplayChildData(ChildData child)
    {
        if (child == null)
        {
            Debug.LogError("Error: Child data is null!");
            return;
        }

        childNameText.text = child.Name;
        childAgeText.text = "Age: " + child.Age.ToString();
        childCoinsText.text = "Coins: " + child.Coins.ToString();
        StartCoroutine(LoadAvatarImage(child.IconPath));
    }

    IEnumerator LoadAvatarImage(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("http://localhost" + url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            childAvatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        else
        {
            Debug.LogError("Error loading avatar image: " + www.error);
        }
    }

    [System.Serializable]
    public class ChildResponse
    {
        public bool success;
        public ChildData child;
        public string message;
    }

    [System.Serializable]
    public class ChildData
    {
        public string Name;
        public int Age;
        public int Coins;
        public string IconPath;
    }
}

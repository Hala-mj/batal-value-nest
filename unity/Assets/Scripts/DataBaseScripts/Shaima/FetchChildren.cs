using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FetchChildren : MonoBehaviour
{
    public Transform childContainer; // UI Parent to display children
    public GameObject childPrefab; // Prefab to display each child

    private string fetchUrl = "http://localhost/BVN/fetch_children.php";

    void Start()
    {
        int parentID = PlayerPrefs.GetInt("ParentID", 0);
        Debug.Log("Stored ParentID: " + parentID);

        if (parentID > 0)
        {
            FetchData(parentID);
        }
        else
        {
            Debug.LogError("ParentID is missing!");
        }
    }

    public void FetchData(int parentID)
    {
        StartCoroutine(FetchDataFromDatabase(parentID));
    }

    IEnumerator FetchDataFromDatabase(int parentID)
    {
        WWWForm form = new WWWForm();
        form.AddField("parentID", parentID.ToString());

        Debug.Log("Fetching children for ParentID: " + parentID);

        UnityWebRequest www = UnityWebRequest.Post(fetchUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text.Trim();
            Debug.Log("Server Response: " + response);

            try
            {
                FetchResponse fetchResponse = JsonUtility.FromJson<FetchResponse>(response);

                if (fetchResponse.success)
                {
                    DisplayChildren(fetchResponse.children);
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

    private void DisplayChildren(List<ChildData> children)
    {
        if (childContainer == null)
        {
            Debug.LogError("Error: ChildContainer is NULL! Assign it in the Inspector.");
            return;
        }

        // Clear previous children before adding new ones
        foreach (Transform child in childContainer)
        {
            Destroy(child.gameObject);
        }

        if (children.Count == 0)
        {
            Debug.LogWarning("No children available. UI remains empty.");
            return;
        }

        foreach (ChildData child in children)
        {
            GameObject childEntry = Instantiate(childPrefab, childContainer);
            TMP_Text nameText = childEntry.transform.Find("Button/ChildName")?.GetComponent<TMP_Text>();
            Image avatarImage = childEntry.transform.Find("Button/ChildAvatar")?.GetComponent<Image>();
            Button childButton = childEntry.transform.Find("Button")?.GetComponent<Button>();

            if (nameText == null || avatarImage == null || childButton == null)
            {
                Debug.LogError("Error: Missing TMP_Text, Image, or Button component!");
                continue;
            }

            nameText.text = child.Name;
            StartCoroutine(LoadImage(child.Avatar, avatarImage));
        }
    }

  IEnumerator LoadImage(string url, Image image)
{
    if (image == null)
    {
        Debug.LogError("Error: Image component is NULL!");
        yield break;
    }

    // Ensure the URL is formatted correctly
    string fullUrl;
    if (!url.StartsWith("http"))
    {
        // Append localhost if the URL is relative
        fullUrl = "http://127.0.0.1" + url;
    }
    else
    {
        // Use the provided absolute URL
        fullUrl = url;
    }

    Debug.Log("Trying to load image from: " + fullUrl);

    UnityWebRequest www = UnityWebRequestTexture.GetTexture(fullUrl);
    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.Success)
    {
        Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        Debug.Log("Image successfully loaded: " + fullUrl);
    }
    else
    {
        Debug.LogError("Failed to load image: " + www.error + " | URL: " + fullUrl);
    }
}


}

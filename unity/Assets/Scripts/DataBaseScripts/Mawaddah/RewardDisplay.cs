using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class RewardDisplay : MonoBehaviour
{
    public GameObject rewardPrefab; // Prefab to instantiate
    public Transform rewardParent; // Parent where rewards will be added
    public Slider progressBar; // Reference to the progress bar
    private string apiUrl = "http://localhost/BVN/get_rewards.php"; // Change to your PHP URL
    private int childId = 1; // Replace with the logged-in child ID

    void Update()
    {
        if (progressBar.value >= 1.0f) // Check if progress is 100%
        {
            StartCoroutine(FetchRewards());
        }
    }

    IEnumerator FetchRewards()
    {
        string url = $"{apiUrl}?child_id={childId}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching rewards: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            RewardData[] rewards = JsonHelper.FromJson<RewardData>(jsonResponse);
            DisplayRewards(rewards);
        }
    }

    void DisplayRewards(RewardData[] rewards)
    {
        foreach (Transform child in rewardParent)
        {
            Destroy(child.gameObject); // Clear existing rewards
        }

        foreach (RewardData reward in rewards)
        {
            GameObject rewardObj = Instantiate(rewardPrefab, rewardParent);
            rewardObj.transform.Find("RewardTitle").GetComponent<TextMeshProUGUI>().text = reward.title;
            rewardObj.transform.Find("RewardDescription").GetComponent<TextMeshProUGUI>().text = reward.description;
            rewardObj.transform.Find("RewardIcon").GetComponent<Image>().sprite = LoadSpriteFromPath(reward.iconPath);
        }
    }

    Sprite LoadSpriteFromPath(string path)
    {
        Texture2D texture = new Texture2D(2, 2);
        byte[] imageData = System.IO.File.ReadAllBytes(path);
        texture.LoadImage(imageData);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    [System.Serializable]
    public class RewardData
    {
        public string title;
        public string description;
        public string iconPath;
    }
}

// Helper class to parse JSON arrays
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string wrappedJson = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

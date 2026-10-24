using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class DynamicProgress : MonoBehaviour
{
    public GameObject progressPrefab; // Reference to your progress prefab
    public Transform contentParent; // Parent object to hold the dynamic prefabs
    public string databaseUrl = "https://localhost/BVN/getValues.php"; // URL to fetch values data
    public int childID; // The ID of the logged-in child

    private float totalWidth = 300f; // Total width of the progress bar (adjust as needed)

    void Start()
    {
        // Fetch data and populate progress bars
        StartCoroutine(FetchValueData());
    }

    IEnumerator FetchValueData()
    {
        // Prepare the form data
        WWWForm form = new WWWForm();
        form.AddField("childID", childID);

        // Send the request to the server
        UnityWebRequest www = UnityWebRequest.Post(databaseUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response
            string jsonResult = www.downloadHandler.text;
            ValueData[] values = JsonHelper.FromJson<ValueData>(jsonResult);

            // Dynamically create a progress bar for each value
            foreach (ValueData value in values)
            {
                // Instantiate a new prefab
                GameObject progressInstance = Instantiate(progressPrefab, contentParent);

                // Get references to UI components within the prefab
                RectTransform progressBar = progressInstance.transform.Find("progress bar").GetComponent<RectTransform>();
                Text progressLabel = progressInstance.transform.Find("prog disc").GetComponent<Text>();

                // Calculate the progress based on the value's current and max values
                float progress = (float)value.currentValue / value.maxValue;
                progressBar.sizeDelta = new Vector2(totalWidth * progress, progressBar.sizeDelta.y);

                // Update the label with the value name
                progressLabel.text = value.valueName;
            }
        }
        else
        {
            Debug.LogError($"Error fetching value data: {www.error}");
        }
    }

    [System.Serializable]
    public class ValueData
    {
        public string valueName; // Name of the value
        public int currentValue; // Current progress of the value
        public int maxValue; // Maximum value for completion
    }

    // Helper class to parse JSON arrays
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"Items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}

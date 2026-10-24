using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json; // Add Newtonsoft.Json for JSON parsing
using TMPro; // For TextMeshPro

public class TaskDisplay : MonoBehaviour
{
    public TMP_Text tasksText; // Reference to the TextMeshPro UI element
    public string phpUrl = "http://localhost/RetrieveTasks.php"; // URL to your PHP script

    void Start()
    {
        StartCoroutine(GetTasks());
    }

    IEnumerator GetTasks()
    {
        // Send a GET request to the PHP script
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(phpUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                // Parse JSON response
                string jsonResponse = www.downloadHandler.text;
                List<string> tasks = JsonConvert.DeserializeObject<List<string>>(jsonResponse);

                // Display tasks in the UI
                tasksText.text = string.Join("\n", tasks);
            }
            else
            {
                Debug.LogError("Error retrieving tasks: " + www.error);
            }
        }
    }
}

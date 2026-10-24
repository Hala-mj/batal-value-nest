using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Tasks : MonoBehaviour
{
    public GameObject taskPrefab; // Reference to the task prefab
    public Transform contentParent; // Parent to hold the dynamically created tasks
    public string databaseUrl = "https://yourserver/getTasks.php"; // URL to fetch tasks from the database
    public int childID; // The ID of the child whose tasks are being fetched
    public Sprite checkedImage; // Image to display when the task is marked as done
    public Sprite uncheckedImage; // Image to display when the task is not done

    void Start()
    {
        // Fetch tasks and populate the task list dynamically
        StartCoroutine(FetchTasks());
    }

    IEnumerator FetchTasks()
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
            TaskData[] tasks = JsonHelper.FromJson<TaskData>(jsonResult);

            // Create a task prefab for each task in the database
            foreach (TaskData task in tasks)
            {
                GameObject taskInstance = Instantiate(taskPrefab, contentParent);

                // Set the task description
                Text taskDescription = taskInstance.transform.Find("Task disc").GetComponent<Text>();
                taskDescription.text = task.description;

                // Set the button image and functionality
                Button taskButton = taskInstance.transform.Find("child chk").GetComponent<Button>();
                Image buttonImage = taskButton.GetComponent<Image>();
                buttonImage.sprite = task.isDone ? checkedImage : uncheckedImage;

                // Add button functionality
                taskButton.onClick.AddListener(() =>
                {
                    // Toggle task state and update button image
                    task.isDone = !task.isDone;
                    buttonImage.sprite = task.isDone ? checkedImage : uncheckedImage;

                    // Optional: Save task state to the database
                    StartCoroutine(UpdateTaskState(task.id, task.isDone));
                });
            }
        }
        else
        {
            Debug.LogError($"Error fetching tasks: {www.error}");
        }
    }

    IEnumerator UpdateTaskState(int taskId, bool isDone)
    {
        // Prepare the form data
        WWWForm form = new WWWForm();
        form.AddField("taskId", taskId);
        form.AddField("isDone", isDone ? 1 : 0);

        // Send the request to update task state
        UnityWebRequest www = UnityWebRequest.Post("https://localhost/BVN/updateTaskState.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error updating task state: {www.error}");
        }
    }

    [System.Serializable]
    public class TaskData
    {
        public int id; // Task ID
        public string description; // Task description
        public bool isDone; // Whether the task is marked as done
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

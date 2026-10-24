using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;

public class ToggleWindow : MonoBehaviour
{
    public Button[] valueButtons;         // Array to hold your 7 predefined value buttons
    public GameObject[] valueCanvases;    // Array to hold your 7 predefined canvases
    public GameObject buttonCanvas;       // Canvas that holds all the buttons
    public Button[] cancelButtons;        // Array of Cancel buttons in each value canvas
    public GameObject taskDetailsCanvas;  // Canvas to display task details

    [SerializeField]
    private List<TaskButtonGroup> taskButtonGroups; // List of groups for task buttons

    private List<ValueData> values;       // List to store fetched values
    private List<TaskData> tasks;         // List to store tasks for each ValueID

    

    void Start()
    {
        // Ensure all canvases are initially disabled except the button canvas
        for (int i = 0; i < valueCanvases.Length; i++)
        {
            valueCanvases[i].SetActive(false);  // Hide all canvases
        }

        buttonCanvas.SetActive(true);  // Show the button canvas (with all value buttons)
        taskDetailsCanvas.SetActive(false);  // Hide task details canvas initially

        // Add listener to each Cancel button in every canvas
        if (cancelButtons.Length == valueCanvases.Length)
        {
            for (int i = 0; i < cancelButtons.Length; i++)
            {
                int index = i;  // Capture the correct index
                cancelButtons[i].onClick.AddListener(() => CloseTaskWindow(index));
            }
        }
        else
        {
            Debug.LogError("Cancel buttons and canvases count mismatch!");
        }

        // Fetch values from the database
        StartCoroutine(FetchValuesFromDatabase());
    }

    void OnTaskButtonClicked(int taskIndex)
    {
        Debug.Log($"Task with index {taskIndex} selected.");

        // Show task details canvas
        taskDetailsCanvas.SetActive(true);

        // Optional: Hide other canvases if needed
        foreach (var canvas in valueCanvases)
        {
            canvas.SetActive(false);
        }
    }

    void CloseTaskDetails()
    {
        taskDetailsCanvas.SetActive(false);

        // Optional: Show other canvases or buttons if needed
        buttonCanvas.SetActive(true);
    }

    IEnumerator FetchValuesFromDatabase()
    {
        string url = "http://localhost/BVN/getValues.php"; // Update with the correct path

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load values: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            values = JsonUtility.FromJson<ValueList>(jsonResponse).values;

            Debug.Log($"Fetched {values.Count} values from the database.");

            for (int i = 0; i < valueButtons.Length; i++)
            {
                if (i < values.Count)
                {
                    RTLTextMeshPro buttonText = valueButtons[i].GetComponentInChildren<RTLTextMeshPro>();
                    if (buttonText != null)
                    {
                        buttonText.text = values[i].ValueName;

                        int index = i;  // Capture the correct index
                        valueButtons[i].onClick.AddListener(() => OnValueButtonClicked(index));
                    }
                    else
                    {
                        Debug.LogError($"No RTLTextMeshPro found on value button {i}");
                    }
                }
            }
        }
    }

    void OnValueButtonClicked(int index)
    {
        Debug.Log($"Value with ID: {values[index].ValueID} is selected.");

        StartCoroutine(FetchTasksForValue(values[index].ValueID, index));

        buttonCanvas.SetActive(false);

        for (int i = 0; i < valueCanvases.Length; i++)
        {
            valueCanvases[i].SetActive(i == index); // Show only the selected canvas
        }
    }

    IEnumerator FetchTasksForValue(int valueID, int valueIndex)
    {
        string url = $"http://localhost/BVN/getTasks.php?valueID={valueID}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load tasks: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            tasks = JsonUtility.FromJson<TaskList>(jsonResponse).tasks;

            Debug.Log($"Fetched {tasks.Count} tasks for ValueID: {valueID}");

            if (valueIndex >= taskButtonGroups.Count || taskButtonGroups[valueIndex].taskButtons == null)
            {
                Debug.LogError($"Task buttons for value index {valueIndex} are not initialized.");
                yield break;
            }

            for (int i = 0; i < 5; i++)
            {
                if (i < tasks.Count)
                {
                    RTLTextMeshPro buttonText = taskButtonGroups[valueIndex].taskButtons[i].GetComponentInChildren<RTLTextMeshPro>();
                    if (buttonText != null)
                    {
                        buttonText.text = tasks[i].TaskName;
                        int taskIndex = i;
                        taskButtonGroups[valueIndex].taskButtons[i].onClick.AddListener(() => OnTaskButtonClicked(taskIndex));
                    }
                }
                else if (i < taskButtonGroups[valueIndex].taskButtons.Count)
                {
                    taskButtonGroups[valueIndex].taskButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void CloseTaskWindow(int index)
    {
        buttonCanvas.SetActive(true);

        for (int i = 0; i < valueCanvases.Length; i++)
        {
            valueCanvases[i].SetActive(false);
        }

        Debug.Log($"Task window for ValueID {values[index].ValueID} closed.");
    }

    

    [System.Serializable]
    public class TaskButtonGroup
    {
        public List<Button> taskButtons = new List<Button>(); // List of task buttons for each value canvas
    }

    [System.Serializable]
    public class TaskData
    {
        public int TaskID;
        public string TaskName;
        public string ValueName;
    }

    [System.Serializable]
    public class TaskList
    {
        public List<TaskData> tasks;
    }

    [System.Serializable]
    public class ValueData
    {
        public int ValueID;
        public string ValueName;
    }

    [System.Serializable]
    public class ValueList
    {
        public List<ValueData> values;
    }
}

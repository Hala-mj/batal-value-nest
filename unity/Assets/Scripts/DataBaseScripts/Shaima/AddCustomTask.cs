using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using RTLTMPro;
using UI.Dates;
using System;


public class AddCustomTask : MonoBehaviour
{
    public RTLTextMeshPro taskNameInput; // RTLTextMeshPro for TaskName
    public RTLTextMeshPro valueNameInput; // RTLTextMeshPro for ValueName
    public Button checkButton;           // Button to trigger submission
    public DatePicker datePicker;       // DatePicker for selecting deadline

    private int taskID; // Store the TaskID after creating the task
    private string selectedDeadline;

    private void Start()
    {
        // Add a listener to the button
        checkButton.onClick.AddListener(SubmitTaskWithValue);
    }

    public void SubmitTaskWithValue()
    {
        string taskName = taskNameInput.text;    // Get TaskName
        string valueName = valueNameInput.text;  // Get ValueName
        StartCoroutine(SendTaskAndValueToServer(taskName, valueName));
    }

    private IEnumerator SendTaskAndValueToServer(string taskName, string valueName)
    {
        // Send task and value to the server to create a new task
        WWWForm form = new WWWForm();
        form.AddField("taskName", taskName);
        form.AddField("valueName", valueName);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/BVN/add_task_with_value.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Task added successfully: " + www.downloadHandler.text);

                // Parse the taskID returned by the server (assumes the server returns the taskID)
                taskID = int.Parse(www.downloadHandler.text);

                // Now send the schedule details to saveScheduled.php
                DateTime selectedDate = datePicker.SelectedDate;
                selectedDeadline = selectedDate.ToString("yyyy-MM-dd");


                int selectedDuration = GetSelectedDuration(); // Get the selected duration (Daily/Weekly/Monthly)

                // Send schedule details to server
                StartCoroutine(SendScheduleToServer(taskID, selectedDuration, selectedDeadline));
            }
            else
            {
                Debug.LogError("Error adding task: " + www.error);
            }
        }
    }

    private IEnumerator SendScheduleToServer(int taskID, int duration, string deadline)
    {
        // Send the taskID, duration, and deadline to saveScheduled.php to store the schedule
        WWWForm form = new WWWForm();
        form.AddField("taskID", taskID);
        form.AddField("duration", duration);
        form.AddField("deadline", deadline);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/BVN/saveScheduled.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Schedule saved successfully: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error saving schedule: " + www.error);
            }
        }
    }

    private int GetSelectedDuration()
    {
        // Logic to get selected duration based on button clicks (Daily, Weekly, Monthly)
        // Example return: 1 for daily, 7 for weekly, 30 for monthly
        // Update based on your specific implementation
        return 1; // Placeholder, replace with actual logic to get the selected duration
    }
}

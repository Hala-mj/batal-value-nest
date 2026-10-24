using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System;
using UI.Dates;

public class ScheduleManager : MonoBehaviour
{
    public Button[] taskButtons;         // Buttons for tasks
    public Button dailyButton;           // Button for daily
    public Button weeklyButton;          // Button for weekly
    public Button monthlyButton;         // Button for monthly
    public Button saveButton;            // Button for saving
    public DatePicker datePicker;        // Reference to your DatePicker component

    private int selectedTaskID = -1;      // To store the selected TaskID
    private string selectedDuration = ""; // To store the selected Duration
    private string selectedDeadline = ""; // To store the selected Deadline
    private int selectedChildID = 0;      // ✅ To store the selected Child ID

    void Start()
    {
        // ✅ Get the selected ChildID from PlayerPrefs
        selectedChildID = PlayerPrefs.GetInt("SelectedChildIDForTask", 0);

        if (selectedChildID > 0)
        {
            Debug.Log("✅ Schedule Manager Loaded for Child ID: " + selectedChildID);
        }
        else
        {
            Debug.LogError("❌ No Child ID found in PlayerPrefs! Check if it was stored before scene switch.");
        }

        // ✅ Log stored value for debugging
        Debug.Log("📌 Retrieved SelectedChildIDForTask: " + selectedChildID);

        // ✅ Ensure task buttons are correctly assigned
        for (int i = 0; i < taskButtons.Length; i++)
        {
            int index = i + 1; // Assuming TaskID starts from 1 (adjust if needed)
            taskButtons[i].onClick.AddListener(() => OnTaskSelected(index));
            Debug.Log($"✅ Task button {index} assigned correctly.");
        }

        // ✅ Ensure duration buttons correctly call `OnDurationSelected`
        if (dailyButton != null)
            dailyButton.onClick.AddListener(() => OnDurationSelected("Daily"));
        else
            Debug.LogError("❌ Daily button is not assigned in the Inspector!");

        if (weeklyButton != null)
            weeklyButton.onClick.AddListener(() => OnDurationSelected("Weekly"));
        else
            Debug.LogError("❌ Weekly button is not assigned in the Inspector!");

        if (monthlyButton != null)
            monthlyButton.onClick.AddListener(() => OnDurationSelected("Monthly"));
        else
            Debug.LogError("❌ Monthly button is not assigned in the Inspector!");

        // ✅ Ensure save button is correctly assigned
        if (saveButton != null)
            saveButton.onClick.AddListener(SaveSchedule);
        else
            Debug.LogError("❌ Save button is not assigned in the Inspector!");
    }

    // ✅ Fix: Define `OnTaskSelected()` method
    void OnTaskSelected(int taskID)
    {
        selectedTaskID = taskID; // ✅ Store selected Task ID
        Debug.Log($"✅ Task selected: {selectedTaskID}"); // ✅ Log selection
    }

    // ✅ Fix: Define `OnDurationSelected()` method
    void OnDurationSelected(string duration)
    {
        selectedDuration = duration; // ✅ Store selected duration
        Debug.Log($"✅ Selected Duration: {selectedDuration}"); // ✅ Log selection
    }

    void SaveSchedule()
    {
        if (selectedChildID == 0)
        {
            Debug.LogError("❌ No Child ID selected! Cannot assign task.");
            return;
        }

        Debug.Log($"📡 Saving schedule for Child ID: {selectedChildID}");

        StartCoroutine(SendScheduleToDatabase());
    }

    IEnumerator SendScheduleToDatabase()
    {
        string url = "http://localhost/BVN/saveScheduled.php";  // ✅ Correct URL

        WWWForm form = new WWWForm();
        form.AddField("childID", selectedChildID); // ✅ Send ChildID to the database
        form.AddField("taskID", selectedTaskID);
        form.AddField("duration", selectedDuration);
        form.AddField("deadline", selectedDeadline);

        Debug.Log($"📡 Sending Schedule Data to: {url}");
        Debug.Log($"✅ ChildID: {selectedChildID}, TaskID: {selectedTaskID}, Duration: {selectedDuration}, Deadline: {selectedDeadline}");

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Failed to save schedule: " + request.error);
        }
        else
        {
            Debug.Log("✅ Schedule successfully saved: " + request.downloadHandler.text);
        }
    }
}

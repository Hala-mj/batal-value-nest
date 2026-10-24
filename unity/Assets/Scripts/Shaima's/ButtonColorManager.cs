using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Make sure this is included for TMP_InputField
using UI.Dates;
using UnityEngine.Networking;
using System.Collections;

using System;

public class ButtonColorManager : MonoBehaviour
{
    [Header("Text Inputs")]
    public TMP_InputField taskNameInput; // TMP_InputField for TaskName
    public TMP_InputField valueNameInput; // TMP_InputField for ValueName

    [Header("Buttons")]
    public Button checkButton;           // Button to trigger submission
    public Button greyDailyButton;       // Grey Daily Button
    public Button greyWeeklyButton;      // Grey Weekly Button
    public Button greyMonthlyButton;     // Grey Monthly Button
    public GameObject blueDailyButton;   // Blue Daily Button
    public GameObject blueWeeklyButton;  // Blue Weekly Button
    public GameObject blueMonthlyButton; // Blue Monthly Button

    [Header("Date Picker")]
    public DatePicker datePicker;        // DatePicker for selecting deadline

    private int taskID;                  // Store the TaskID after creating the task
    private string selectedDeadline;
    private int selectedDuration = 1;    // Default to Daily

    private void Start()
    {
        // Add a listener to the check button
        checkButton.onClick.AddListener(SubmitTaskWithValue);

        // Add listeners for duration buttons
        greyDailyButton.onClick.AddListener(() => OnButtonClicked(greyDailyButton.gameObject, 1));
        greyWeeklyButton.onClick.AddListener(() => OnButtonClicked(greyWeeklyButton.gameObject, 7));
        greyMonthlyButton.onClick.AddListener(() => OnButtonClicked(greyMonthlyButton.gameObject, 30));

        // Set initial button state
        ResetAllButtons();
    }

    // Handle button state for Daily, Weekly, Monthly
    private void OnButtonClicked(GameObject clickedButton, int duration)
    {
        selectedDuration = duration;

        // Toggle button states based on selection
        if (clickedButton == greyWeeklyButton.gameObject)
        {
            blueDailyButton.SetActive(false);
            blueWeeklyButton.SetActive(true);
            blueMonthlyButton.SetActive(false);

            greyDailyButton.gameObject.SetActive(true);
            greyWeeklyButton.gameObject.SetActive(false);
            greyMonthlyButton.gameObject.SetActive(true);
        }
        else if (clickedButton == greyDailyButton.gameObject)
        {
            blueDailyButton.SetActive(true);
            blueWeeklyButton.SetActive(false);
            blueMonthlyButton.SetActive(false);

            greyDailyButton.gameObject.SetActive(false);
            greyWeeklyButton.gameObject.SetActive(true);
            greyMonthlyButton.gameObject.SetActive(true);
        }
        else if (clickedButton == greyMonthlyButton.gameObject)
        {
            blueDailyButton.SetActive(false);
            blueWeeklyButton.SetActive(false);
            blueMonthlyButton.SetActive(true);

            greyDailyButton.gameObject.SetActive(true);
            greyWeeklyButton.gameObject.SetActive(true);
            greyMonthlyButton.gameObject.SetActive(false);
        }
    }

    // Reset all buttons to the default state
    void ResetAllButtons()
    {
        blueDailyButton.SetActive(true);
        blueWeeklyButton.SetActive(false);
        blueMonthlyButton.SetActive(false);

        greyDailyButton.gameObject.SetActive(false);
        greyWeeklyButton.gameObject.SetActive(true);
        greyMonthlyButton.gameObject.SetActive(true);

        selectedDuration = 1; // Default to Daily
    }

    public void SubmitTaskWithValue()
    {
        // Get the task and value names from the input fields
        string taskName = taskNameInput.text;    // Get TaskName
        string valueName = valueNameInput.text;  // Get ValueName

        if (string.IsNullOrEmpty(taskName) || string.IsNullOrEmpty(valueName))
        {
            Debug.LogError("Task Name or Value Name is empty!");
            return;
        }

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

                // The server should return the inserted ValueID (or success message)
                string serverResponse = www.downloadHandler.text;

                // Handle server response, such as parsing the ValueID or error message
                if (int.TryParse(serverResponse, out taskID))
                {
                    // Now send the schedule details to saveScheduled.php
                    DateTime selectedDate = datePicker.SelectedDate;
                    selectedDeadline = selectedDate.ToString("yyyy-MM-dd");

                    // Send schedule details to the server
                    StartCoroutine(SendScheduleToServer(taskID, selectedDuration, selectedDeadline));
                }
                else
                {
                    Debug.LogError("Failed to parse TaskID: " + serverResponse);
                }
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

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/BVN/saveCScheduled.php", form))
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
}

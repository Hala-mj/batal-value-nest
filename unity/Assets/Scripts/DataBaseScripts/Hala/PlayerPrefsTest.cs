using UnityEngine;

public class PlayerPrefsTest : MonoBehaviour
{
    void Start()
    {
        // Set a test value in PlayerPrefs
        PlayerPrefs.SetInt("TestID", 123);
        PlayerPrefs.Save(); // Ensure the value is saved immediately

        // Retrieve the test value from PlayerPrefs
        int testID = PlayerPrefs.GetInt("TestID", -1); // Default to -1 if not found

        // Log the retrieved value to the console
        Debug.Log("TestID retrieved: " + testID);
    }
}

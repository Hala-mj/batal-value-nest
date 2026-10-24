using UnityEngine;
using UnityEngine.SceneManagement;

public class ChildSelectionHandler : MonoBehaviour
{
    private int childID;
    private string childName;
    private string childDashboardScene = "Kid Dashboard"; // Ensure this matches your scene name

    // Sets the child data (called when a child is selected)
    public void SetChildData(int id, string name)
    {
        childID = id;
        childName = name;
        Debug.Log($"✅ SetChildData Called → ID: {childID}, Name: {childName}");
    }

    // Called when the user confirms the selection
    public void OnChildClicked()
    {
        Debug.Log($"👶 OnChildClicked Called → ID: {childID}, Name: {childName}");

        if (childID > 0)
        {
            // Save to PlayerPrefs
            PlayerPrefs.SetInt("ChildID", childID);
            PlayerPrefs.SetString("SelectedChildName", childName);
            PlayerPrefs.Save();

            Debug.Log($"✅ PlayerPrefs Saved → ChildID={PlayerPrefs.GetInt("ChildID")}, Name={PlayerPrefs.GetString("SelectedChildName")}");
        }
        else
        {
            Debug.LogError("❌ Invalid Child ID. Aborting.");
            return;
        }

        // Ensure the scene exists in Build Settings
        if (SceneUtility.GetBuildIndexByScenePath(childDashboardScene) == -1)
        {
            Debug.LogError($"❌ Scene '{childDashboardScene}' is not in Build Settings!");
            return;
        }

        StartCoroutine(LoadSceneWithDelay(childDashboardScene));
    }

    // Coroutine to delay scene transition for PlayerPrefs to save
    private System.Collections.IEnumerator LoadSceneWithDelay(string sceneName)
    {
        Debug.Log("⏳ Waiting for PlayerPrefs to Save...");
        yield return new WaitForSeconds(0.2f); // Delay ensures PlayerPrefs.Save completes
        Debug.Log($"🚀 Loading Scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
}

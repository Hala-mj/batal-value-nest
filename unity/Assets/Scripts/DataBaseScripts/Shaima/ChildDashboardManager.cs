using UnityEngine;
using TMPro; // Ensure you have TextMeshPro installed and the reference added

public class ChildDashboardManager : MonoBehaviour
{
    public TMP_Text childNameText; // Reference to the UI text element

    void Start()
    {
        Debug.Log("🏁 ChildDashboardManager Start Called");

        // Retrieve data from PlayerPrefs
        int childID = PlayerPrefs.GetInt("ChildID", 0);
        string childName = PlayerPrefs.GetString("SelectedChildName", "");

        Debug.Log($"🔍 PlayerPrefs Loaded → ID: {childID}, Name: '{childName}'");

        // Display the retrieved data or fallback text
        if (childID > 0)
        {
            Debug.Log($"✅ Kid Dashboard Loaded for: {childName} (ID: {childID})");
            childNameText.text = childName + " البطل"; // Display child's name with "البطل"
        }
        else
        {
            Debug.LogError("❌ No Child ID found in PlayerPrefs! Check scene switch.");
            childNameText.text = "البطل"; // Fallback text
        }
    }
}

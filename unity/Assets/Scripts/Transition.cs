using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Transition : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // Name of the next scene to load

    public void Start()
    {
        // Ensure the Button component is attached
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
        else
        {
            Debug.LogError("No Button component found on this GameObject.");
        }
    }

    private void OnButtonClick()
    {
        // Load the specified next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name is not set in the inspector.");
        }
    }
}

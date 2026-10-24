using UnityEngine;
using UnityEngine.UI;

public class AddImageDisplay : MonoBehaviour
{
    public Image MessageImage;  // The image to display when the "Add" button is clicked
    [SerializeField] int popUpTime;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the image is disabled when the game starts
        MessageImage.gameObject.SetActive(false);
    }

    // This will be called when the "Add" button is clicked
    public void ShowAddImage()
    {
        // Enable the image when the "Add" button is clicked
        MessageImage.gameObject.SetActive(true);

        // Optionally, hide the image after a certain time (e.g., 3 seconds)
        Invoke("HideAddImage", popUpTime);  // Hide the image after 3 seconds
    }

    // This hides the "Add" image
    public void HideAddImage()
    {
        MessageImage.gameObject.SetActive(false);  // Disable the "Add" image
    }
}

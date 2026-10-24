using UnityEngine;
using UnityEngine.UI;

public class ColorChangeOnClick : MonoBehaviour
{
    private Image boxImage;

    void Start()
    {
        // Get the Image component attached to this GameObject
        boxImage = GetComponent<Image>();
    }

    public void OnBoxClick()
    {
        if (boxImage != null)
        {
            // Change the color to green
            boxImage.color = Color.green;
        }
    }
}


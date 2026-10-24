using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HList : MonoBehaviour
{
    public GameObject imagePrefab;   // Prefab containing an Image component
    public Transform content;        // Parent Content in Scroll View
    public Sprite[] imageSprites;    // Array of Sprites to display

    void Start()
    {
        // Ensure all fields are assigned
        if (imagePrefab == null || content == null || imageSprites == null)
        {
            Debug.LogError("Please assign all necessary fields in the Inspector.");
            return;
        }

        // Populate the list with images
        PopulateList();
    }

    void PopulateList()
    {
        // Remove any previous content in the content area
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Create new image items
        foreach (Sprite sprite in imageSprites)
        {
            if (sprite == null) continue;

            // Instantiate the image prefab
            GameObject newItem = Instantiate(imagePrefab, content);

            // Set the sprite in the Image component
            Image imgComponent = newItem.GetComponentInChildren<Image>();
            if (imgComponent != null)
            {
                imgComponent.sprite = sprite;
            }

            // Adjust the prefab position for horizontal alignment
            RectTransform rectTransform = newItem.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(0, 0.5f);
                rectTransform.anchorMax = new Vector2(0, 0.5f);
                rectTransform.pivot = new Vector2(0, 0.5f);
            }
        }

        // Adjust the content size to fit all items horizontally
        AdjustContentSize();
    }

    void AdjustContentSize()
    {
        RectTransform contentRect = content.GetComponent<RectTransform>();
        if (contentRect == null) return;

        int itemCount = imageSprites.Length;
        float itemWidth = imagePrefab.GetComponent<RectTransform>().sizeDelta.x;
        float spacing = content.GetComponent<HorizontalLayoutGroup>()?.spacing ?? 0;

        // Set the content width to fit all items and spacing
        contentRect.sizeDelta = new Vector2((itemWidth + spacing) * itemCount, contentRect.sizeDelta.y);
    }
}

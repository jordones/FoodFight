using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItems : MonoBehaviour
{

    private int numItems = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    public void AddItemToDisplay(Item item)
    {
        // Get the sprite
        Sprite sprite = item.transform.parent.GetComponent<SpriteRenderer>().sprite;
        GameObject newItem = new GameObject();
        // Make it a child of the canvas
        newItem.transform.parent = this.transform;
        // Add the ui image component to store the sprite
        Image itemImage = newItem.AddComponent<Image>();
        // Add the sprite to the image box
        itemImage.sprite = sprite;
        // 15 is the approximate size the item sprite needs and moves it down that much foreach item collected
        Vector2 itemPosition = new Vector2(0, 40 - (numItems * 7));
        // Move the image to an open space        
        RectTransform rt = newItem.GetComponent<RectTransform>() as RectTransform;
        rt.localPosition = itemPosition;
		// These numbers for relative to the items RectTransform in the ui prefab
        rt.localScale = new Vector2(6, 0.07f);
        numItems++;
    }
}

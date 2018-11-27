using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCompleted : MonoBehaviour
{
    // Use this for initialization
    public List<Image> unlockedItemWrapper;

    public void UpdateUnlockedItems(List<Item> items)
    {
        for (int i = 0; i < unlockedItemWrapper.Count; i++)
        {
            unlockedItemWrapper[i].sprite = items[i].transform.parent.GetComponent<SpriteRenderer>().sprite;
        } 
    }

}

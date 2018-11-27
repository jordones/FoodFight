using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunCompleted : MonoBehaviour
{
    // Use this for initialization
    public List<Image> unlockedItemWrapper;
    public static RunCompleted instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
            RunManager.instance.RunComplete();
        } else {
            Destroy(gameObject);
        }
    }

    public void UpdateUnlockedItems(List<Sprite> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            unlockedItemWrapper[i].sprite = items[i];
        } 
    }

}

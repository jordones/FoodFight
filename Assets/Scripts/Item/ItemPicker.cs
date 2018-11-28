using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{

    List<Item> items = new List<Item>();
    // Use this for initialization
    void Awake()
    {
    }

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems() {
        int i = 0;
        yield return new WaitUntil(() => ItemPool.instance != null && ItemPool.instance.ready);

        List<GameObject> prefabs = ItemPool.instance.GetRandomUnlockedItems(transform.childCount);
        foreach (Transform itemStand in transform)
        {
            Debug.Log(itemStand);
            Spawn(itemStand, prefabs[i]);
            i++;
        }
    }

    void Update()
    {
        // print(items.Count);
        foreach (Item item in items)
        {
            if (item.pickedUp)
            {
                destoryAllItems();
            }
        }
    }

    private void destoryAllItems()
    {
        foreach (Item item in items)
        {
            if (!item.pickedUp)
            {
                if (item != null) {
                    Destroy(item.transform.parent.gameObject);
                }
            }
        }
        Destroy(gameObject);
    }

    private void Spawn(Transform itemStand, GameObject itemFab)
    {
         //Wait until ItemPool is ready

        Vector3 newPosition = itemStand.position;
        newPosition.y += 0.7f;
        GameObject item = Instantiate(itemFab, newPosition, itemStand.rotation);
        items.Add(item.GetComponentsInChildren<Item>()[0]);
    }
}

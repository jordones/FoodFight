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
        foreach (Transform itemStand in transform)
        {
            StartCoroutine(Spawn(itemStand));
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
                Destroy(item.transform.parent.gameObject);
            }
        }
        Destroy(gameObject);
    }

    private IEnumerator Spawn(Transform itemStand)
    {
        yield return new WaitUntil(() => ItemPool.instance != null && ItemPool.instance.ready); //Wait until ItemPool is ready
        GameObject itemFab = ItemPool.instance.GetRandomItem();
        Vector3 newPosition = itemStand.position;
        newPosition.y += 0.7f;
        GameObject item = Instantiate(itemFab, newPosition, itemStand.rotation);
        items.Add(item.GetComponentsInChildren<Item>()[0]);
    }
}

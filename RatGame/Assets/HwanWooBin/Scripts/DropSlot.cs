using System;
using UnityEngine;

public class DropSlot : MonoBehaviour
{
    public ItemClass Item;

    public Action Load;

    public bool Get;

    public bool Lock;

    public void Getitem()
    {
        if (Get)
        {
            Get = false;
            GameManager.Instance.AddItem(Item, true);
            Debug.Log("È¹µæ" + transform.name);
            Item = new ItemClass();
            Load();
        }
    }

    public void BackItem()
    {
        GameManager.Instance.AddItem(Item, false);
        Item = new ItemClass();
        Load();
    }

    public void DeleteItem()
    {
        Item = new ItemClass();
    }
}

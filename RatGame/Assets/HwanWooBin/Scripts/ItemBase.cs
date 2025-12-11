using System;
using UnityEngine;


[Serializable]
public enum ItemType
{
    Herb,
    Potion
}

[Serializable]
public abstract class ItemBase : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;

    public ItemType itemType;
    [TextArea]
    public string Explanation;

    public float Price;

    public virtual void SetValue(ItemBase item)
    {
        itemName = item.itemName;
        itemType = item.itemType;
        Explanation = item.Explanation;
        Price = item.Price;
    }
}







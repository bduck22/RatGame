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

    public virtual ItemBase InitData(string[] datas)
    {
        this.itemName = datas[0];
        this.itemType = Enum.Parse<ItemType>(datas[1]);
        this.Explanation = datas[2];
        this.Price = int.Parse(datas[3]);
        return this;
    }
}







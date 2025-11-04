using System;
using UnityEngine;


[Serializable]
public enum ItemType
{
    Herb,
    Potion
}

[Serializable]
public class ItemBase : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;

    public ItemType itemType;
    public string Explanation;
}







using System;
using UnityEngine;


[Serializable]
public enum ItemType
{
    Herb,
    Potion
}

public class ItemBase : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public Sprite TopImage;

    public ItemType itemType;
    public string Explanation;
}


[CreateAssetMenu(menuName = "Herb")]
public class HerbData : ItemBase
{
    public bool IsProcessed;
    public int[] itemProcessedWay;
}


[CreateAssetMenu(menuName = "Potion")]
public class PotionData : ItemBase
{
    public HerbData Herb1;
    public HerbData Herb2;

    public int HerbAmount1;
    public int HerbAmount2;


    public int itemLevel=0; // 0 ±âº»°ª, 1 ÀÏ¹Ý, 2 Èñ±Í, 3 Àü¼³
    public float Completeness;
    public bool PotionCase;
}


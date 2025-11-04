using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class DicManager : MonoBehaviour
{
    public List<float> OpenedPer;

    ItemDatas itemDatas;
    void Start()
    {
        itemDatas = GameManager.Instance.itemDatas;

        foreach (var item in itemDatas.items)
        {
            if (item == null)
            {
                continue;
            }
            if (item.itemType == ItemType.Potion)
            {
                var Item = item as PotionData;
                if(Item.itemLevel > 0)
                {
                    OpenedPer.Add(0f);
                }
            }
        }
    }
    void Update()
    {
        
    }
}

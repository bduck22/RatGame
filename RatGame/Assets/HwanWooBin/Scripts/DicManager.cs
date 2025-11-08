using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class DicManager : MonoBehaviour
{
    public List<float> OpenedPer;

    ItemDatas itemDatas;

    public Transform DictionaryPos;
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
    public void LoadDictionary()
    {
        int j = 0;
        for(int i = 0; i < DictionaryPos.childCount; i++)
        {
            if(DictionaryPos.GetChild(i).GetComponent<TextMeshProUGUI>()||!DictionaryPos.GetChild(i).gameObject.activeSelf)
            {
                continue;
            }
            else
            {
                for(int l = 0; l < DictionaryPos.GetChild(i).childCount; l++)
                {
                    Image Slot = DictionaryPos.GetChild(i).GetChild(l).GetComponent<Image>();
                    Slot.transform.GetChild(0).GetComponent<Image>().sprite = itemDatas.items[13+j].itemImage;
                    Slot.GetComponentInChildren<TextMeshProUGUI>().text = OpenedPer[j].ToString() + "%";
                    j++;
                }
            }
        }
    }
}

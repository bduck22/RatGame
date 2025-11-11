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

    public RectTransform DictionaryInfo;

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

    public void ViewInfo(int number)
    {
#if UNITY_EDITOR
        Vector3 itemViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 mouseP = Input.mousePosition;
#endif
        if (Input.touchCount > 0)
        {
            itemViewportPos = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
            mouseP = Input.GetTouch(0).position;
        }
        
        bool isLeftHalf = itemViewportPos.x < 0.5f;

        if (isLeftHalf)
        {
            DictionaryInfo.pivot = new Vector2(0f, 0.8f);
            DictionaryInfo.position = mouseP;
        }
        else
        {
            DictionaryInfo.pivot = new Vector2(1f, 0.8f);
            DictionaryInfo.position = mouseP;
        }

        
        PotionData data = itemDatas.items[number+13] as PotionData;

        if (OpenedPer[number] >= 30)
        {
            DictionaryInfo.GetChild(1).gameObject.SetActive(true);
            DictionaryInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.HerbAmount1.ToString();
            DictionaryInfo.GetChild(1).GetComponentInChildren<Image>().sprite = data.Herb1.itemImage;
            if (data.process1 != -1 && data.process1 != 3)
            {
                DictionaryInfo.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
                DictionaryInfo.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.inventoryManager.ProcessIcon[data.process1];
            }
            else
            {
                DictionaryInfo.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
            } 
        }
        else
        {
            DictionaryInfo.GetChild(1).gameObject.SetActive(false);
            DictionaryInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = "30% 필요";
        }

        if (OpenedPer[number] >= 60)
        {
            DictionaryInfo.GetChild(2).gameObject.SetActive(true);
            DictionaryInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.HerbAmount2.ToString();
            DictionaryInfo.GetChild(2).GetComponentInChildren<Image>().sprite = data.Herb2.itemImage;
            if (data.process1 != -1 && data.process1 != 3)
            {
                DictionaryInfo.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(true);
                DictionaryInfo.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.inventoryManager.ProcessIcon[data.process2];
            }
            else
            {
                DictionaryInfo.GetChild(2).GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            DictionaryInfo.GetChild(2).gameObject.SetActive(false);
            DictionaryInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = "60% 필요";
        }

        if (OpenedPer[number] >= 100)
        {
            DictionaryInfo.GetChild(3).GetComponent<TextMeshProUGUI>().text = (data.NonWater ? "고체" : "액체");
        }
        else
        {
            DictionaryInfo.GetChild(3).GetComponent<TextMeshProUGUI>().text = "100% 필요";
        }


            DictionaryInfo.gameObject.SetActive(true);
    }
}

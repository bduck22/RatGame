using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class DicManager : MonoBehaviour
{
    [Header("옥규의 약물")]
    public List<int> PotionData;
    public List<float> OpenedPer;

    [Header("우빈의 약초")]
    public List<int> HerbData;
    public List<int> ProcessedCountHer;


    ItemDatas itemDatas;

    
    public Transform DictionaryPos;

    public RectTransform DictionaryInfo;

    void Start()
    {
        itemDatas = GameManager.Instance.itemDatas;

        for(int i=0; i < itemDatas.items.Length; i++)
        {
            ItemBase item = itemDatas.items[i];
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
                    PotionData.Add(i);
                }
            }
            else
            {
                HerbData.Add(i);
                ProcessedCountHer.Add(0);
            }
        }
    }
    public void LoadDictionary()
    {
        int j = 0;
        DictionaryInfo.gameObject.SetActive(false);
        for (int i = 0; i < DictionaryPos.childCount; i++)
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

    public void SettingHerbProcessedCount(ItemClass numder)
    {
        for(int i=0; i< HerbData.Count; i++)
        {
            if (HerbData[i] == numder.itemNumber)
            {
                ProcessedCountHer[i] += 1; // 가공횟수 증가
                break;
            }
        }
    }


    public void ViewInfo(int number)
    {
        Vector3 itemViewportPos= Vector3.zero;
            Vector3 mouseP = Vector3.zero;
#if UNITY_EDITOR
        itemViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mouseP = Input.mousePosition;
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
        HerbData herddata = itemDatas.items[number + 13] as HerbData;

        if (data != null)
        {
            // 약물 이름
            if (OpenedPer[number] >= 10)
            {
                DictionaryInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.itemName;
            }
            else
            {
                DictionaryInfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = "10% 필요";
            }

            // 약물 설명
            if (OpenedPer[number] >= 30)
            {
                DictionaryInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.Explanation;
            }
            else
            {
                DictionaryInfo.GetChild(2).GetComponent<TextMeshProUGUI>().text = "30% 필요";
            }

            // 약물 가격
            if (OpenedPer[number] >= 50)
            {
                DictionaryInfo.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.Price.ToString();
            }
            else
            {
                DictionaryInfo.GetChild(3).GetComponent<TextMeshProUGUI>().text = "50% 필요";
            }

            // 재료1
            if (OpenedPer[number] >= 70)
            {
                DictionaryInfo.GetChild(4).GetChild(0).gameObject.SetActive(true);
                DictionaryInfo.GetChild(4).GetComponent<TextMeshProUGUI>().text = data.HerbAmount1.ToString();
                DictionaryInfo.GetChild(4).GetComponentInChildren<Image>().sprite = data.Herb1.itemImage;
                if (data.process1 != -1 && data.process1 != 3)
                {
                    DictionaryInfo.GetChild(4).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    DictionaryInfo.GetChild(4).GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.inventoryManager.ProcessIcon[data.process1];
                }
                else
                {
                    DictionaryInfo.GetChild(4).GetChild(0).GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                DictionaryInfo.GetChild(4).GetChild(0).gameObject.SetActive(false);
                DictionaryInfo.GetChild(4).GetComponent<TextMeshProUGUI>().text = "70% 필요";
            }

            // 재료2
            if (OpenedPer[number] >= 90)
            {

                DictionaryInfo.GetChild(5).GetChild(0).gameObject.SetActive(true);
                DictionaryInfo.GetChild(5).GetComponent<TextMeshProUGUI>().text = data.HerbAmount2.ToString();
                DictionaryInfo.GetChild(5).GetComponentInChildren<Image>().sprite = data.Herb2.itemImage;
                if (data.process1 != -1 && data.process1 != 3)
                {
                    DictionaryInfo.GetChild(5).GetChild(0).GetChild(0).gameObject.SetActive(true);
                    DictionaryInfo.GetChild(5).GetChild(0).GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.inventoryManager.ProcessIcon[data.process2];
                }
                else
                {
                    DictionaryInfo.GetChild(5).GetChild(0).GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                DictionaryInfo.GetChild(5).GetChild(0).gameObject.SetActive(false);
                DictionaryInfo.GetChild(5).GetComponent<TextMeshProUGUI>().text = "90% 필요";
            }

            // 최적 조합법
            if (OpenedPer[number] >= 100)
            {
                DictionaryInfo.GetChild(6).GetComponent<TextMeshProUGUI>().text = (data.NonWater ? "고체" : "액체");
            }
            else
            {
                DictionaryInfo.GetChild(6).GetComponent<TextMeshProUGUI>().text = "100% 필요";
            }
        }
        else
        {
            string way = "";
            string falseway = "닳이기 / 빻기 / 건조하기";
            for (int i = 0; i < herddata.itemProcessedWay.Length; i++)
            {
                switch (herddata.itemProcessedWay[i])
                {
                    case 0:
                        way += "닳이기";
                        falseway.Replace("닳이기 /", "");
                        break;
                    case 1:
                        way += "빻기";
                        falseway.Replace("/ 빻기 /", "/");
                        break;
                    case 2:
                        way += "건조하기";
                        falseway.Replace("/ 건조하기", "");
                        break;
                    default:
                        way += "뭐노";
                        break;
                }
                way += " / ";
            }
           

            // 약초 틀린 가공법
            if (ProcessedCountHer[number] >= 6)
            {
                DictionaryInfo.GetChild(9).GetComponent<TextMeshProUGUI>().text = falseway;
            }
            else
            {
                DictionaryInfo.GetChild(9).GetComponent<TextMeshProUGUI>().text = "가공 6회 필요";
            }

            // 약초 옳은 가공법
            if (ProcessedCountHer[number] >= 10)
            {
                DictionaryInfo.GetChild(10).GetComponent<TextMeshProUGUI>().text = way;
            }
            else
            {
                DictionaryInfo.GetChild(10).GetComponent<TextMeshProUGUI>().text = "가공 10회 필요";
            }
        }

            DictionaryInfo.gameObject.SetActive(true);
    }
}

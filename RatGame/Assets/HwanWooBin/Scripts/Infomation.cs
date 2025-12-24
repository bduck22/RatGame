using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Infomation : MonoBehaviour
{
    public Sprite Yes;
    public Sprite No;
    public Sprite WhatThe;
    public Sprite[] ProcessWay;

    [Header("약초")]
    public Transform HerbPanel;
    public TextMeshProUGUI Herb_Name;
    public TextMeshProUGUI Herb_Description;
    public Transform Herb_ItemInfo;
    public Transform Herb_DicInfo;
    [Header("아이템")]
    public Image Herb_Item_Itemmain;
    public TextMeshProUGUI Herb_Item_Type;
    public Transform Herb_Item_ProcessIcon;
    public TextMeshProUGUI Herb_Item_Count;
    [Header("도감")]
    public Image Herb_Dic_Itemmain;
    public TextMeshProUGUI Herb_Dic_Price;
    public Transform Herb_Dic_Processes;
    public TextMeshProUGUI Herb_Dic_ProcessCount;

    [Header("약품")]
    public Transform PotionPanel;
    public TextMeshProUGUI Potion_Tier;
    public TextMeshProUGUI Potion_Complete;
    public TextMeshProUGUI Potion_Name;
    public TextMeshProUGUI Potion_Explanation;
    public Transform Potion_ItemInfo;
    public Transform Potion_DicInfo;
    [Header("아이템")]
    public TextMeshProUGUI Potion_Item_Liquid;
    public Image Potion_Item_Icon;
    public TextMeshProUGUI Potion_Item_Herb1Name;
    public TextMeshProUGUI Potion_Item_Herb2Name;
    public Image Potion_Item_Herb1Icon;
    public Image Potion_Item_Herb2Icon;
    public Image Potion_Item_Herb1Process;
    public Image Potion_Item_Herb2Process;
    public TextMeshProUGUI Potion_Item_Herb1Amount;
    public TextMeshProUGUI Potion_Item_Herb2Amount;
    [Header("도감")]
    public Image Potion_Dic_LiquidIcon;
    public Image Potion_Dic_SolidIcon;
    public TextMeshProUGUI Potion_Dic_Price;
    public Image Potion_Dic_Herb1Icon;
    public Image Potion_Dic_Herb2Icon;
    public Image Potion_Dic_Herb1Process;
    public Image Potion_Dic_Herb2Process;
    public TextMeshProUGUI Potion_Dic_Herb1Amount;
    public Transform Potion_Dic_Liquid;
    
    public void ShowInfo(bool isdic, ItemClass item)
    {
        ItemBase iteminfo = ItemDatas.instance.items[item.itemNumber];
        if (item.itemType == ItemType.Herb)
        {
            HerbPanel.gameObject.SetActive(true);
            PotionPanel.gameObject.SetActive(false);
            HerbData herb = iteminfo as HerbData;
            Herb_Name.text = item.itemName;
            LayoutRebuilder.ForceRebuildLayoutImmediate(Herb_Name.transform.parent.parent.GetComponent<RectTransform>());
            Herb_Description.text = item.itemDescription;
            if (isdic)
            {
                Herb_DicInfo.gameObject.SetActive(true);
                Herb_ItemInfo.gameObject.SetActive(false);

                Herb_Dic_Itemmain.sprite = herb.itemImage;
                Herb_Dic_Price.text = herb.Price.ToString("#,##0");

                int ProcessCount = GameManager.Instance.dicManager.ProcessedCountHer[item.itemNumber];

                Herb_Dic_ProcessCount.text = "가공 횟수 : "+ProcessCount.ToString("#,##0");

                for(int i = 0; i < 3; i++)
                {
                    if(herb.itemProcessedWay[i] == i&& ProcessCount >= 6)
                    {
                            Herb_Dic_Processes.GetChild(i).GetChild(1).GetComponent<Image>().sprite = Yes;
                    }
                    else if (ProcessCount >= 10)
                    {
                            Herb_Dic_Processes.GetChild(i).GetChild(1).GetComponent<Image>().sprite = No;
                    }
                    else
                    {
                        Herb_Dic_Processes.GetChild(i).GetChild(i).GetComponent<Image>().sprite = WhatThe;
                    }
                }
            }
            else
            {
                Herb_ItemInfo.gameObject.SetActive(true);
                Herb_DicInfo.gameObject.SetActive(false);

                Herb_Item_Itemmain.sprite = herb.itemImage;
                if (item.ProcessWay == -1)
                {
                    Herb_Item_ProcessIcon.GetChild(3).gameObject.SetActive(true);
                }
                else
                {
                    Herb_Item_ProcessIcon.GetChild(item.ProcessWay).gameObject.SetActive(true);
                }

                    Herb_Item_Type.text = "가공 상태 : " + (item.ProcessWay == 0 ? "<color=green>달이기</color>" : item.ProcessWay == 1 ? "<color=blue>빻기</color>" : item.ProcessWay == 2 ? "<color=red>말리기</color>" : "X");
                Herb_Item_Count.text = item.ItemCount.ToString("#,##0")+"개";
            }
        }
        else
        {
            HerbPanel.gameObject.SetActive(false);
            PotionPanel.gameObject.SetActive(true);
            PotionData potion = iteminfo as PotionData;
            switch (potion.itemLevel)
            {
                case -1:
                    Potion_Tier.text = "<color=black>X</color>";
                    break;
                case 0:
                    Potion_Tier.text = "<color=red>일반</color>";
                    break;
                case 1:
                    Potion_Tier.text = "<color=blue>희귀</color>";
                    break;
                case 2:
                    Potion_Tier.text = "<color=red><전설</color>";
                    break;
                case 3:
                    Potion_Tier.text = "<color=black>미상</color>";
                    break;
            }

            float openper = GameManager.Instance.dicManager.OpenedPer[item.itemNumber - ItemDatas.instance.PotionIndex];

            Potion_Complete.text = "해금도 : " + openper.ToString() + "%";

            if (openper >= 10)
            {
                Potion_Name.text = potion.name;
            }
            else
            {
                Potion_Name.text = "<color=blue>10%(이름)</color>";
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(Potion_Name.transform.parent.parent.GetComponent<RectTransform>());

            if (openper >= 30)
            {
                Potion_Explanation.text = potion.Explanation;
            }
            else
            {
                Potion_Explanation.text = "30%(설명)";
            }

            if (isdic)
            {
                Potion_DicInfo.gameObject.SetActive(true);
                Potion_ItemInfo.gameObject.SetActive(false);

                Potion_Dic_SolidIcon.sprite = (potion.NonWater ? potion.itemImage : potion.NonShapeImage);
                Potion_Dic_LiquidIcon.sprite = (potion.NonWater ? potion.NonShapeImage : potion.itemImage);

                if(openper >= 50)
                {
                    Potion_Dic_Price.text = potion.Price.ToString("#,##0");
                }
                else
                {
                    Potion_Dic_Price.text = "50%(가격)";
                }

                if (openper >= 70)
                {
                    Potion_Dic_Herb1Icon.gameObject.SetActive (true);
                    Potion_Dic_Herb2Icon.gameObject.SetActive (true);
                    Potion_Dic_Herb1Icon.sprite = potion.Herb1.itemImage;
                    Potion_Dic_Herb2Icon.sprite = potion.Herb2.itemImage;
                }
                else
                {
                    Potion_Dic_Herb1Icon.gameObject.SetActive(false);
                    Potion_Dic_Herb2Icon.gameObject.SetActive(false);
                }

                    Potion_Dic_Herb1Process.sprite = ProcessWay[potion.process1];
                    Potion_Dic_Herb2Process.sprite = ProcessWay[potion.process2];

                if(openper >= 90)
                {
                    Potion_Dic_Herb1Amount.text = $"{potion.HerbAmount1} : {potion.HerbAmount2}";
                }
                else
                {
                    Potion_Dic_Herb1Amount.text = "";
                }

                if(openper >= 100)
                {
                    Potion_Dic_Liquid.gameObject.SetActive(true);
                }
                else
                {
                    Potion_Dic_Liquid.gameObject.SetActive(false);
                }
            }
            else
            {
                Potion_ItemInfo.gameObject.SetActive(true);
                Potion_DicInfo.gameObject.SetActive(false);

                Potion_Item_Liquid.text = "형태 : "+(item.shap?"고체":"액체");

                Potion_Item_Icon.sprite = (item.shap == potion.NonWater ? potion.itemImage : potion.NonShapeImage);

                Potion_Item_Herb1Name.text ="재료 1 : "+ (item.process1==0?"달인":item.process1==1?"빻은":item.process1==2?"말린":"")+" "+item.herb1.name;
                Potion_Item_Herb2Name.text = "재료 2 : " + (item.process2 == 0 ? "달인" : item.process2 == 1 ? "빻은" : item.process2 == 2 ? "말린" : "") + " " + item.herb2.name;

                Potion_Item_Herb1Icon.sprite = item.herb1.itemImage;
                Potion_Item_Herb2Icon.sprite = item.herb2.itemImage;

                Potion_Item_Herb1Process.sprite = ProcessWay[item.process1];
                Potion_Item_Herb2Process.sprite = ProcessWay[item.process2];

                Potion_Item_Herb1Amount.text = item.amount1.ToString();
                Potion_Item_Herb2Amount.text = item.amount2.ToString();
            }
        }
    }
}

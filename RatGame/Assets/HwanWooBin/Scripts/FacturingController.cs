using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacturingController : MonoBehaviour
{
    public DropSlot Herb1;
    public DropSlot Herb2;

    public DropSlot Result;

    ItemDatas ItemDatas;

    public ItemClass WillPotion;

    public Slider AmountSlider;
    public int Amount;
    public TextMeshProUGUI AmountText;

    public TextMeshProUGUI WaterText;

    public bool NonWater;
    public Animator WaterSlider;
    private void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
        Herb1.Load += WillLoad;
        Herb2.Load += WillLoad;
        Result.Load += WillLoad;
    }

    void ImageLoad(DropSlot slot)
    {
        Transform Image = slot.transform.GetChild(0);
        Image.gameObject.SetActive(true);
        Image.GetComponent<Image>().sprite = ItemDatas.items[slot.Item.itemNumber].itemImage;

        if (slot.Item.itemNumber == 7)
        {
            Image.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Image.GetChild(0).gameObject.SetActive(true);
            Image.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.inventoryManager.ProcessIcon[slot.Item.ProcessWay];
        }
    }

    public void WillLoad()
    {
        Result.transform.GetChild(0).gameObject.SetActive(false);
        bool notempty1 = Herb1.Item.itemNumber != -1;
        bool notempty2 = Herb2.Item.itemNumber != -1;
        bool empty = Result.Item.itemNumber == -1;



        if (notempty1)
        {
            ImageLoad(Herb1);
        }
        else
        {
            Herb1.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (notempty2)
        {
            ImageLoad(Herb2);
        }
        else
        {
            Herb2.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (!empty)
        {
            loadresult();
        }

        if (!notempty1 || !notempty2)
        {
            return;
        }

        WillPotion = new ItemClass();
        WillPotion.itemType = ItemType.Potion;
        WillPotion.ItemCount = 1;
        WillPotion.shap = NonWater;
        WillPotion.amount1 = Amount;
        WillPotion.amount2 = 10 - Amount;
        WillPotion.herb1 = ItemDatas.items[Herb1.Item.itemNumber] as HerbData;
        WillPotion.herb2 = ItemDatas.items[Herb2.Item.itemNumber] as HerbData;
        WillPotion.process1 = Herb1.Item.ProcessWay;
        WillPotion.process2 = Herb2.Item.ProcessWay;

        for (int i = 13; i < ItemDatas.items.Length; i++)
        {
            PotionData potion = ItemDatas.items[i] as PotionData;
            switch (potion.itemLevel)
            {
                case 1:
                    bool correct1 = (WillPotion.herb1 == potion.Herb1 && WillPotion.process1 == potion.process1);
                    bool correct2 = (WillPotion.herb2 == potion.Herb1 && WillPotion.process2 == potion.process1);
                    if (correct1 || correct2)
                    {
                        if (WillPotion.itemNumber != -1 && WillPotion.itemNumber != 12)
                        {
                            if (correct1)
                            {
                                if (WillPotion.amount1 > WillPotion.amount2)
                                {
                                    WillPotion.itemNumber = i;
                                }
                            }
                            else
                            {
                                if (WillPotion.amount1 < WillPotion.amount2)
                                {
                                    WillPotion.itemNumber = i;
                                }
                            }
                            if (WillPotion.amount1 == WillPotion.amount2)
                            {
                                WillPotion.itemNumber = 12;
                            }
                        }
                        else
                        {
                            WillPotion.itemNumber = i;
                        }
                    }
                    break;
                case 0:
                case 2:
                    correct1 = ((WillPotion.herb1 == potion.Herb1 && WillPotion.process1 == potion.process1) && (WillPotion.herb2 == potion.Herb2 && WillPotion.process2 == potion.process2));
                    correct2 = ((WillPotion.herb1 == potion.Herb2 && WillPotion.process1 == potion.process2) && (WillPotion.herb2 == potion.Herb1 && WillPotion.process2 == potion.process1));
                    if (correct1 || correct2)
                    {
                        WillPotion.itemNumber = i;
                    }
                    break;
                case 3:
                    correct1 = ((WillPotion.herb1 == potion.Herb1 && WillPotion.process1 == potion.process1) && (WillPotion.herb2 == potion.Herb2 && WillPotion.process2 == potion.process2) && Mathf.Abs(WillPotion.amount1 - potion.HerbAmount1) < 3);
                    correct2 = ((WillPotion.herb1 == potion.Herb2 && WillPotion.process1 == potion.process2) && (WillPotion.herb2 == potion.Herb1 && WillPotion.process2 == potion.process1) && Mathf.Abs(WillPotion.amount1 - potion.HerbAmount1) < 3);
                    if (correct1 || correct2)
                    {
                        WillPotion.itemNumber = i;
                    }
                    break;
            }
        }

        if (WillPotion.itemNumber == -1)
        {
            WillPotion.itemNumber = 12;
        }
    }

    public void ChangeShape()
    {
        NonWater = !NonWater;
        WaterSlider.SetBool("water", NonWater);
        if (NonWater)
        {
            WaterText.text = "고체";
        }
        else
        {
            WaterText.text = "액체";
        }
            WillLoad();
    }

    public void ChangeAmount()
    {
        AmountSlider.value = Mathf.RoundToInt(AmountSlider.value); //
        Amount = (int)AmountSlider.value;
        AmountText.text = Amount.ToString() + ":"+(10-Amount).ToString();
        WillLoad();
    }


    public void Make()
    {
        bool notempty1 = Herb1.Item.itemNumber != -1;
        bool notempty2 = Herb2.Item.itemNumber != -1;
        bool empty = Result.Item.itemNumber == -1;

        if (!notempty1 || !notempty2||!empty)
        {
            return;
        }
        /*
        PotionData potion = ItemDatas.items[WillPotion.itemNumber] as PotionData;


        switch (potion.itemLevel)
        {
            case 1:
                bool isoneOk = (WillPotion.herb1 == potion.Herb1);
                bool istwoOk = (WillPotion.herb2 == potion.Herb2);
                bool twoherbsame = ((WillPotion.herb1 == potion.Herb1 && WillPotion.herb2 == potion.Herb2) || (WillPotion.herb1 == potion.Herb2 && WillPotion.herb2 == potion.Herb1));
                bool oneamountsame = (isoneOk && (WillPotion.amount1 == potion.HerbAmount1))||(istwoOk && (WillPotion.amount2 == potion.HerbAmount1));
                bool sameeverything = (WillPotion.amount1 == potion.HerbAmount1 || WillPotion.amount1 )
                
                //50 + (재료 2개가 같은가?)*15 + (특정 약초의 비율이 같은가?)*15 + (모든 약초의 비율이 같은가?)*20
                WillPotion.Completeness = 50;
                break;
            case 2:
                //50 + (1번재료와 2번재료의 각각의 비율의 차이의 합이 5이하인가?)*10 + (1번재료의 비율이 같은가?)*20 + (2번재료의 비율이 같은가?)*20
                break;
            case 3:
                //50 + (1번재료의 비율이 같은가?) *10+(2번재료의 비율이 같은가?)*20+(모든 재료의 비율이 같은가?)*20
                break;
        }
        */
        Herb1.DeleteItem();
        Herb2.DeleteItem();

        Result.Item = WillPotion;
        WillPotion = new ItemClass();

        Result.Get = true;
        WillLoad();
    }

    void loadresult()
    {
        Transform slot = Result.transform.GetChild(0);
        slot.gameObject.SetActive(true);
        var itemdata = ItemDatas.items[Result.Item.itemNumber] as PotionData;
        if (itemdata.NonWater == Result.Item.shap)
        {
            slot.GetComponent<Image>().sprite = ItemDatas.items[Result.Item.itemNumber].itemImage;
        }
        else
        {
            slot.GetComponent<Image>().sprite = itemdata.NonShapeImage;
        }
    }
}

using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Store : MonoBehaviour
{
    public int[] Prices;
    ItemDatas ItemDatas;

    public StoreUI uis;

    //public ItemClass selectPotion;
    public int selectpotionnumber;
   

    //public List<ItemClass> selectPotionList;

    [Header("암시장")]
    public int MaxRatCount = 5;
    private int ratCount = 0;
    public int SellRatCount
    {
        get { return ratCount; }
        set
        {
            ratCount = Mathf.Clamp(ratCount + value, 0, MaxRatCount);
            uis.SellRatCountText.text = ratCount.ToString();
            GameManager.Instance.inventoryManager.ratDeliverycounts = ratCount;
        }
    }
    public int SellHerbRisk = 5;
    public int SellRatRisk = 10;
    int expertationMoney = 0;
    public int ExpectationMoney
    {
        get { return expertationMoney; }
        set
        {
            expertationMoney = value;
            uis.ExpectationMoneyText.text = "예상 사용 금액 : " + ExpectationMoney.ToString("#,###");
        }
    }

    private void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
        GameManager.Instance.store = this;
    }

    private void OnEnable()
    {
        uis.Reload();
    }

    [ContextMenu("BuyHerb")]
    public void Selling()
    {
        BuyHerb();
        Upgrade(3);
        ExpectationMoney = 0;
    }

    public void BuyHerb() // 나중에 날 넘어갈 때 사용
    {
        for(int i = 0; i < uis.herdTabs.Length; i++)
        {
            ItemBase data = ItemDatas.items[uis.herdTabs[i].itemNumder];
                if (GameManager.Instance.IsBuyed((int)data.Price * uis.herdTabs[i].ItemCount) && uis.herdTabs[i].ItemCount > 0)
                {
                for (int j = 0; j < uis.herdTabs[i].ItemCount; j++) {
                    GameManager.Instance.report.UseCoinInStore(data, true);
                }
                uis.herdTabs[i].Reset();
                uis.Reload();
                }

        }
    }

    public void Upgrade(int number)
    {
        if (GameManager.Instance.IsBuyed(Prices[number]))
        {
            // 업그레이드도 추가하기
            if (number == 3)
            {

                GameManager.Instance.darkstoreRisk += SellRatRisk;
                for(int ii = 0; ii< SellRatCount; ii++)
                GameManager.Instance.report.UseCoinInStore(number.ToString(), false, Prices[number], uis.RatBuy_Image); // 업그래이드 가공
                GameManager.Instance.IsBuyed(Prices[number] * (SellRatCount - 1)); // 쥐 수 만큼 돈 차감
                //GameManager.Instance.inventoryManager.ratDeliverycounts += SellRatCount;
                SellRatCount = 0;


            }
            else
            {
                GameManager.Instance.ProcessController.ProcessLevel[number]++;
                ExpectationMoney += Prices[number];
                GameManager.Instance.report.UseCoinInStore(number.ToString(), true, Prices[number] , uis.Proccessed_Image[number]); // 업그래이드 가공
                GameManager.Instance.ProcessController.SetProcessTime(1, true, number);
              
            }
            uis.Reload();
        }
    }

    // 쥐 판매 수 조절
    public void MouseCountUpper(bool isUpper)
    {

        if (isUpper)
        {
            if (SellRatCount < MaxRatCount)
                ExpectationMoney += Prices[3];
            SellRatCount = 1;
            
        }
        else
        {
            if (SellRatCount > 0)
                ExpectationMoney -= Prices[3];
            SellRatCount = -1;
           
        }
    }


    public void DarkStore()
    {

        for (int i = 0; i < GameManager.Instance.selectPotionList.Count; i++) // 야굼ㄹ 판매
        {
            GameManager.Instance.Money += (GameManager.Instance.selectPotionList[i].Completeness * 0.01f) * GameManager.Instance.itemDatas.items[GameManager.Instance.selectPotionList[i].itemNumber].Price * Random.Range(0.5f, 2f); // 약마다 랜덤한 가격으로 판매 가능
            GameManager.Instance.darkstoreRisk += SellHerbRisk;

            ItemBase itembase = ItemDatas.items[GameManager.Instance.selectPotionList[i].itemNumber];
            GameManager.Instance.report.UseCoinInStore(itembase, false);
            GameManager.Instance.report.SellPotion.Add(GameManager.Instance.selectPotionList[i]);

        }
        GameManager.Instance.selectPotionList.Clear();

        for (int i = 0; i < uis.DropInvenPos.Length; i++) // 오브젝트 재활용
        {

            uis.DropInvenPos[i].gameObject.SetActive(false);
          
        }

        uis.Reload();
    }



}

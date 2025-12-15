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

    public List<ItemClass> selectPotionList;

    [Header("구매할 쥐의 수")]
    public int MouseCountUp
    {
        get { return mouseSellCount; }
        set
        {
            mouseSellCount = value;
            mouseSellCount = Mathf.Clamp(mouseSellCount, 1, 5); // 1~5마리 사이로 설정
            uis.SellMouseCountText.text = "마리 수 : " + mouseSellCount.ToString();
        }
    }
    int mouseSellCount = 1;

    private void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
        GameManager.Instance.store = this;
    }

    private void OnEnable()
    {
        uis.Reload();
    }

    public void BuyHerb(int number)
    {
        ItemBase data = ItemDatas.items[number];

        if (GameManager.Instance.IsBuyed((int)data.Price))
        {
            GameManager.Instance.inventoryManager.deliverycounts[number]++;
            GameManager.Instance.report.UseCoinInStore(data, true);


            uis.Reload();
        }
    }

    public void Upgrade(int number)
    {
        if (GameManager.Instance.IsBuyed(Prices[number]))
        {
            // 업그레이드도 추가하기
            if (number == 3)
            {

                GameManager.Instance.darkstoreRisk += 10;
                GameManager.Instance.mouseCount += mouseSellCount;
                GameManager.Instance.MouseCount += mouseSellCount;
                GameManager.Instance.IsBuyed(Prices[number] * (mouseSellCount -1)); // 쥐 수 만큼 돈 차감
            }
            else
            {
                GameManager.Instance.ProcessController.ProcessLevel[number]++;

                GameManager.Instance.ProcessController.SetProcessTime(1, true, number);
            }
            uis.Reload();
        }
    }

    //public void Setpotion(int number)
    //{
    //    selectpotionnumber = number;

    //    selectPotion = GameManager.Instance.inventoryManager.inventory[selectpotionnumber];
    //}

    // 쥐 판매 수 조절
    public void MouseCountUpper(bool isUpper)
    {

        if (isUpper)
            MouseCountUp += 1;
        else
            MouseCountUp -= 1;
    }


    public void DarkStore()
    {

        for (int i = 0; i < selectPotionList.Count; i++)
        {
            GameManager.Instance.Money += (selectPotionList[i].Completeness * 0.01f) * GameManager.Instance.itemDatas.items[selectPotionList[i].itemNumber].Price * Random.Range(0.5f, 2f); // 약마다 랜덤한 가격으로 판매 가능
            GameManager.Instance.darkstoreRisk += 5;

            ItemBase itembase = ItemDatas.items[selectPotionList[i].itemNumber];
            GameManager.Instance.report.UseCoinInStore(itembase, false);
            GameManager.Instance.report.SellPotion.Add(selectPotionList[i]);

        }
        selectPotionList.Clear();

        for (int i = uis.DropInvenPos.childCount - 1; i >= 0; i--) // 오브젝트 재활용
        {

            uis.DropInvenPos.GetChild(i).gameObject.SetActive(false);
            uis.DropInvenPos.GetChild(i).transform.SetParent(uis.InvenPos);
        }

        uis.Reload();
    }



}

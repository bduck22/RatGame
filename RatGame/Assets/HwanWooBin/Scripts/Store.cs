using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public int[] Prices;
    ItemDatas ItemDatas;

    public StoreUI uis;

    //public ItemClass selectPotion;
    public int selectpotionnumber;

    public List<ItemClass> selectPotionList;

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

            //ItemClass itemdummy = new ItemClass();
            //itemdummy.itemNumber = number;
            //itemdummy.ItemCount = 1;
            //itemdummy.shap = false;
            //itemdummy.ProcessWay = -1;
            //itemdummy.itemType = ItemType.Herb;
            //GameManager.Instance.AddItem(itemdummy);
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
                GameManager.Instance.mouseCount++;
                GameManager.Instance.MouseCount++;
            }
            else
            {
                GameManager.Instance.ProcessController.ProcessLevel[number]++;
                GameManager.Instance.darkstoreRisk += 10;
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

    public void DarkStore()
    {
       

       // GameManager.Instance.Money += (selectPotion.Completeness * 0.01f) * GameManager.Instance.itemDatas.items[selectPotion.itemNumber].Price * Random.Range(0.5f, 2f); // 약마다 랜덤한 가격으로 판매 가능
       // GameManager.Instance.darkstoreRisk += 5;
       // GameManager.Instance.report.SellPotion.Add(GameManager.Instance.inventoryManager.inventory[selectpotionnumber]);
       // GameManager.Instance.inventoryManager.inventory.RemoveAt(selectpotionnumber);

        for(int i=0; i< selectPotionList.Count; i++)
        {
            GameManager.Instance.Money += (selectPotionList[i].Completeness * 0.01f) * GameManager.Instance.itemDatas.items[selectPotionList[i].itemNumber].Price * Random.Range(0.5f, 2f); // 약마다 랜덤한 가격으로 판매 가능
            GameManager.Instance.darkstoreRisk += 5;
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

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class InventoryManger : MonoBehaviour
{
    public int bannedItemCount = 0;

    // public Transform InvenPos;
    public GameObject InvenPre;

    public List<ItemClass> inventory = new List<ItemClass>();


    public bool Sawherb;
    public bool SawProcessed;
    public bool SawPotion;
    public bool NotFaildPotion;

    public Sprite HerbCase;
    public Sprite PotionCase;
    public Sprite NeedLevel;

    public Sprite[] ProcessIcon;

    public int[] deliverycounts = new int[5];
    public int ratDeliverycounts = 0;

    [Header("새로운 인벤토리")]
    public Transform[] page;
    public int pageMaxSlot = 9;
    public int nowPage = 1;
    public TextMeshProUGUI pageText;

    public int NowPage
    {
        get { return nowPage; }
        set
        {
            nowPage += value;
            nowPage = Mathf.Clamp(nowPage, 1, page.Length);
        }
    }

  
    public void UpdateInventory()
    {
        bannedItemCount = 0;
        for (int i = 0; i < page.Length; i++)
        {
            for (int j = 0; j < page[i].childCount; j++)
            {
                page[i].GetChild(j).gameObject.SetActive(false);

            }

        }


        int index = 0;
        for (int i = 0; i < inventory.Count; i++)//4
        {
            GameObject slot;
            int slotIndex = i - bannedItemCount;
            int pageIndex = slotIndex / pageMaxSlot;

            if (slotIndex % pageMaxSlot == 0 && slotIndex != 0) { index++; Debug.Log("페이지 넘기기"); }


            if (page[pageIndex].childCount <= i - bannedItemCount)
            {
                //생성
                slot = Instantiate(InvenPre, page[pageIndex]);

            }
            else
            {
                slot = page[pageIndex].GetChild(slotIndex % pageMaxSlot).gameObject;
                slot.SetActive(true);
            }


            ItemBase itemdata = GameManager.Instance.itemDatas.items[inventory[i].itemNumber];

            inventory[i].itemName = itemdata.itemName;
            inventory[i].itemDescription = itemdata.Explanation;

            if ((!Sawherb && itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay == -1) || // 아이템 밴하기
                (!SawPotion && itemdata.itemType == ItemType.Potion) ||
                (!SawProcessed && itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay != -1) ||
                (NotFaildPotion && inventory[i].itemNumber == 12)
                )
            {
                bannedItemCount++;
                slot.SetActive(false);
                continue;
            }

            slot.gameObject.name = (i).ToString();
            slot.GetComponent<Image>().sprite = itemdata.itemType == ItemType.Potion ? PotionCase : HerbCase;
            if (itemdata.itemType == ItemType.Potion)
            {
                slot.transform.GetChild(1).gameObject.SetActive(false);

                var Itemdata = itemdata as PotionData;


                if (Itemdata.NonWater == inventory[i].shap)
                {
                    slot.transform.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;
                }
                else
                {

                    slot.transform.GetChild(0).GetComponent<Image>().sprite = Itemdata.NonShapeImage;

                }
            }
            else
            {

                slot.transform.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;

                if (inventory[i].ProcessWay != -1 && inventory[i].ProcessWay != 3)
                {
                    slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    slot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ProcessIcon[inventory[i].ProcessWay];
                }
                else
                {
                    slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                }
            }
            slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (inventory[i].ItemCount <= 1 ? "" : inventory[i].ItemCount.ToString("#,###"));
        }

    }

    public int ItemCount(int itemnumber)
    {
        int count = 0;
        foreach (ItemClass item in inventory)
        {
            if (item.itemNumber == itemnumber && item.ProcessWay == -1)
            {
                count += item.ItemCount;
            }
        }

        return count;
    }


   void MovePage()
    {
        for (int i = 0; i < page.Length; i++)
        {
            page[i].gameObject.SetActive(false);
            if (i+1 == NowPage) { page[i].gameObject.SetActive(true); }
            
        }
    }

   
    public void movePage(bool pageUp)
    {
        NowPage = pageUp ? 1 : -1; // 오른쪽 왼쪽
        pageText.text = NowPage.ToString() +" / " + page.Length.ToString();
        MovePage();
    }
}

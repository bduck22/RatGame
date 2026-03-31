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

    public Transform SortPanel;
    public Image[] CheckImage;

    public bool IsSubmit;

    public DropItemInReport DropItemInReport;

    public Transform Info;
    [Header("정보창")]
    public Infomation Infomation;

    [Header("새로운 인벤토리")]
    public Transform page;
    public int pageMaxSlot = 9;
    public int nowPage = 0;
    [SerializeField] int bestpage = 1;
    public TextMeshProUGUI pageText;

    public int NowPage
    {
        get { return nowPage; }
        set
        {
            nowPage += value;
            nowPage = Mathf.Clamp(nowPage, 0, bestpage-1);
        }
    }

    public void SubmitPotion(int number)
    {
        DropItemInReport.AddItemInReportList(inventory[number]);
    }

    public void OpenInfo(bool IsDictio, int index)
    {
        Info.gameObject.SetActive(true);
        Infomation.ShowInfo(IsDictio, inventory[index]);
    }


    public void UpdateInventory()
    {
        bannedItemCount = 0;
        for (int i = 0; i < page.childCount; i++)
        {
                page.GetChild(i).gameObject.SetActive(false);
        }


        for (int i = NowPage*pageMaxSlot; i < (NowPage+1)*pageMaxSlot; i++)//4
        {
            GameObject slot;
            slot = page.GetChild(i % pageMaxSlot).gameObject;
            slot.SetActive(false);
            int itemnumber = i + bannedItemCount;
            if (itemnumber >= inventory.Count)
            {
                break;
            }

            //if (slotIndex % pageMaxSlot == 0 && slotIndex != 0)
            //{
            //    Debug.Log("페이지 넘기기");
            //} // text설정


            ItemBase itemdata = GameManager.Instance.itemDatas.items[inventory[itemnumber].itemNumber];

            if ((!Sawherb && itemdata.itemType == ItemType.Herb && inventory[itemnumber].ProcessWay == -1) || // 아이템 밴하기
                (!SawPotion && itemdata.itemType == ItemType.Potion) ||
                (!SawProcessed && itemdata.itemType == ItemType.Herb && inventory[itemnumber].ProcessWay != -1) ||
                (NotFaildPotion && inventory[itemnumber].itemNumber == 12)
                )
            {
                bannedItemCount++;
                i--;
                continue;
            }
            slot.SetActive(true);

            SetIconInfo(slot.transform, inventory[itemnumber], itemnumber);
        }

        bestpage = (inventory.Count-bannedItemCount) / 9 + ((inventory.Count - bannedItemCount) % 9 > 0 ? 1 : (inventory.Count - bannedItemCount)==0?1:0);
        pageText.text = (NowPage + 1).ToString() + " / " + bestpage.ToString();

    }

    public void LockOnOff(bool On)
    {
        for(int i = 0; i < 9; i++)
        {
            page.GetChild(i).GetComponentInChildren<MoveItem>().isStop = On;
        }
    }

    public void SetIconInfo(Transform slot, ItemClass Item, int itemNumber)
    {
        ItemBase itemdata = GameManager.Instance.itemDatas.items[Item.itemNumber];

        slot.gameObject.name = (itemNumber).ToString();
        slot.GetComponent<Image>().sprite = itemdata.itemType == ItemType.Potion ? PotionCase : HerbCase;
        if (itemdata.itemType == ItemType.Potion)
        {
            slot.transform.GetChild(1).gameObject.SetActive(false);

            var Itemdata = itemdata as PotionData;


            if (Itemdata.NonWater == Item.shap)
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

            if (Item.ProcessWay != -1 && Item.ProcessWay != 3)
            {
                slot.transform.GetChild(1).gameObject.SetActive(true);
                slot.transform.GetChild(1).GetComponent<Image>().sprite = ProcessIcon[Item.ProcessWay];
            }
            else
            {
                slot.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
        slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (Item.ItemCount <= 1 ? "" : Item.ItemCount.ToString("#,###"));
        slot.GetComponentInChildren<MoveItem>().LoadIndex();
    }

    public void OpenPanel()
    {
        SortPanel.gameObject.SetActive(!SortPanel.gameObject.activeSelf);
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

    public void OnOffBan(int type)
    {
        switch (type)
        {
            case 0:Sawherb = !Sawherb; break;
            case 1:SawProcessed = !SawProcessed; break;
            case 2:SawPotion = !SawPotion; break;
        }

        CheckImage[type].gameObject.SetActive((type==0?Sawherb:type==1?SawProcessed:SawPotion));
        UpdateInventory();
    }

   
    public void movePage(bool pageUp)
    {
        NowPage = pageUp ? 1 : -1; // 오른쪽 왼쪽
        pageText.text = NowPage.ToString() +" / " + bestpage.ToString();
        UpdateInventory();
    }
}

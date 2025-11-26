using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
    public int bannedItemCount = 0;

    public Transform InvenPos;
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
    public void UpdateInventory()
    {
        bannedItemCount = 0;
        for (int i = 0; i < InvenPos.childCount; i++)
        {
            InvenPos.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < inventory.Count; i++)//4
        {
            GameObject slot;
            if (InvenPos.childCount <= i - bannedItemCount)
            {
                //생성
                slot = Instantiate(InvenPre, InvenPos);
            }
            else
            {
                slot = InvenPos.GetChild(i - bannedItemCount).gameObject;
                slot.SetActive(true);
                //true
            }

            ItemBase itemdata = GameManager.Instance.itemDatas.items[inventory[i].itemNumber];

            if ((!Sawherb&& itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay == -1)|| // 아이템 밴하기
                (!SawPotion&& itemdata.itemType == ItemType.Potion)||
                (!SawProcessed&& itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay != -1) ||
                (NotFaildPotion&& inventory[i].itemNumber == 12)
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
                slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

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
        foreach(ItemClass item in inventory)
        {
            if (item.itemNumber == itemnumber&&item.ProcessWay==-1)
            {
                count+=item.ItemCount;
            }
        }

        return count;
    }
}

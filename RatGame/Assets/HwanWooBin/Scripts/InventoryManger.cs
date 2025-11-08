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

    public Sprite HerbCase;
    public Sprite PotionCase;
    public Sprite NeedLevel;

    public Sprite[] ProcessIcon;
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
                //»ý¼º
                slot = Instantiate(InvenPre, InvenPos);
            }
            else
            {
                slot = InvenPos.GetChild(i - bannedItemCount).gameObject;
                slot.SetActive(true);
                //true
            }

            ItemBase itemdata = GameManager.Instance.itemDatas.items[inventory[i].itemNumber];

            if (!Sawherb)
            {
                if (itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay == -1)
                {
                    bannedItemCount++;
                    slot.SetActive(false);
                    continue;
                }
            }

            if (!SawPotion)
            {
                if (itemdata.itemType == ItemType.Potion)
                {
                    bannedItemCount++;
                    slot.SetActive(false);
                    continue;
                }
            }

            if (!SawProcessed)
            {
                if (itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay != -1)
                {
                    bannedItemCount++;
                    slot.SetActive(false);
                    continue;
                }
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
}

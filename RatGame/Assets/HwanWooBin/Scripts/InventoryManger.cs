using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
    public int bannedItemCount = 0; // 밴된 아이템 수

    public Transform InvenPos;
    public GameObject InvenPre;

    public List<ItemClass> inventory = new List<ItemClass>();


    public bool Sawherb;        // 허브 보이기
    public bool SawProcessed;   // 가공품 보이기
    public bool SawPotion;      // 포션 보이기
    public bool NotFaildPotion;// 실패 포션 아니기?

    public Sprite HerbCase;
    public Sprite PotionCase;
    public Sprite NeedLevel;

    public Sprite[] ProcessIcon;

    public int[] deliverycounts = new int[5];  // 배달용 카운트
    public void UpdateInventory()
    {
        // 인벤토리 슬롯 모두 숨기기
        bannedItemCount = 0;
        for (int i = 0; i < InvenPos.childCount; i++)
        {
            InvenPos.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < inventory.Count; i++)//4
        {
            GameObject slot;

            // 아이템을 생성하는 거군
            if (InvenPos.childCount <= i - bannedItemCount)
            {
                //생성
                slot = Instantiate(InvenPre, InvenPos);
            }
            else // 근데 아이템이 있으면 그냥 보이게 하기
            {
                slot = InvenPos.GetChild(i - bannedItemCount).gameObject;
                slot.SetActive(true);
                //true
            }

            ItemBase itemdata = GameManager.Instance.itemDatas.items[inventory[i].itemNumber]; // 해당 번호에 있는 아이템 Base가져오기

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


using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class DropItemInReport : MonoBehaviour
{
    [Header("아이템 인벤토리 부분")]
    public GameObject sellPre;
    public Transform InvContentPos;
    InventoryManger inventoryManager;
    int bannedItemCount = 0;

    [Header("아이템 제출창 부분")]
    public ProjectReport report;
    public Transform submitContentPos;



    private void Awake()
    {
        inventoryManager = GameManager.Instance.inventoryManager;
        report.RemoveDinnerReport += DistroyedSwllObjects;
    }

    public void OnEnable()
    {
        InitializeSellPoints();
    }

    [ContextMenu("인벤토리 초기화")]
    public void InitializeSellPoints()
    {
        for(int i = 0; i< InvContentPos.childCount; i++)
        {
            InvContentPos.GetChild(i).gameObject.SetActive(false);
        }

        bannedItemCount = 0;
        for (int i = 0; i < inventoryManager.inventory.Count; i++)
        {

            ItemBase itemdata = GameManager.Instance.itemDatas.items[inventoryManager.inventory[i].itemNumber];


            if (itemdata.itemType != ItemType.Potion)
            {
                bannedItemCount++;
                continue;
            }

            GameObject slot;

            if (InvContentPos.childCount <= i - bannedItemCount) // 생성
            {
                slot = Instantiate(sellPre, InvContentPos);
            }
            else // 재활용 
            {
                slot = InvContentPos.GetChild(i - bannedItemCount).gameObject;
                slot.SetActive(true);
            }

            slot.gameObject.name = (i).ToString();
            slot.GetComponent<Image>().sprite = inventoryManager.PotionCase;
            slot.GetComponent<Button>().onClick.RemoveAllListeners(); // 모든 호출 삭제

            var Itemdata = itemdata as PotionData;
            if (Itemdata.NonWater == inventoryManager.inventory[i].shap)
            {
                slot.transform.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;
            }
            else
            {
                slot.transform.GetChild(0).GetComponent<Image>().sprite = Itemdata.NonShapeImage;
            }

            slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (inventoryManager.inventory[i].ItemCount <= 1 ? "" : inventoryManager.inventory[i].ItemCount.ToString("#,###"));

            // 나중에 약이 판매 슬롯에 있을 때 다시 인벤토리로 돌아가는 기능 추가해야함

            ItemClass itemdataindex = inventoryManager.inventory[i];

            slot.GetComponent<Button>().onClick.AddListener(() => AddItemInReportList(itemdataindex, slot));   // 아이템 제가 기능

            slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

          
        }

    
    }

    public void AddItemInReportList(ItemClass item, GameObject slot)
    {
        report.SellPotion.Add(item);
        inventoryManager.inventory.Remove(item);
        slot.transform.SetParent(submitContentPos);
        slot.GetComponent<Button>().onClick.RemoveAllListeners();
        slot.GetComponent<Button>().onClick.AddListener(() => RemoveItemInReportList(item, slot));
    }

    public void RemoveItemInReportList(ItemClass item, GameObject slot)
    {
        report.SellPotion.Remove(item);
        inventoryManager.inventory.Add(item);
        slot.transform.SetParent(InvContentPos);
        slot.GetComponent<Button>().onClick.RemoveAllListeners();
        slot.GetComponent<Button>().onClick.AddListener(() => AddItemInReportList(item, slot));
    }

    public void DistroyedSwllObjects() // 제출창 초기화
    {
        for (int i = 0; i < submitContentPos.childCount; i++)
        {
            submitContentPos.GetChild(i).gameObject.SetActive(false);
        }
    }


}

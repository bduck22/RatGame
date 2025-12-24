
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class DropItemInReport : MonoBehaviour
{
    [Header("아이템 인벤토리 부분")]
    public GameObject sellPre;
    InventoryManger inventoryManager;

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
        for(int i = 0; i < submitContentPos.childCount; i++)
        {
            submitContentPos.gameObject.SetActive(false);
        }

        for(int i = 0; i < report.SellPotion.Count; i++)
        {
            Transform Slot = submitContentPos.GetChild(i);
            Slot.gameObject.SetActive(true);
            inventoryManager.SetIconInfo(Slot, report.SellPotion[i], i);
        }
    }

    public void AddItemInReportList(ItemClass item)
    {
        report.SellPotion.Add(item);
        inventoryManager.inventory.Remove(item);
        InitializeSellPoints();
    }

    public void Undo(int number)
    {
        RemoveItemInReportList(report.SellPotion[number]);
    }

    public void RemoveItemInReportList(ItemClass item)
    {
        report.SellPotion.Remove(item);
        inventoryManager.inventory.Add(item);
        InitializeSellPoints();
    }

    public void DistroyedSwllObjects() // 제출창 초기화
    {
        for (int i = 0; i < submitContentPos.childCount; i++)
        {
            submitContentPos.GetChild(i).gameObject.SetActive(false);
        }
    }


}

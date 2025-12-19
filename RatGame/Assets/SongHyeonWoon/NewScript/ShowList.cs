using System;
using UnityEngine;


[Serializable]
public class ShowItem
{
    public GameObject itemObject;
    public int itemCount;

    [Header("해당 아이템 정보")]
    public string itemName;
    public string itemDescription;

    public ShowItem()
    {
        itemCount = 0;
    }
}
public class ShowList : MonoBehaviour
{
    public ShowItem[] HerbItems = new ShowItem[11];
    public ShowItem[] PotionItem = new ShowItem[5];
    public int EstimatedMoneyCount = 0;
    public int EstimatedRatCount = 0;
    GameManager manager;

    private void Awake()
    {
        manager = GameManager.Instance;

        for (int i = 0; i < manager.inventoryManager.deliverycounts.Length; i++) // 정보 초기화
        {
            HerbItems[i].itemName = manager.itemDatas.items[i].itemName;
            HerbItems[i].itemDescription = manager.itemDatas.items[i].Explanation;

        }
    }

    private void OnEnable()
    {
        ShowListAndUpdate();
    }

    public void ShowListAndUpdate()
    {
        for(int i=0; i < manager.inventoryManager.deliverycounts.Length; i++)
        {
            HerbItems[i].itemCount = manager.inventoryManager.deliverycounts[i]; // 구매 횟수 결정
        }
    }
}

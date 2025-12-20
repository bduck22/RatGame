using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;


[Serializable]
public class ShowItem
{
    public GameObject itemObject;
    public TextMeshProUGUI itemCountText;
    public int itemCount;
    public int ItemCount
    {
        get { return itemCount; }
        set
        {
            itemCount = value;
            if(itemCountText != null)
            itemCountText.text = itemCount.ToString();
        
        
        }
    }

    //[Header("해당 아이템 정보")]
    //public string itemName;
    //public string itemDescription;

    public ShowItem()
    {
        ItemCount = 0;
    }
}
public class ShowList : MonoBehaviour
{
    public ShowItem[] HerbItems = new ShowItem[11];
    public ShowItem[] PotionItems = new ShowItem[5];

    public TextMeshProUGUI EstimatedTotalMoneyCountText;
    public TextMeshProUGUI CurrentRistPoint;
    public TextMeshProUGUI EstimatedRatCountText;
    GameManager manager;

    private void Awake()
    {
        manager = GameManager.Instance;

        for (int i = 0; i < HerbItems.Length; i++) // 정보 초기화
        {
            HerbItems[i].itemCountText = HerbItems[i].itemObject?.GetComponentInChildren<TextMeshProUGUI>();

        }
    }

    private void OnEnable()
    {
        ShowListAndUpdate();
    }

    public void ShowListAndUpdate()
    {
        // GameManager의 inventory에서 deliverycounts의 값을 찾아서 쓰기
        int money = GameManager.Instance.report.TodayuseMoney;
        EstimatedTotalMoneyCountText.text = "예상 소모 금액 : " + (money != 0 ?money.ToString("#,###"):"0");
        CurrentRistPoint.text = "현재 위험도 : " + GameManager.Instance.darkstoreRisk.ToString() +"%";
        int rat = GameManager.Instance.inventoryManager.ratDeliverycounts;
        EstimatedRatCountText.text = "구매 예정\n" + $"<color=red>{rat.ToString()}마리</color>";

        for (int i=0; i < manager.inventoryManager.deliverycounts.Length; i++)
        {
            HerbItems[i].ItemCount = manager.inventoryManager.deliverycounts[i]; // 구매 횟수 결정
        }
    }
}

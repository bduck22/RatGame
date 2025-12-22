using System;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


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
    public Transform DarkStoreOpen;

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
            HerbItems[i].itemObject.transform.GetChild(0).GetComponent<Image>().sprite = ItemDatas.instance.items[i]?.itemImage;
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

       
        if (GameManager.Instance.DarkStoreIsOpen)
        {
            DarkStoreOpen.GetChild(4).gameObject.SetActive(false);
            DarkStoreOpen.GetChild(3).gameObject.SetActive(true);
           
        }
        else
        {
            DarkStoreOpen.GetChild(3).gameObject.SetActive(false);
            DarkStoreOpen.GetChild(4).gameObject.SetActive(true);


        }
        DarkStoreOpen.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "확정 오픈까지 D-" + (manager.DarkstoreConfirmedDay - manager.DarkstoreConfirmedDayCount).ToString();
        if (DarkStoreOpen.GetChild(3).gameObject.activeSelf == true)
            DarkStoreOpen.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "판매 예정 약품";

        for (int i = 0; i < PotionItems.Length; i++)
            {
                PotionItems[i].itemObject.SetActive(false);
            }

        for (int i = 0; i < manager.selectPotionList.Count; i++)
        {
            Debug.Log("보여;주기");
            PotionItems[i].itemObject.SetActive(true);
            PotionItems[i].itemObject.transform.GetChild(0).GetComponent<Image>().sprite = ItemDatas.instance.items[manager.selectPotionList[i].itemNumber]?.itemImage;
        }

    }
}

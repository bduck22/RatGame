using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


[Serializable]
public class DayList
{
    public int DayNumder = -1;  // 날짜
    public int UseMoney = -1; // 사용한 금액

    public void Reset()
    {
        DayNumder = -1;
        UseMoney = -1;
    }
}

[Serializable]
public class CheeseMoneyUse
{
    public string FromItemName;
    public int ItemCount = 0;

    public void Reset()
    {
        ItemCount = -1;
        ItemCount = 0;
    }

    public void BuyOrSelle(ItemBase useFrom)
    { 
        FromItemName = useFrom.itemName;
        ItemCount = 1;
    }
    public void MoreBuyOrSelle(ItemBase useFrom)
    {
        ItemCount++;
    }

}

public class ProjectReport : MonoBehaviour
{
    // 7일 간의 보고 ----------------------------------------------------------------
    [Header("날짜 내역")]
    public DayList SetDayList
    {

        set
        {

            for (int i = 0; i < dayLists.Length; i++)
            {
                if (dayLists[i].DayNumder == -1)
                {
                    dayLists[i].DayNumder = value.DayNumder;
                    dayLists[i].UseMoney = value.UseMoney;
                    return;
                }
            }

            for (int i = 1; i < dayLists.Length; i++)
            {
                dayLists[i - 1].DayNumder = dayLists[i].DayNumder;
                dayLists[i - 1].UseMoney = dayLists[i].UseMoney;
            }
            dayLists[dayLists.Length - 1] = value;

        }
    }
    public DayList[] dayLists = new DayList[7];


    [Header("실험 횟수")]
    public int RatTestCount;

    [Header("도감 내역")]
    public DicManager DicManager;


    [Header("약물 제출")]
    public List<ItemClass> SellPotion;

    [Header("성과")]
    public int SeccessfulExperiments;


    [Header("보고서")]
    public string ProjectReportText;


    // 매일 아침 내역서 ----------------------------------------------------------------

    // 상점
    public List<CheeseMoneyUse> StoreCheese;
    public int BestStoreAmount;
    // 암시장
    public List<CheeseMoneyUse> DarkStoreCheese;
    public int BestDarkAmount;

    private void Start()
    {

        for (int i = 0; i < 7; i++)
        {
            dayLists[i].Reset();
        }

        StoreCheese = new List<CheeseMoneyUse>();
        DarkStoreCheese = new List<CheeseMoneyUse>();
    }

    // 성과 보고, 7일에 한번 ----------------------------------------------------------------

    [ContextMenu("보고서 생성")]
    public void GenerateReport()
    {
        if ((GameManager.Instance.Day - 2) % 7 == 0) return; // 7일마다 반복


        // 실험 횟수------------------------------------------------------------------
        ProjectReportText += $"\n실험 횟수: {RatTestCount}회\n";


        // 도감 완성도----------------------------------------------------------------
        float put = 0;
        for (int i = 0; i < DicManager.OpenedPer.Count; i++)
        {
            put += DicManager.OpenedPer[i] * 0.01f;
            // ProjectReportText += $"{GameManager.Instance.itemDatas.items[DicManager.PotionData[i]]}: 도감 완성도 {DicManager.OpenedPer[i]}%\n";
        }

        ProjectReportText += $"도감 완성도 : {Mathf.RoundToInt(put / DicManager.OpenedPer.Count * 100f)}%";




        // 약물 제출------------------------------------------------------------------
        for (int i = 0; i < SellPotion.Count; i++)
        {
            for (int ii = 0; ii < DicManager.PotionData.Count; ii++)
            {
                if (DicManager.PotionData[ii] == SellPotion[i].itemNumber)
                {
                    int nowMoney = 0;

                    nowMoney += Mathf.RoundToInt((SellPotion[i].Completeness * 0.01f) * GameManager.Instance.itemDatas.items[DicManager.PotionData[ii]].Price);
                    nowMoney += Mathf.RoundToInt(ii - 1 <= -1 ? DicManager.OpenedPer[DicManager.OpenedPer.Count - 1] * 10 : DicManager.OpenedPer[ii - 1] * 10);

                    ProjectReportText += $"제출한 물약 : {GameManager.Instance.itemDatas.items[DicManager.PotionData[ii]].itemName} 금액 : (도감 완성도[{Mathf.RoundToInt(ii - 1 <= -1 ? DicManager.OpenedPer[DicManager.OpenedPer.Count - 1] * 10 : DicManager.OpenedPer[ii - 1] * 10)}] * 약물 가격[{GameManager.Instance.itemDatas.items[DicManager.PotionData[ii]].Price}])\n";
                    string moneyText = nowMoney.ToString("#,##0");
                    ProjectReportText += "총 금액 : " + moneyText + "\n";
                    SeccessfulExperiments += nowMoney;
                    break;
                }

            }

        }
        Debug.Log("보고서 체출 완료");

    }

    public void ResetReport()
    {


        RatTestCount = 0;
        SellPotion.Clear();
        ProjectReportText = null;
        SeccessfulExperiments = 0;
    }


    // 아침 보고, 매일 아침 구매한 목록들 표시 ----------------------------------------------------------------

    public void UseCoinInStore(ItemBase useFrom, bool isLegal)
    {

        CheeseMoneyUse ss = new CheeseMoneyUse();

        if (isLegal)
        {
            for(int i =0; i < StoreCheese.Count; i++)
            {
                if (StoreCheese[i].FromItemName == useFrom.itemName)
                {
                    StoreCheese[i].MoreBuyOrSelle(useFrom);
                    BestStoreAmount += (int)useFrom.Price;
                    return;
                }
            }
            ss.BuyOrSelle(useFrom);
            BestStoreAmount += (int)useFrom.Price;
            StoreCheese.Add(ss);
        }
        else
        {
            for (int i = 0; i < DarkStoreCheese.Count; i++)
            {
                if (DarkStoreCheese[i].FromItemName == useFrom.itemName)
                {
                    DarkStoreCheese[i].MoreBuyOrSelle(useFrom);
                    BestDarkAmount += (int)useFrom.Price;
                    return;
                }
            }
            
            ss.BuyOrSelle(useFrom);
            BestDarkAmount += (int)useFrom.Price;
            DarkStoreCheese.Add(ss);
        }


    }


    [ContextMenu("아침 보고서 생성")] // UI 적용시 추가할 예정-----------------------------------------------------------------------
    public void CheeseListReport()
    {


        // 치즈 내역서----------------------------------------------------------------
        if (StoreCheese == null) return;

        Debug.Log($"최종 자산 : {BestStoreAmount}\n");
        for (int i = 0; i < StoreCheese.Count; i++)
        {
            Debug.Log($"{StoreCheese[i].FromItemName} / {StoreCheese[i].ItemCount}\n");
        }


        if (DarkStoreCheese == null) return;

        Debug.Log($"\n최종 자산 : {BestDarkAmount}\n");

        for (int i = 0; i < DarkStoreCheese.Count; i++)
        {
            Debug.Log($"{DarkStoreCheese[i].FromItemName} / {DarkStoreCheese[i].ItemCount}\n");
        }

    }




}

using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

public class ProjectReport : MonoBehaviour
{
    Dictionary<string, int[]> CheeseSave;

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
                    SeccessfulExperiments += value.UseMoney;
                    return;
                }
            }

            for (int i = 1; i < dayLists.Length; i++)
            {
                dayLists[i - 1].DayNumder = dayLists[i].DayNumder;
                dayLists[i - 1].UseMoney = dayLists[i].UseMoney;
            }
            dayLists[dayLists.Length - 1] = value;
            SeccessfulExperiments += value.UseMoney;

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


    private void Start()
    {
        CheeseSave = new Dictionary<string, int[]>();
        
        for(int i =0; i < 7; i++)
        {
            dayLists[i].Reset();
        }
    }


    public void UseCheeseCoin(string useFrom, int pay)
    {

        if (!CheeseSave.ContainsKey(useFrom))
        {
            CheeseSave[useFrom] = new int[2];
            CheeseSave[useFrom][0] = 1;
            CheeseSave[useFrom][1] = pay;
        }
        else
        {
            CheeseSave[useFrom][0]++;
            CheeseSave[useFrom][1] += pay;
        }
    }

    [ContextMenu("보고서 생성")]
    public void GenerateReport()
    {
        if ((GameManager.Instance.Day - 2) % 7 != 0) return; // 7일마다 반복


        // 치즈 내역서----------------------------------------------------------------


        //var key = new List<string>(CheeseSave.Keys);
        //var pay = new List<int[]>(CheeseSave.Values);


        //for (int i = 0; i < CheeseSave.Count; i++)
        //{

        //    ProjectReportText += $"{key[i]}           {pay[i][0]}   {pay[i][1]}\n";
        //}


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
                    break;
                }

            }

        }
        Debug.Log("보고서 체출 완료");



    }

    public void ResetReport()
    {

        CheeseSave.Clear();
        RatTestCount = 0;
        SellPotion.Clear();
        
        ProjectReportText = null;
    }



}

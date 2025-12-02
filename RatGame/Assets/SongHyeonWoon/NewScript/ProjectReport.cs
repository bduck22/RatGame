using System.Collections.Generic;
using UnityEngine;

public class ProjectReport : MonoBehaviour
{
  
    [Header("치즈 코인 사용 내역서")]
 
    Dictionary<string, int[]> CheeseSave;




    [Header("실험 횟수")]
    public int RatTestCount;

    [Header("도감 내역")]
    public DicManager DicManager;


    //[Header("약물 제출")]



    [Header("보고서")]
    public string ProjectReportText;

    


    private void Start()
    {
        CheeseSave = new Dictionary<string, int[]>();
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
        // 치즈 내역서----------------------------------------------------------------
        ProjectReportText = "치즈 코인 사용 내역서\n상품명           수량   금액\n";


        var key = new List<string>(CheeseSave.Keys);
        var pay = new List<int[]>(CheeseSave.Values);


        for (int i = 0; i < CheeseSave.Count; i++)
        {
           
            ProjectReportText += $"{key[i]}           {pay[i][0]}   {pay[i][1]}\n";
        }

        
       
        
        
        for (int i = 0; i < DicManager.OpenedPer.Count; i++)
        {
           ProjectReportText += $"{GameManager.Instance.itemDatas.items[DicManager.PotionData[i]]}: 도감 완성도 {DicManager.OpenedPer[i]}%\n";
        }
    }

}

using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class ProjectReportUI : MonoBehaviour
{
    public ProjectReport report;
    [Header("ReportUIs")]
    public GameObject ReportUIs;
    public GameObject MorningReport;

    [Header("리포트 UI")]
    public Slider[] dayLine;
    public TextMeshProUGUI[] dayText;
    public Image[] dayLineRenderer;
    public int MinValue;
    public int MaxValue;


    [Header("여러가지 텍스트")]
    public TextMeshProUGUI WeekText;
    public TextMeshProUGUI D_DayText;
    public TextMeshProUGUI ResultScoreText;
    public TextMeshProUGUI SaveWarringText; // 누적 경고 횟수
    public TextMeshProUGUI RemainingBudget; // 남은 예산
    public TextMeshProUGUI PlusBudget; // 증가 예산
    public TextMeshProUGUI RiskText; // 이거는 딱히 안건들여도 될듯
    [TextArea]
    public string resultReportLog;
    public TextMeshProUGUI ResultReportText;
    [TextArea]
    public string warringLog;
    public TextMeshProUGUI WarringLogText;

    private void Start()
    {


        for (int i = 0; i < dayLine.Length; i++)
        {

            dayLine[i].maxValue = MaxValue;
            dayLine[i].minValue = MinValue;
            dayLine[i].gameObject.SetActive(false);
        }
    }



    public void RestUIUpdate()
    {

        for (int i = 0; i < report.dayLists.Length; i++) // 최대 최고값 조절
        {

            if (report.dayLists[i].UseMoney > MaxValue)
            {
                MaxValue = report.dayLists[i].UseMoney;
            }
            if (report.dayLists[i].UseMoney < MinValue)
            {
                MinValue = report.dayLists[i].UseMoney;
            }
        }

        for (int i = 0; i < report.dayLists.Length; i++)
        {
            if (report.dayLists[i].DayNumder == -1) continue;



            dayText[i].text = "Day " + report.dayLists[i].DayNumder.ToString();
            dayLine[i].maxValue = MaxValue;
            dayLine[i].minValue = MinValue;
            dayLine[i].value = report.dayLists[i].UseMoney;


        }

        for (int i = 0; i < dayLine.Length; i++)
        {
            if (report.dayLists[i].DayNumder == -1) continue;
            dayLine[i].gameObject.SetActive(true);

        }


        for (int i = 0; i < dayLine.Length; i++)
        {
            if (report.dayLists[i].DayNumder == -1) continue;

            Transform point1 = dayLine[i].transform.GetChild(0).GetChild(0);
            Transform point2 = i + 1 != dayLine.Length ? dayLine[i + 1].transform.GetChild(0).GetChild(0) : null;




            if (point1 != null && point2 != null)
            {
                Vector2 dir = point2.position - point1.position;


                Debug.Log("설정");


                dayLineRenderer[i].rectTransform.rotation = Quaternion.FromToRotation(Vector3.right, dir.normalized);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                float tipy = Mathf.Abs(angle) / 90;

                dayLineRenderer[i].rectTransform.localScale = new Vector2(Mathf.Clamp((tipy + 0.85f), 1, 1.8f), dayLineRenderer[i].rectTransform.localScale.y);
            }

        }

        // 리스트 작성
        WeekText.text = (Mathf.FloorToInt(GameManager.Instance.Day / 7)).ToString() + "주차 보고서 제출 결과";
        D_DayText.text = "마지막 날 D - " + (GameManager.Instance.LastDay - GameManager.Instance.Day).ToString();
        ResultScoreText.text = "성과\n" + ((report.SeccessfulExperiments != 0) ? report.SeccessfulExperiments.ToString("#,###") : "0");
        SaveWarringText.text = "현재\n누적 경고\n" + GameManager.Instance._WarringCount.ToString() + "회";

        RemainingBudget.text = GameManager.Instance.Money.ToString("#,###");
        PlusBudget.text = "예산 +\n" + ((report.SeccessfulExperiments != 0) ? report.SeccessfulExperiments.ToString("#,###") : "0");



        // 이건 나중에
        int dy = GameManager.Instance.Day % 7 == 0 ? 7 : 2;
        resultReportLog =
            dy.ToString() + " 일간 실험 횟수 : " + report.RatTestCount.ToString() + "회\n" +
            "도감 완성도 : " + report.DicSuccessCount.ToString() + "%\n" +
            "제출한 약물 : " + report.SellPotion.Count.ToString() + "개\n\n";


        //warringLog


        ResultReportText.text = resultReportLog;
        WarringLogText.text = warringLog;

    }

    public void AllUiOn()
    {
        //if (GameManager.Instance.Day <= 2) return;
        for (int i = 0; i < dayLineRenderer.Length; i++)
        {
            dayLineRenderer[i].gameObject.SetActive(true);
        }

        //Text.gameObject.SetActive(false);
    }

}

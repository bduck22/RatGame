using TMPro;
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


    [Header("Warring Text")]
    public TextMeshProUGUI warringText;


    private void Start()
    {
        MaxValue = 5000;
        MinValue = 0;


        for (int i = 0; i < dayLine.Length; i++)
        {
            
            dayLine[i].maxValue = MaxValue;
            dayLine[i].minValue = MinValue;
           
        }
    }


  
    public void DayLineUpdate()
    {

        for (int i = 0; i < report.dayLists.Length; i++) // 최대 최고값 조절
        {

            if (report.dayLists[i].UseMoney > MaxValue)
            {
                MaxValue = report.dayLists[i].UseMoney;
            }
            if(report.dayLists[i].UseMoney < MinValue)
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
            Transform point1 = dayLine[i].transform.GetChild(2).GetChild(0);
            Transform point2 = i + 1 != dayLine.Length ?dayLine[i + 1].transform.GetChild(2).GetChild(0) :null;

            if (point1 != null && point2 != null)
            {
                Vector2 dir = point2.position - point1.position;
                
                dayLineRenderer[i].rectTransform.rotation = Quaternion.FromToRotation(Vector3.right, dir.normalized);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                float tipy = Mathf.Abs(angle) / 90;

                dayLineRenderer[i].rectTransform.localScale = new Vector2(Mathf.Clamp((tipy+0.85f),1,1.4f), dayLineRenderer[i].rectTransform.localScale.y);

            }
           
        }
    }

    public void AllUiOn()
    {
        //if (GameManager.Instance.Day <= 2) return;
        for (int i = 0; i < dayLineRenderer.Length; i++)
        {
            dayLineRenderer[i].gameObject.SetActive(true);
        }

        warringText.gameObject.SetActive(false);
    }

}

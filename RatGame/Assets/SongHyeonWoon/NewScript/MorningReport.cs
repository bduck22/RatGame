using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MorningReport : MonoBehaviour
{
    public ProjectReport report;
    [Header("아이템 배치 Pos")]
    public Transform NormalStore_MPos;
    public Transform DarkStore_MPos;

    [Header("아침 구매내역 UI")]
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI NormalStore_Price;
    public TextMeshProUGUI DarkStore_Price;
    public TextMeshProUGUI TotalPrice;

    [Header("생성할 오브젝트")]
    public GameObject showUIObjectPre;

    private void OnEnable() // 활성화 시 오브젝트들 다시 정렬
    {
        ShowReportInMorning();
    }

    public void ShowReportInMorning()
    {
        DayText.text = GameManager.Instance.Day.ToString() + "일차";

        for (int i = 0; i < NormalStore_MPos.childCount; i++)
        {
            NormalStore_MPos.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < DarkStore_MPos.childCount; i++)
        {
            DarkStore_MPos.GetChild(i).gameObject.SetActive(false);
        }


        // rpeort를 통해 아이템 생성 여부 판단
        // 일반 상점
        ShowReportbeforeDay(report.StoreCheese.Count, true);
        NormalStore_Price.text = (-report.BestStoreAmount).ToString();

        // 다크 상점
        ShowReportbeforeDay(report.DarkStoreCheese.Count, false);
        DarkStore_Price.text = (report.BestDarkAmount).ToString();

        int total = -report.BestStoreAmount + report.BestDarkAmount;
        TotalPrice.text = "총 " + (total >= 0 ? "+" : "") + ((total != 0)?total.ToString("#,###") : "0");
    }

    public void ShowReportbeforeDay(int reportCount, bool isNormalStore)
    {
        GameObject slot = null;
        Transform tt = isNormalStore ? NormalStore_MPos : DarkStore_MPos;


        for (int i = 0; i < reportCount; i++)
        {
            if (tt.childCount <= i)
            {
                slot = Instantiate(showUIObjectPre, tt);

            }
            else
            {
                slot = tt.GetChild(i).gameObject;
                slot.SetActive(true);
            }
            slot.GetComponent<Image>().sprite = isNormalStore ? GameManager.Instance.inventoryManager.HerbCase : GameManager.Instance.inventoryManager.PotionCase;
            slot.transform.GetChild(0).GetComponent<Image>().sprite = isNormalStore ? report.StoreCheese[i].Image : report.DarkStoreCheese[i].Image;
            slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = isNormalStore ? report.StoreCheese[i].ItemCount.ToString() : report.DarkStoreCheese[i].ItemCount.ToString();

        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MorningReport : MonoBehaviour
{
    public ProjectReport report;
    [Header("아침 구매내역 UI")]
    public Transform NormalStore_MPos;
    public Transform DarkStore_MPos;

    [Header("생성할 오브젝트")]
    public GameObject showUIObjectPre;

    private void OnEnable()
    {
        ShowReportInMorning();
    }

    public void ShowReportInMorning()
    {
        // 전원 활성화 종료 -----------------------------------------------------------
        for(int i = 0; i < NormalStore_MPos.childCount; i++)
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
        // 다크 상점
        ShowReportbeforeDay(report.DarkStoreCheese.Count, false);

    }

    public void ShowReportbeforeDay(int reportCount, bool isNormalStore)
    {
        GameObject slot = null;
        Transform tt = isNormalStore? NormalStore_MPos : DarkStore_MPos;


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

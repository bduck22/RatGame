using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class DictionaryUi : MonoBehaviour
{
    public Transform DicPos;
    public Infomation Infomation;
    public int NowPage
    {
        get { return nowPage; }
        set
        {
            Debug.Log(value);
            if(value >= 0&&value < nowPage)
            {
                iconAnimator.SetTrigger("Left");
            }

            if(value < MaxPage&&value > nowPage)
            {
                iconAnimator.SetTrigger("Right");
            }
                nowPage = Mathf.Clamp(value, 0, MaxPage - 1);
        }
    }
    public int nowPage;
    public int MaxPage=2;

    public int PotionPageNum;

    public int HerbCount;
    public int PotionCount;

    public TextMeshProUGUI PageName;
    public TextMeshProUGUI PageNumber;

    private Animator iconAnimator;
    private void Awake()
    {
        iconAnimator = GetComponent<Animator>();
        SetDefault();
    }
    public void SetDefault()
    {
        PotionCount = ItemDatas.instance.items.Length - ItemDatas.instance.PotionIndex;
        HerbCount = ItemDatas.instance.items.Length - PotionCount;

        PotionPageNum = (Mathf.FloorToInt(HerbCount/16) + (HerbCount%16>0?1:0));
        MaxPage = PotionPageNum + (Mathf.FloorToInt(PotionCount / 16) + (PotionCount % 16 > 0 ? 1 : 0));

        LoadImage();
    }

    public void SawInfo(int number)
    {
        ItemClass item = new ItemClass();
        bool IsPotion = nowPage >= PotionPageNum;
        if (IsPotion)
        {
            item.itemNumber = number + HerbCount;
        }
        else 
        {
            item.itemNumber = number;
        }

        Infomation.gameObject.SetActive(true);
        Infomation.ShowInfo(true, item);
    }

    public void LoadImage()
    {

        for(int i = 0; i < 8; i++)
        {
            int pivotnum = 0;
            bool IsPotion = nowPage >= PotionPageNum;
            PageNumber.text = $"{nowPage+1} / {MaxPage}";
            PageName.text = "약초";
            if (IsPotion)
            {
                pivotnum = HerbCount;
                PageName.text = "약품";
            }
            bool IsLeftNumInherb = !IsPotion && (i + pivotnum) < HerbCount;
            bool IsRightNumInherb = !IsPotion && (i + pivotnum + 8) < HerbCount;

            if ((i + pivotnum) < ItemDatas.instance.items.Length&&(IsLeftNumInherb||IsPotion))
            {
                Image RightIcon = DicPos.GetChild(1).GetChild(1).GetChild(i).GetComponent<Image>();
                Image LeftIcon = DicPos.GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>();

                RightIcon.gameObject.SetActive(false);
                LeftIcon.sprite = ItemDatas.instance.items[i + pivotnum].itemImage;

                float openper;
                if (IsPotion)
                {
                    openper = GameManager.Instance.dicManager.OpenedPer[i + pivotnum - ItemDatas.instance.PotionIndex];
                    if (openper < 30)
                    {
                        LeftIcon.color = Color.black;
                    }
                    else
                    {
                        LeftIcon.color = Color.white;
                    }
                }




                LeftIcon.gameObject.SetActive(true);
                if ((i + pivotnum + 8) < ItemDatas.instance.items.Length - 1 && (IsRightNumInherb || IsPotion))
                {
                    RightIcon.sprite = ItemDatas.instance.items[i + pivotnum + 8].itemImage;

                    if (IsPotion)
                    {
                        openper = GameManager.Instance.dicManager.OpenedPer[i + pivotnum + 8 - ItemDatas.instance.PotionIndex];
                        if (openper < 30)
                        {
                            RightIcon.color = Color.black;
                        }
                        else
                        {
                            RightIcon.color = Color.white;
                        }
                    }

                    RightIcon.gameObject.SetActive(true);
                }
                continue;
            }
            DicPos.GetChild(0).GetChild(1).GetChild(i).gameObject.SetActive(false);
        }
    }

    public void MovePage(bool IsUp)
    {
        if (IsUp)
        {
            NowPage++;
        }
        else
        {
            NowPage--;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class DictionaryUi : MonoBehaviour
{
    public Transform DicPos;
    public int NowPage
    {
        get { return nowPage; }
        set
        {
            nowPage = Mathf.Clamp(value, 0, MaxPage);
        }
    }
    public int nowPage;
    public int MaxPage=2;

    public int PotionPageNum;

    public int HerbCount;
    public int PotionCount;
    private void Awake()
    {
        SetDefault();
    }
    public void SetDefault()
    {
        HerbCount = ItemDatas.instance.items.Length - ItemDatas.instance.PotionIndex;
        PotionPageNum = (HerbCount/16 + (HerbCount%16>0?1:0))+1;

        PotionCount = ItemDatas.instance.items.Length - HerbCount;
        MaxPage = PotionPageNum + (PotionCount / 16 + (PotionCount % 16 > 0 ? 1 : 0));
    }

    public void LoadImage(int pagenum)
    {
        for(int i = 0; i < 8; i++)
        {
            int pivotnum=0;
            if(pagenum >= PotionPageNum)
            {
                pivotnum = HerbCount + PotionCount - 16 * (pagenum-PotionPageNum);
            }
            else
            {
                pivotnum = HerbCount - 16 * pagenum;
            }
                DicPos.GetChild(0).GetChild(i).GetComponent<Image>().sprite = ItemDatas.instance.items[i+pivotnum].itemImage;
            if(ItemDatas.instance.items.Length - pivotnum > 8)
            {
                DicPos.GetChild(1).GetChild(i).GetComponent<Image>().sprite = ItemDatas.instance.items[i + pivotnum+8].itemImage;
            }
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
        LoadImage(NowPage);
    }
}

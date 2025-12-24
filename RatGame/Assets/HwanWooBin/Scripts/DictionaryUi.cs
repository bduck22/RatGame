using UnityEngine;
using UnityEngine.UI;

public class DictionaryUi : MonoBehaviour
{
    public Transform DicPos;
    public Infomation Infomation;
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
        PotionCount = ItemDatas.instance.items.Length - ItemDatas.instance.PotionIndex;
        PotionPageNum = (HerbCount/16 + (HerbCount%16>0?1:0))+1;

        HerbCount = ItemDatas.instance.items.Length - PotionCount;
        MaxPage = PotionPageNum + (PotionCount / 16 + (PotionCount % 16 > 0 ? 1 : 0));

        LoadImage(NowPage);
    }

    public void SawInfo(int number)
    {
        ItemClass item = new ItemClass();
        item.itemNumber = number + NowPage * 16;
        Infomation.gameObject.SetActive(true);
        Infomation.ShowInfo(true, item);
    }

    public void LoadImage(int pagenum)
    {
        for(int i = 0; i < 8; i++)
        {
            int pivotnum=0;
            if(pagenum >= PotionPageNum)
            {
                pivotnum = HerbCount + PotionCount - 16 * (pagenum - PotionPageNum);
            }

            DicPos.GetChild(0).GetChild(1).GetChild(i).gameObject.SetActive(true);
            DicPos.GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>().sprite = ItemDatas.instance.items[i+pivotnum].itemImage;
            if(ItemDatas.instance.items.Length - pivotnum - (pagenum>=PotionPageNum?0:PotionCount) > 8)
            {
                DicPos.GetChild(1).GetChild(1).GetChild(i).gameObject.SetActive(true);
                DicPos.GetChild(1).GetChild(1).GetChild(i).GetComponent<Image>().sprite = ItemDatas.instance.items[i + pivotnum+8].itemImage;
            }
            else
            {
                DicPos.GetChild(1).GetChild(1).GetChild(i).gameObject.SetActive(false);
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

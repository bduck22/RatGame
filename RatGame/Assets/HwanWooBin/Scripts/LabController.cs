using UnityEngine;
using UnityEngine.UI;

public class LabController : MonoBehaviour
{
    public int nowtype;

    public Sprite NotSelet;
    public Sprite Select;

    public Image[] Icons;

    public DropSlot Slot;
    public DropSlot CellSlot;
    ItemDatas ItemDatas;

    [Header("세포실험")]
    public int CellTestMoney = 500;
    public int CellTestLevel = 1;
    public int DefaultRate = 20; // 성공 실패 확률

    void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
        Refresh(0);
        Slot.Load += LoadImage;
        CellSlot.Load += LoadImage;
        dicManager = GameManager.Instance.dicManager;
    }



    public void LoadImage()
    {
        // 쥐 실험
        if (Slot.Item.itemNumber != -1)
        {
            Slot.transform.GetChild(0).gameObject.SetActive(true);
            PotionData data = ItemDatas.items[Slot.Item.itemNumber] as PotionData;
            if (Slot.Item.shap == data.NonWater)
            {
                Slot.transform.GetChild(0).GetComponent<Image>().sprite = data.itemImage;
            }
            else
            {
                Slot.transform.GetChild(0).GetComponent<Image>().sprite = data.NonShapeImage;
            }
        }
        else { Slot.transform.GetChild(0).gameObject.SetActive(false); }

        // 세포 실험
        if (CellSlot.Item.itemNumber != -1)
        {
            CellSlot.transform.GetChild(0).gameObject.SetActive(true);
            PotionData data = ItemDatas.items[CellSlot.Item.itemNumber] as PotionData;
            if (CellSlot.Item.shap == data.NonWater)
            {
                CellSlot.transform.GetChild(0).GetComponent<Image>().sprite = data.itemImage;
            }
            else
            {
                CellSlot.transform.GetChild(0).GetComponent<Image>().sprite = data.NonShapeImage;
            }
        }
        else { CellSlot.transform.GetChild(0).gameObject.SetActive(false); }
    }

    public void Refresh(int num)
    {
        for (int i = 0; i < 4; i++)
        {
            Icons[i].sprite = Select;
        }

        nowtype = num;
        Icons[num].sprite = NotSelet;
    }

    DicManager dicManager;

    public void Confirm()     // 쥐 실험 진행
    {
        if (GameManager.Instance.MouseCount > 0 && Slot.Item.itemNumber != -1)
        {
            GameManager.Instance.MouseCount--;
            GameManager.Instance.report.RatTestCount++;
            int DictioNum = Slot.Item.itemNumber - 13;



            if (DictioNum == 5)
            {
                int num = Random.Range(0, dicManager.OpenedPer.Count);
                dicManager.OpenedPer[num] += 5;
                if (dicManager.OpenedPer[num] > 100)
                {
                    dicManager.OpenedPer[num] = 100;
                }
            }
            else
            {
                PotionData potion = ItemDatas.items[Slot.Item.itemNumber] as PotionData;

                dicManager.OpenedPer[DictioNum] += potion.Persents[nowtype];
                if (potion.NonWater == Slot.Item.shap)
                {
                    dicManager.OpenedPer[DictioNum] += 5;
                }

                if (dicManager.OpenedPer[DictioNum] > 100)
                {
                    dicManager.OpenedPer[DictioNum] = 100;
                }
            }

            Slot.DeleteItem();
            LoadImage();
        }
    }


    public void CellConfirm() // 세포 실험 진행
    {
        if (GameManager.Instance.MouseCount > 0 && CellSlot.Item.itemNumber != -1)
        {
            if (Random.RandomRange(0, 101) >= DefaultRate)
            {



                GameManager.Instance.Money -= CellTestMoney;
                GameManager.Instance.report.RatTestCount++;
                int DictioNum = CellSlot.Item.itemNumber - 13;



                if (DictioNum == 5)
                {
                    int num = Random.Range(0, dicManager.OpenedPer.Count);
                    dicManager.OpenedPer[num] += 2;
                    if (dicManager.OpenedPer[num] > 100)
                    {
                        dicManager.OpenedPer[num] = 100;
                    }
                }
                else
                {
                    PotionData potion = ItemDatas.items[CellSlot.Item.itemNumber] as PotionData;

                    dicManager.OpenedPer[DictioNum] += (int)((potion.Persents[nowtype] / 10) * CellTestLevel);
                    if (potion.NonWater == CellSlot.Item.shap)
                    {
                        dicManager.OpenedPer[DictioNum] += 1;
                    }

                    if (dicManager.OpenedPer[DictioNum] > 100)
                    {
                        dicManager.OpenedPer[DictioNum] = 100;
                    }
                }
            }
            else
            {
                               Debug.Log("세포 실험 실패");
            }

                CellSlot.DeleteItem();
            LoadImage();
        }
    }
}

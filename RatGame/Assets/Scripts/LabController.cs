using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class LabController : MonoBehaviour
{
    public int nowtype;
    public bool RatLab = false;

    public Sprite NotSelet;
    public Sprite Select;

    public DropSlot Slot;
    //public DropSlot CellSlot;
    ItemDatas ItemDatas;

    [Header("세포실험")]
    public int CellTestMoney = 500;
    public int CellTestLevel = 1;
    public int DefaultRate = 20; // 성공 실패 확률

    public GameObject[] ConfirmTypeIcon = new GameObject[2];
    public GameObject[] ConfirmTypeText = new GameObject[2];
    public TextMeshProUGUI ConfirmText;

    [Header("Rat UI")]
    public TextMeshProUGUI Rat_ConfirmPriceoKineText;
    [Header("Cell UI")]
    public TextMeshProUGUI Cell_ConfirmPriceoKineText;

    public Image TestKindIcon;
    public Sprite[] IconsImage;
    DicManager dicManager;

    int page = 0;
    public int TestKindPage
    {
        get { return page; }
        set
        {
            page = (page + value + 4) % 4;
            nowtype = page;
        }
    }

    void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
        Slot.Load += LoadImage;
        ConfirmTypeSetting();
        dicManager = GameManager.Instance.dicManager;
    }

    public void ConfirmTypeSetting() // 실험 종류 설정
    {
        RatLab = !RatLab;
        if (RatLab)
        {
            nowtype = 0;
            page = 0;
            ConfirmText.text = "쥐 실험";
            ConfirmTypeIcon[0].SetActive(true);
            ConfirmTypeIcon[1].SetActive(false);
            ConfirmTypeText[0].SetActive(true);
            ConfirmTypeText[1].SetActive(false);

            Rat_ConfirmPriceoKineText.text = "<size=50>비용 : <color=yellow>쥐 1마리</color></size>\n\n" + "실험 종류 : <color=blue>병균</color>";
        }
        else
        {
            ConfirmText.text = "세포 실험";
            ConfirmTypeIcon[1].SetActive(true);
            ConfirmTypeIcon[0].SetActive(false);
            ConfirmTypeText[1].SetActive(true);
            ConfirmTypeText[0].SetActive(false);

            Cell_ConfirmPriceoKineText.text = $"비용 : <color=yellow>{CellTestMoney.ToString("#,###")}치즈코인</color>\r\n실패확률 : <color=red>{DefaultRate}%</color>\r\n성공 시 해금도 : <color=green>{CellTestLevel}%</color>";
        }
           

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

        //// 세포 실험
        //if (CellSlot.Item.itemNumber != -1)
        //{
        //    CellSlot.transform.GetChild(0).gameObject.SetActive(true);
        //    PotionData data = ItemDatas.items[CellSlot.Item.itemNumber] as PotionData;
        //    if (CellSlot.Item.shap == data.NonWater)
        //    {
        //        CellSlot.transform.GetChild(0).GetComponent<Image>().sprite = data.itemImage;
        //    }
        //    else
        //    {
        //        CellSlot.transform.GetChild(0).GetComponent<Image>().sprite = data.NonShapeImage;
        //    }
        //}
        //else { CellSlot.transform.GetChild(0).gameObject.SetActive(false); }
    }

    public void Refresh(int amount)
    {



        //for (int i = 0; i < 4; i++)
        //{
        //    TestKindIcon.sprite = Select;
        //}
        TestKindPage = amount;
        switch (nowtype)
        {
            case 0:
                Rat_ConfirmPriceoKineText.text = "<size=50>비용 : <color=yellow>쥐 1마리</color></size>\n\n" + "실험 종류 : <color=blue>병균</color>";
                break;
            case 1:
                Rat_ConfirmPriceoKineText.text = "<size=50>비용 : <color=yellow>쥐 1마리</color></size>\n\n" + "실험 종류 : <color=blue>X-Ray</color>";
                break;
            case 2:
                Rat_ConfirmPriceoKineText.text = "<size=50>비용 : <color=yellow>쥐 1마리</color></size>\n\n" + "실험 종류 : <color=blue>미로</color>";
                break;
            case 3:
                Rat_ConfirmPriceoKineText.text = "<size=50>비용 : <color=yellow>쥐 1마리</color></size>\n\n" + "실험 종류 : <color=blue>쳇바퀴</color>";
                break;
        }
        TestKindIcon.sprite = IconsImage[TestKindPage];
    }

 


    public void ConfirmStart()
    {
        if (RatLab) // 쥐실험이면
        {
            Confirm();
        }
        else  // 아니면
        {
            CellConfirm();
        }
    }


    void Confirm()     // 쥐 실험 진행
    {
        if (GameManager.Instance.MouseCount > 0 && Slot.Item.itemNumber != -1)
        {
            GameManager.Instance.MouseCount--;
            GameManager.Instance.report.RatTestCount++;
            int DictioNum = Slot.Item.itemNumber - 12;
            Debug.Log(DictioNum);


            if (DictioNum == 15)
            {
                int num = Random.Range(0, dicManager.OpenedPer.Count);
                dicManager.OpenedPer[num] += 5;
                if (dicManager.OpenedPer[num] > 100)
                {
                    dicManager.OpenedPer[num] = 100;
                }
                Debug.Log("---11");
            }
            else
            {
                PotionData potion = ItemDatas.items[Slot.Item.itemNumber] as PotionData;

                
                Debug.Log(dicManager.OpenedPer[DictioNum]);
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


    void CellConfirm() // 세포 실험 진행
    {
        if (GameManager.Instance.MouseCount > 0 && Slot.Item.itemNumber != -1)
        {
            if (Random.RandomRange(0, 101) >= DefaultRate)
            {



                GameManager.Instance.Money -= CellTestMoney;
                GameManager.Instance.report.RatTestCount++;
                int DictioNum = Slot.Item.itemNumber - 12;



                if (DictioNum == 15)
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
                    PotionData potion = ItemDatas.items[Slot.Item.itemNumber] as PotionData;

                    dicManager.OpenedPer[DictioNum] += (int)((potion.Persents[nowtype] / 10) * CellTestLevel);
                    if (potion.NonWater == Slot.Item.shap)
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

                Slot.DeleteItem();
            LoadImage();
        }
    }
}

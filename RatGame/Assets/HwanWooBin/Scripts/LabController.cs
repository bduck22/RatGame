using UnityEngine;
using UnityEngine.UI;

public class LabController : MonoBehaviour
{
    public int nowtype;

    public Sprite NotSelet;
    public Sprite Select;

    public Image[] Icons;

    public DropSlot Slot;

    ItemDatas ItemDatas;

    void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;
        Refresh(0);
        Slot.Load += LoadImage;

        dicManager = GameManager.Instance.dicManager;
    }



    public void LoadImage()
    {
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

    public void Confirm()
    {
        if (GameManager.Instance.MouseCount > 0&& Slot.Item.itemNumber!=-1)
        {
            GameManager.Instance.MouseCount--;
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
                    dicManager.OpenedPer[DictioNum] += 10;
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
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreUI : MonoBehaviour
{
    public int tabnumber;

    public Button[] buttons;

    public Transform[] tabs;

    public Transform potioninfo;

    public TextMeshProUGUI[] counts;
    public TextMeshProUGUI[] deliverys;
    public TextMeshProUGUI[] prices;

    public TextMeshProUGUI[] levels;

    public InventoryManger inventoryManager;
    public ItemDatas ItemDatas;

    public Transform InvenPos;

    public GameObject InvenPre;

    int bannedItemCount;

    public Store store;

    public Image PotionInfoSlot;

    [TextArea]
    public string[] Exs;
    private void Awake()
    {
        inventoryManager = GameManager.Instance.inventoryManager;
        ItemDatas = GameManager.Instance.itemDatas;
        movetab(0);
    }

    public void movetab(int number)
    {
        tabnumber = number;
        if (tabs[tabnumber].gameObject.activeSelf)
        {
            return;
        }
        else
        {
            potioninfo.gameObject.SetActive(false);
        }

            for (int i = 0; i < 3; i++)
            {
                buttons[i].interactable = true;
                tabs[i].gameObject.SetActive(false);
                buttons[i].GetComponent<Image>().color = Color.white;
            }

        buttons[tabnumber].interactable=false;
        tabs[tabnumber].gameObject.SetActive(true);
        buttons[tabnumber].GetComponent<Image>().color = Color.gray;

        Reload();
    }

    public void Reload()
    {
        bannedItemCount = 0;
        switch (tabnumber)
        {
            case 0:
                for (int i = 0; i < 5; i++)
                {
                    ItemBase data = ItemDatas.items[i];

                    prices[i].text = data.Explanation+"가격 : <color=\"yellow\">" + data.Price.ToString("#,##0") + " 치즈코인</color>";
                    deliverys[i].text = inventoryManager.deliverycounts[i].ToString("#,##0") + "개 배송예정";
                    counts[i].text = inventoryManager.ItemCount(i).ToString("#,##0");
                }
                break;
            case 1:
                for(int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            levels[i].text = "달이기 Lv.";
                            break;
                        case 1:
                            levels[i].text = "빻기 Lv.";
                            break;
                        case 2:
                            levels[i].text = "말리기 Lv.";
                            break;
                        case 3:
                            levels[i].text = "쥐 ";
                            break;
                    }
                    if (i == 3)
                    {
                        levels[i].text += GameManager.Instance.MouseCount.ToString("#,##0")+"마리";
                    }
                    else
                    {
                        levels[i].text += GameManager.Instance.ProcessController.ProcessLevel[i].ToString("#,##0");
                    }
                        prices[i + 5].text = Exs[i] +"가격 : <color=\"yellow\">" + store.Prices[i].ToString("#,##0") + " 치즈코인</color>";
                }
                break;
            case 2:
                for (int i = 0; i < inventoryManager.inventory.Count; i++)//4
                {
                    GameObject slot;
                    if (InvenPos.childCount <= i - bannedItemCount)
                    {
                        //생성
                        slot = Instantiate(InvenPre, InvenPos);
                    }
                    else
                    {
                        slot = InvenPos.GetChild(i - bannedItemCount).gameObject;
                        slot.SetActive(true);
                        //true
                    }

                    ItemBase itemdata = GameManager.Instance.itemDatas.items[inventoryManager.inventory[i].itemNumber];


                        if (itemdata.itemType != ItemType.Potion)
                        {
                        bannedItemCount++;
                            slot.SetActive(false);
                            continue;
                        }


                    slot.gameObject.name = (i).ToString();
                    slot.GetComponent<Image>().sprite = inventoryManager.PotionCase;
                    int index = i;
                    slot.GetComponent<Button>().onClick.AddListener(() => store.Setpotion(index));
                    slot.GetComponent<Button>().onClick.AddListener(openinfo);
                        slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

                        var Itemdata = itemdata as PotionData;
                        if (Itemdata.NonWater == inventoryManager.inventory[i].shap)
                        {
                            slot.transform.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;
                        }
                        else
                        {
                            slot.transform.GetChild(0).GetComponent<Image>().sprite = Itemdata.NonShapeImage;
                        }
                    
                    slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (inventoryManager.inventory[i].ItemCount <= 1 ? "" : inventoryManager.inventory[i].ItemCount.ToString("#,###"));
                }
                break;
        }
    }

    public void openinfo()
    {
        potioninfo.gameObject.SetActive(true);
        //포션 정보 출력

        PotionData itemdata = GameManager.Instance.itemDatas.items[store.selectPotion.itemNumber] as PotionData;


        if (itemdata.NonWater == store.selectPotion.shap)
        {
            PotionInfoSlot.sprite = itemdata.itemImage;
        }
        else
        {
            PotionInfoSlot.sprite = itemdata.NonShapeImage;
        }

        PotionInfoSlot.transform.parent.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemdata.name;
        PotionInfoSlot.transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text = "등급 : "+(itemdata.itemLevel==3?"전설":itemdata.itemLevel==2?"희귀":"일반")+"\n완성도 : "+store.selectPotion.Completeness.ToString("##0%")+"\n\n가격 : <color=\"yellow\">"+(itemdata.Price*(store.selectPotion.Completeness*0.01f))+"치즈코인</color>";
    }
}

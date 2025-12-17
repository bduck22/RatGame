using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Transform DropInvenPos;

    public GameObject InvenPre;

    int bannedItemCount;

    public Store store;

    public Image PotionInfoSlot;
    public TextMeshProUGUI SellMouseCountText;


    [TextArea]
    public string[] Exs;
    private void Awake()
    {
        inventoryManager = GameManager.Instance.inventoryManager;
        ItemDatas = GameManager.Instance.itemDatas;
    }

    private void OnEnable()
    {
        movetab(0);
    }

    public void movetab(int number)
    {

        if (number == 2 && !GameManager.Instance.DarkStoreIsOpen) { Debug.Log("오늘은 날이 아닙니다!"); return; }

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



        buttons[tabnumber].interactable = false;
        tabs[tabnumber].gameObject.SetActive(true);
        buttons[tabnumber].GetComponent<Image>().color = Color.gray;

        Reload();
    }

    public void Reload()
    {
        //potioninfo.gameObject.SetActive(false); // 아이템 설명 보이는 창
        bannedItemCount = 0;
        switch (tabnumber)
        {
            case 0:
                for (int i = 0; i < 5; i++)
                {
                    ItemBase data = ItemDatas.items[i];

                    prices[i].text = data.Explanation + "가격 : <color=\"yellow\">" + data.Price.ToString("#,##0") + " 치즈코인</color>";
                    deliverys[i].text = inventoryManager.deliverycounts[i].ToString("#,##0") + "개 배송예정";
                    counts[i].text = inventoryManager.ItemCount(i).ToString("#,##0");
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
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
                        levels[i].text += GameManager.Instance.mouseCount.ToString("#,##0") + "마리";
                    }
                    else
                    {
                        levels[i].text += GameManager.Instance.ProcessController.ProcessLevel[i].ToString("#,##0");
                    }
                    prices[i + 5].text = Exs[i] + "가격 : <color=\"yellow\">" + store.Prices[i].ToString("#,##0") + " 치즈코인</color>";
                }
                break;
            case 2:
                for (int i = 0; i < InvenPos.childCount; i++)
                {
                    InvenPos.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < inventoryManager.inventory.Count; i++)//4
                {

                    ItemBase itemdata = GameManager.Instance.itemDatas.items[inventoryManager.inventory[i].itemNumber];


                    if (itemdata.itemType != ItemType.Potion)
                    {
                        bannedItemCount++;
                        //slot.SetActive(false);
                        continue;
                    }


                    GameObject slot;

                    if (InvenPos.childCount <= i - bannedItemCount)
                    {
                        //생성
                        slot = Instantiate(InvenPre, InvenPos);
                        Debug.Log("생성");
                    }
                    else
                    {
                        slot = InvenPos.GetChild(i - bannedItemCount).gameObject;
                        slot.SetActive(true);
                        Debug.Log("재활용");
                        //true
                    }


                    slot.gameObject.name = (i).ToString();
                    slot.GetComponent<Image>().sprite = inventoryManager.PotionCase;
                    // int index = i;

                    slot.GetComponent<Button>().onClick.RemoveAllListeners(); // 모든 호출 삭제
                                                                             
                    //slot.GetComponent<Button>().onClick.AddListener(() => store.Setpotion(index));

                    ItemClass itemdataindex = inventoryManager.inventory[i];
                    slot.GetComponent<Button>().onClick.AddListener(() => DropSellPotion(slot, itemdataindex));

                    //slot.GetComponent<Button>().onClick.AddListener(openinfo);





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

    public void DropSellPotion(GameObject slot, ItemClass item)
    {
        slot.transform.SetParent(DropInvenPos);
        store.selectPotionList.Add(item);

        for (int i = 0; i < inventoryManager.inventory.Count; i++)
        {
            if (inventoryManager.inventory[i] == item)
            {
                inventoryManager.inventory.Remove(item); // 인벤토리에서 제거
                Debug.Log("포션 판매창으로 이동");
            }
        }

        slot.GetComponent<Button>().onClick.RemoveAllListeners();
        slot.GetComponent<Button>().onClick.AddListener(() => ReturenPotion(slot, item));
        Reload();
    }

    public void ReturenPotion(GameObject slot, ItemClass index)
    {
        slot.transform.SetParent(InvenPos);
        int ind = 0;
        inventoryManager.inventory.Add(index); // 인벤토리에 다시 추가

        for (int i = 0; i < inventoryManager.inventory.Count; i++)
        {
            if (inventoryManager.inventory[i] == index)
            {
                ind = i;
                break;
            }
        }

        store.selectPotionList.Remove(index);


        slot.GetComponent<Button>().onClick.AddListener(() => DropSellPotion(slot, index));

        Reload();
    }


   
}

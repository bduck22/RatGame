using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HerdTab
{
    public Transform Herd;
    public string itemName;
    public int itemNumder;
    private int count;
    public int ItemCount
    {
        get { return count; }
        set 
        {
            count = value;
            GameManager.Instance.inventoryManager.deliverycounts[itemNumder] = count;
        }
    }

    public int price;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI priceText;
    public Image itemImage;

    public Button up;
    public Button down;

  
    public void HerdTabUpdate()
    {
        ItemCount = 0;
        countText = Herd.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        priceText = Herd.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        up = Herd.transform.GetChild(1).GetChild(1).GetComponent<Button>();
        down = Herd.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        itemImage = Herd.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        Herd.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemName;
        countText.text = ItemCount.ToString();
        priceText.text = price.ToString("#,###");


        up.onClick.AddListener(upper);
        down.onClick.AddListener(under);
    }

    public void Reset()
    {
        ItemCount = 0;
        countText.text = ItemCount.ToString();
    }
    public void upper()
    {
        if (ItemCount < 99)
            GameManager.Instance.store.ExpectationMoney += price;
        ItemCount = Mathf.Clamp(ItemCount + 1, 0, 99);
        countText.text = ItemCount.ToString();
    }

    public void under()
    {
        if (ItemCount > 0)
            GameManager.Instance.store.ExpectationMoney -= price;
        ItemCount = Mathf.Clamp(ItemCount - 1, 0, 99);
        countText.text = ItemCount.ToString();
    }
}

public class StoreUI : MonoBehaviour
{
    [Header("Top")]
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI ExpectationMoneyText;
    public TextMeshProUGUI nowRiskText;


    public int tabnumber;

    public Button[] buttons;

    public RectTransform[] tabs;

    public RectTransform MainTabs;
    public HerdTab[] herdTabs;
    
    //public TextMeshProUGUI[] counts;
    //public TextMeshProUGUI[] deliverys;
    //public TextMeshProUGUI[] prices;

    public TextMeshProUGUI[] levels;

    public InventoryManger inventoryManager;
    public ItemDatas ItemDatas;

   

    public GameObject InvenPre;

    int bannedItemCount;

    public Store store;

    public Image PotionInfoSlot;
    public TextMeshProUGUI SellMouseCountText;

    [Header("아이템 업그래이드 이미지")]
    public Sprite[] Proccessed_Image = new Sprite[3];
    public Sprite RatBuy_Image;
    public TextMeshProUGUI[] Exs;

    [Header("암시장 UI")]
    public GameObject[] OnOpen;
    public Transform InvenPos;
    public Transform[] DropInvenPos;
    public TextMeshProUGUI SellHerbRiskText;
    public TextMeshProUGUI SellHerbAveragePrice;
    public TextMeshProUGUI RatRiskText;
    public TextMeshProUGUI CurrentRatCount;
    public TextMeshProUGUI SellRatCountText;
    public TextMeshProUGUI MaxRatCountText;
    public TextMeshProUGUI DarkStoryOpenDay;

    private void Awake()
    {
        inventoryManager = GameManager.Instance.inventoryManager;
        ItemDatas = GameManager.Instance.itemDatas;
       

    }
    private void Start()
    {
        for (int i = 0; i < herdTabs.Length; i++)
        {
            herdTabs[i].itemName = ItemDatas.items[i].itemName;
           
            herdTabs[i].itemNumder = i;
            herdTabs[i].price = (int)ItemDatas.items[i].Price;
            herdTabs[i].HerdTabUpdate();
            herdTabs[i].itemImage.sprite = ItemDatas?.items[i]?.itemImage != null ? ItemDatas.items[i]?.itemImage : herdTabs[i].itemImage.sprite;
        }
    }

    private void OnEnable()
    {

        movetab(0);
        
      
    }

    public void movetab(int number)
    {
        if (number >= 3)
        {
            gameObject.SetActive(false);
            return;
        }
        if(!GameManager.Instance.DarkStoreIsOpen) 
        {
            OnOpen[0].SetActive(true);
            OnOpen[1].SetActive(false);
            DarkStoryOpenDay.text = "확정 오픈까지 D-" + (GameManager.Instance.DarkstoreConfirmedDay - GameManager.Instance.DarkstoreConfirmedDayCount).ToString();
        }
        else
        {
            OnOpen[0].SetActive(false);
            OnOpen[1].SetActive(true);
        }


        tabnumber = number;
        float pos = -tabs[number].anchoredPosition.x;
        Vector2 moveing  = new Vector2(pos, MainTabs.anchoredPosition.y);

        MainTabs.anchoredPosition = moveing;

        for (int i = 0; i < 3; i++)
        {
            buttons[i].interactable = true;

            //tabs[i].gameObject.SetActive(false);
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
            //buttons[i].GetComponent<Image>().color = Color.white;
        }


        buttons[tabnumber].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 30f, 0f);
        buttons[tabnumber].interactable = false;
        //tabs[tabnumber].gameObject.SetActive(true);
        //buttons[tabnumber].GetComponent<Image>().color = Color.gray;

        Reload();
    }

    public void Reload()
    {

        bannedItemCount = 0;
        MoneyText.text = GameManager.Instance.Money.ToString("#,###");
        nowRiskText.text = "현재 위험도 : " + GameManager.Instance.darkstoreRisk.ToString() + "%";

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
                Exs[i].text = $"가공 시간 - {GameManager.Instance.ProcessController.ProcessLevel[i]}초\r\n가공 슬롯 + {Mathf.Clamp(GameManager.Instance.ProcessController.ProcessLevel[i],0,2)}칸";
            }
            
        }







        // 암시장------------------------------------------------------------------------------------------------------------

        
        SellHerbRiskText.text = "위험도 + " + store.SellHerbRisk.ToString() + "%";
        RatRiskText.text = "위험도 + " + store.SellRatRisk.ToString() + "%";
        CurrentRatCount.text = "<size=35>보유 수</size> :" + GameManager.Instance.mouseCount.ToString();
        MaxRatCountText.text = "최대\n" + store.MaxRatCount.ToString() + "마리";
        int averageprice = 0;

        if (GameManager.Instance.selectPotionList != null)
        {
            for (int i = 0; i < GameManager.Instance.selectPotionList.Count; i++)
            {
                averageprice += (int)ItemDatas.items[GameManager.Instance.selectPotionList[i].itemNumber].Price;
            }


            averageprice = averageprice / (GameManager.Instance.selectPotionList.Count+1);
        }

        SellHerbAveragePrice.text = "평균 가격 : " + averageprice.ToString("#,###");





        if (OnOpen[1].activeSelf == false) return;

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


            slot.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners(); // 모든 호출 삭제

            ItemClass itemdataindex = inventoryManager.inventory[i];
            slot.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => DropSellPotion(slot, itemdataindex));







            slot.transform.GetChild(1).gameObject.SetActive(false);

            var Itemdata = itemdata as PotionData;
            if (Itemdata.NonWater == inventoryManager.inventory[i].shap)
            {
                Debug.Log("포션 판매창으로 이동");
                slot.transform.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;
            }
            else
            {
                Debug.Log("포션 판매창으로 이동");
                slot.transform.GetChild(0).GetComponent<Image>().sprite = Itemdata.NonShapeImage;
            }

            //slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (inventoryManager.inventory[i].ItemCount <= 1 ? "" : inventoryManager.inventory[i].ItemCount.ToString("#,###"));
            slot.transform.GetChild(2).gameObject.SetActive(false);
        }


    }

    public void DropSellPotion(GameObject slot , ItemClass item)
    {
        //slot.transform.SetParent(DropInvenPos);
        int count = 0;
        for (int i = 0; i < DropInvenPos.Length; i++)
        {
            if(!DropInvenPos[i].gameObject.activeSelf)
            {
                DropInvenPos[i].gameObject.SetActive(true);
                DropInvenPos[i].GetChild(0).GetComponent<Image>().sprite = slot.transform.GetChild(0).GetComponent<Image>().sprite;

                DropInvenPos[i].GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                DropInvenPos[i].GetChild(0).GetComponent<Button>().onClick.AddListener(() => ReturenPotion(item , i));
                break;
            }
            count++;
        }
        if (count >= DropInvenPos.Length) return;
        
        slot.SetActive(false);
        GameManager.Instance.selectPotionList.Add(item);

       

        for (int i = 0; i < inventoryManager.inventory.Count; i++)
        {
            if (inventoryManager.inventory[i] == item)
            {
                inventoryManager.inventory.Remove(item); // 인벤토리에서 제거

            }
        }

        slot.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

        ItemBase itemdata = GameManager.Instance.itemDatas.items[item.itemNumber];
        store.ExpectationMoney += (int)itemdata.Price;
        Reload();
    }

    public void ReturenPotion(ItemClass indexx, int index)
    {

        if (indexx.ItemCount <= 0) return;
        GameObject slot = null;
        for(int i = 0; i < InvenPos.childCount; i++)
        {
            if (!InvenPos.GetChild(i).gameObject.activeSelf)
            {
                slot = InvenPos.GetChild(i).gameObject;
                slot.SetActive(true);
                break;
            }
        }

        if (slot == null) return;



        inventoryManager.inventory.Add(indexx); // 인벤토리에 다시 추가
        //for (int i = 0; i < inventoryManager.inventory.Count; i++)
        //{
        //    if (inventoryManager.inventory[i] == indexx)
        //    {
        //        break;
        //    }
        //}

        GameManager.Instance.selectPotionList.Remove(indexx);


        slot.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => DropSellPotion(slot, indexx));
        DropInvenPos[index].gameObject.SetActive(false);

        ItemBase itemdata = GameManager.Instance.itemDatas.items[indexx.itemNumber];
        store.ExpectationMoney -= (int)itemdata.Price;

        Reload();
    }


   
}

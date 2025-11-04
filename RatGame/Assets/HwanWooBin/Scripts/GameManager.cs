using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum Room
{
    약초방,
    실험실,
    침실
}

public enum ScreenType
{
    가공전체,
    가공,
    제조,
    실험전체,
    실험,
    침실전체,
    상점,
    침대
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("기능")]
    public Room room;
    public int nowRoom;
    public float Money;
    public ScreenType nowscreen;

    public int MouseCount;

    public int Day;

    [Header("인벤토리")]
    public ItemDatas itemDatas;

    public List<ItemClass> inventory = new List<ItemClass>();
    public Transform InvenPos;
    public GameObject InvenPre;

    public Sprite HerbCase;
    public Sprite PotionCase;
    public Sprite NeedLevel;

    public Sprite[] ProcessIcon;

    public ProcessController ProcessController;

    public bool Sawherb;
    public bool SawProcessed;
    public bool SawPotion;


    public int bannedItemCount = 0;
    [Header("UI")]

    public UIManager uimanager;

    public Animator ScreenFade;

    public Transform[] Screens;

    public Transform RoomControler;

    public bool stopping = false;

    public Animator dayani;

    void Awake()
    {
        nowRoom = 0;
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    void Update()
    {
        
    }

    public void AddItem(int Itemnumber, int process, float complete, bool shape)
    {
        ItemClass item = new ItemClass();
        item.itemNumder = Itemnumber;
        if (itemDatas.items[Itemnumber].itemType == ItemType.Potion)
        {
            item.ItemCount = 1;
            item.shap = shape;
            item.Completeness = complete;

            inventory.Add(item);
        }
        else
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].itemNumder == Itemnumber)
                {
                    if (inventory[i].ProcessWay == process)
                    {
                        inventory[i].ItemCount++;

                        UpdateInventory();
                        return;
                    }
                }
            }

            item.ProcessWay = process;
            item.ItemCount = 1;

            inventory.Add(item);
        }
        UpdateInventory();
    }

    public bool IsBuyed(int price)
    {
        if(Money >= price)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void playingday()
    {
        OnScreen(5);
        Day++;
        uimanager.UpdateDayText();
    }
    public void NextDay()
    {
        dayani.gameObject.SetActive(true);
        dayani.SetTrigger("Play");
    }

    public void NextDayEnd()
    {
        dayani.gameObject.SetActive(false);
    }

    public void OnScreen(int screen)
    {
        if (!stopping)
        {
            ScreenType screen1 = (ScreenType)screen;
            nowscreen = screen1;
            stopping = true;

            ScreenFade.SetTrigger("Play");
        }
    }

    public Transform SetOb;

    public Transform InventoryUI;

    public void Objectin(Transform ob)
    {
        SetOb = ob;
    }

    public void RealOnScreen()
    {
        foreach (Transform t in Screens)
        {
            t.gameObject.SetActive(false);
        }


        RoomControler.gameObject.SetActive(nowscreen == ScreenType.가공전체 || nowscreen == ScreenType.실험전체||nowscreen==ScreenType.침실전체);

        InventoryUI.gameObject.SetActive(false);

        switch (nowscreen)
        {
            case ScreenType.가공:
                SawPotion = false;
                SawProcessed = false;
                Sawherb = true;
                UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
            case ScreenType.제조:
                SawPotion = false;
                Sawherb = false;
                SawProcessed = true;
                UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
            case ScreenType.실험:
                Sawherb=false;
                SawProcessed = false;
                SawPotion = true;
                UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
        }

        SetOb.gameObject.SetActive(!SetOb.gameObject.activeSelf);

        Screens[(int)nowscreen].gameObject.SetActive(true);
    }


    public void moveRoom(int count)
    {
        if (!stopping)
        {
            nowRoom += count;
            nowRoom = (3 + nowRoom) % 3;//0  3
            room = (Room)nowRoom;
            stopping = true;
        }
    }

    public void falsestop()
    {
        stopping = false;
    }

    public Transform[] backgrounds;

    public void RefreshBackground()
    {
        for (int i = 0; i < 3; i++)
        {
            backgrounds[i].gameObject.SetActive(false);
        }
        backgrounds[nowRoom].gameObject.SetActive(true);
        switch (nowRoom)
        {
            case 0:
                Screens[0].gameObject.SetActive(true);
                break;
            case 1:
                Screens[3].gameObject.SetActive(true);
            break;
            case 2:
                Screens[5].gameObject.SetActive(true);
                break;
        }
    }

    public void UpdateInventory()
    {
        bannedItemCount = 0;
        for (int i = 0; i < InvenPos.childCount; i++)
        {
            InvenPos.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = 0; i < inventory.Count; i++)//4
        {
            GameObject slot;
            if (InvenPos.childCount <= i - bannedItemCount)
            {
                //생성
                slot = Instantiate(InvenPre, InvenPos);
            }
            else
            {
                slot = InvenPos.GetChild(i-bannedItemCount).gameObject;
                slot.SetActive(true);
                //true
            }

            ItemBase itemdata = itemDatas.items[inventory[i].itemNumder];

            if (!Sawherb)
            {
                if (itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay == -1)
                {
                    bannedItemCount++;
                    slot.SetActive(false);
                    continue;
                }
            }

            if (!SawPotion)
            {
                if(itemdata.itemType == ItemType.Potion)
                {
                    bannedItemCount++;
                    slot.SetActive(false);
                    continue;
                }
            }

            if (!SawProcessed)
            {
                if(itemdata.itemType == ItemType.Herb && inventory[i].ProcessWay != -1)
                {
                    bannedItemCount++;
                    slot.SetActive(false);
                    continue;
                }
            }

            slot.gameObject.name = (i).ToString();
            slot.GetComponent<Image>().sprite = itemdata.itemType==ItemType.Potion?PotionCase:HerbCase;
            slot.transform.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;
            slot.transform.GetChild(1).GetComponent<Text>().text = (inventory[i].ItemCount<=1?"": inventory[i].ItemCount.ToString("#,###"));

            if (itemdata.itemType == ItemType.Herb)
            {
                if (inventory[i].ProcessWay != -1)
                {
                    slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    slot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ProcessIcon[inventory[i].ProcessWay];
                }
                else
                {
                    slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                slot.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }

            //slot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = 
        }
    }
}

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

[DefaultExecutionOrder(-51)]
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

    [Header("하위 관리스크립트")]
    public ItemDatas itemDatas;

    public InventoryManger inventoryManager;

    public ProcessController ProcessController;

    public DicManager dicManager;

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

    public void AddItem(ItemClass Item)
    {
        ItemClass item = new ItemClass();
        item.itemNumber = Item.itemNumber;
        item.shap = Item.shap;
        item.Completeness = Item.Completeness;
        item.ProcessWay = Item.ProcessWay;
        item.itemType = Item.itemType;
        item.ItemCount = Item.ItemCount;
        if (itemDatas.items[item.itemNumber].itemType == ItemType.Potion)
        {
            inventoryManager.inventory.Add(item);
        }
        else
        {
            for (int i = 0; i < inventoryManager.inventory.Count; i++)
            {
                if (inventoryManager.inventory[i].itemNumber == item.itemNumber)
                {
                    if (inventoryManager.inventory[i].ProcessWay == item.ProcessWay)
                    {
                        inventoryManager.inventory[i].ItemCount+= Item.ItemCount;

                        inventoryManager.UpdateInventory();
                        return;
                    }
                }
            }

            inventoryManager.inventory.Add(item);
        }
        inventoryManager.UpdateInventory();
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
                inventoryManager.SawPotion = false;
                inventoryManager.SawProcessed = false;
                inventoryManager.Sawherb = true;
                inventoryManager.UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
            case ScreenType.제조:
                inventoryManager.SawPotion = false;
                inventoryManager.Sawherb = false;
                inventoryManager.SawProcessed = true;
                inventoryManager.UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
            case ScreenType.실험:
                inventoryManager.Sawherb =false;
                inventoryManager.SawProcessed = false;
                inventoryManager.SawPotion = true;
                inventoryManager.UpdateInventory();
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

    
}

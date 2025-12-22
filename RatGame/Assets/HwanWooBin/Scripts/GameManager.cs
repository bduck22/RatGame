using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum Room
{
    약초방,
    실험실,
    침실
}

public enum ScreenType
{
    제조실,
    가공,
    제조,
    실험실,
    실험,
    침실,
    상점,
    침대,
    설정,
    도감,
    가공상태,
    내역서,
    인벤토리,
    제출,
    영수증,
    보고서
}

[DefaultExecutionOrder(-51)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("기능")]
    public Room room;
    public int nowRoom;
    public float Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
            MoneyText.text = money.ToString("#,##0");
        }
    }
    private float money;

    public ScreenType nowscreen;

    public int MouseCount
    {
        get
        {
            return usingMouse;
        }
        set
        {
            usingMouse = value;

            MouseText.text = usingMouse.ToString() + "/" + mouseCount.ToString();
        }
    }
    public int mouseCount;

    private int usingMouse;

    public int Day;
    public int LastDay = 30;

    [Header("하위 관리스크립트")]
    public ItemDatas itemDatas;

    public InventoryManger inventoryManager;

    public ProcessController ProcessController;

    public DicManager dicManager;

    public ProjectReport report;

    [Header("UI")]

    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI MouseText;
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI RoomText;
    public TextMeshProUGUI weekText;
    public TextMeshProUGUI NextDayText;
    public TextMeshProUGUI NextNextText;

    public UIManager uimanager;

    public Animator ScreenFade;

    public Transform[] Screens;

    public Transform RoomControler;

    public bool stopping = false;

    public Animator dayani;

    public Animator GetItem;

    public Transform Background;

    public Transform X;

    [Header("암시장 스폰 확률")]
    public int OpenProbably = 40;           // 기본 확률 40%
    public int DarkstoreConfirmedDayCount = 0;
    public int DarkstoreConfirmedDay = 3;       // 반드시 암시장 오픈되는 날
    public List<ItemClass> selectPotionList; // 판매 약물
    public Store store;


    [Header("게임 패배")]
    public int _WarringCount = 0; // 경고가 3회 이상 쌓이면 게임 오버
    public int darkstoreRisk = 0;           // 위험도

    public bool DarkStoreIsOpen
    {
        get { return darkStoreIsOpen; }
        set
        {
            darkStoreIsOpen = value;
            if (darkStoreIsOpen)
            {
                DarkstoreConfirmedDayCount = 0;
            }
            else
            {
                DarkstoreConfirmedDayCount++;
                if (DarkstoreConfirmedDayCount >= DarkstoreConfirmedDay)
                {
                    darkStoreIsOpen = true;
                    DarkstoreConfirmedDayCount = 0;
                }
            }
        }


    }
    bool darkStoreIsOpen;

    void Awake()
    {
        nowRoom = 0;
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    private void Start()
    {
        InitGame();
    }

    public void InitGame()
    {
        Money = 5000;
        mouseCount = 5;

        StartDay();
    }

    public Transform EndGame;
    public void StartDay() // 시작할 때
    {
        MouseCount = mouseCount;
        Day++;
 
        DarkStoreIsOpen = (Random.RandomRange(0, 101) <= OpenProbably);
        report.RemoveTodayData();

        if (store != null) store.DarkStore(); // 암시장의 아이템 판매


        ProcessController.SetProcessTime(30, false, 0);

        for (int i = 0; i < inventoryManager.deliverycounts.Length; i++)
        {
            int count = inventoryManager.deliverycounts[i];
            if (count > 0)
            {
                AddItem(i, count);
            }
            inventoryManager.deliverycounts[i] = 0;
        }

        mouseCount += inventoryManager.ratDeliverycounts;
        MouseCount += inventoryManager.ratDeliverycounts;
        Debug.Log("하루 시작");
        //UI적 요소들 --------------------------------------------------------------------
        DayText.text = Day.ToString() + "일차";
        switch (Day % 7) 
        {
            case 0: weekText.text = "일"; break;
            case 1: weekText.text = "월"; break;
            case 2: weekText.text = "화"; break;
            case 3: weekText.text = "수"; break;
            case 4: weekText.text = "목"; break;
            case 5: weekText.text = "금"; break;
            case 6: weekText.text = "토"; break;
            default: weekText.text = "???"; break;
        }

        uimanager.UpdateDayText();

        //if (Day == 6)
        //{
        //    EndGame.gameObject.SetActive(true);
        //}

        if (Day > 1)
        {
            report.reportUI.MorningReport.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void AddItem(int number, int count) // 처음 시작 and 배달한 아이템 획득
    {
        //Debug.Log("AddItem함수 실행됨 - 아이템 번호: " + number + ", 개수: " + count);
        ItemBase data = itemDatas.items[number];

        ItemClass item = new ItemClass();
        item.itemNumber = number;
        item.ItemCount = count;

        item.itemName = data.itemName;
        item.itemDescription = data.Explanation;


        for (int i = 0; i < inventoryManager.inventory.Count; i++)
        {
            if (inventoryManager.inventory[i].itemNumber == number)
            {



                if (inventoryManager.inventory[i].ProcessWay == item.ProcessWay)
                {

                    inventoryManager.inventory[i].ItemCount += count;

                    inventoryManager.UpdateInventory();
                    return;
                }
            }
        }

        if (count <= 0)
        {
            return;
        }
        inventoryManager.inventory.Add(item);
        inventoryManager.UpdateInventory();
    }

    public void AddItem(ItemClass Item, bool isnewget) // 가공 된 아이템 획득
    {

        // Debug.Log("AddItem함수 실행됨 - 아이템 번호: " + Item + ", 개수: " + isnewget);
        ItemClass item = new ItemClass();
        item.itemName = Item.itemName;
        item.itemDescription = Item.itemDescription;
        item.itemNumber = Item.itemNumber;
        item.shap = Item.shap;
        item.Completeness = Item.Completeness;
        item.ProcessWay = Item.ProcessWay;
        item.itemType = Item.itemType;
        item.ItemCount = Item.ItemCount;

        item.amount1 = Item.amount1;
        item.amount2 = Item.amount2;
        item.herb1 = Item.herb1;
        item.herb2 = Item.herb2;
        item.process1 = Item.process1;
        item.process2 = Item.process2;

        ItemBase itemdata = GameManager.Instance.itemDatas.items[item.itemNumber];
        if (isnewget)
        {

            Transform GetSlot = GetItem.transform.GetChild(0); // background부분
            Transform Icon = GetItem.transform.GetChild(0).GetChild(0); // 약초 아이콘
            TextMeshProUGUI PlusText = GetSlot.GetChild(6).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI TypeText = GetSlot.GetChild(5).GetComponent<TextMeshProUGUI>();
            Transform Potioninfo = GetSlot.GetChild(3); // 아이템이 포션일 때
            Transform HerbInfo = GetSlot.GetChild(4);
            Potioninfo.gameObject.SetActive(false);
            HerbInfo.gameObject.SetActive(false);



            Icon.GetComponent<Image>().sprite = item.itemType == ItemType.Potion ? inventoryManager.PotionCase : inventoryManager.HerbCase;

            Icon.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;
            GetSlot.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = itemdata.itemName;
            GetSlot.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = itemdata.Explanation;
            TypeText.text = "약물";

            if (item.itemType == ItemType.Potion)
            {
                Icon.GetChild(1).gameObject.SetActive(false);
                var Itemdata = itemdata as PotionData;
                if (Itemdata.NonWater == item.shap)
                {
                    PlusText.text = "약물";
                    Icon.GetChild(0).GetComponent<Image>().sprite = Itemdata.itemImage;
                    GetSlot.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = Itemdata.itemName;


                }
                else
                {
                    PlusText.text = "약물";
                    Icon.GetChild(0).GetComponent<Image>().sprite = Itemdata.NonShapeImage;
                    GetSlot.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = Itemdata.NonShapeName;

                }


                //약물 전용 데이터 출력
                Potioninfo.gameObject.SetActive(true);

                if (Itemdata.Herb1 != null && Itemdata.Herb2 != null)
                {
                    string proccess1 = "달인 ";
                    string proccess2 = "달인 ";

                    if (Itemdata.process1 == 1) proccess1 = "빻은 ";
                    if (Itemdata.process1 == 2) proccess1 = "말린 ";
                    if (Itemdata.process2 == 1) proccess2 = "빻은 ";
                    if (Itemdata.process2 == 2) proccess2 = "말린 ";

                    Potioninfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = "재료 1 : " + proccess1 + Itemdata.Herb1.itemName;
                    Potioninfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = "재료 2 : " + proccess2 + Itemdata.Herb2.itemName;
                }
                else
                {
                    Potioninfo.GetChild(0).GetComponent<TextMeshProUGUI>().text = "재료 1 : X";
                    Potioninfo.GetChild(1).GetComponent<TextMeshProUGUI>().text = "재료 2 : X";
                }

                // 나중에 이미지도 들어갈 수 있음
                Potioninfo.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Itemdata.Herb1?.itemImage;
                Potioninfo.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>().sprite = Itemdata.Herb2?.itemImage;

               // Potioninfo.GetChild(1).GetChild(0).GetComponent<Image>().sprite = inventoryManager.ProcessIcon[Itemdata.Herb1.itemProcessedWay];    




            }
            else // 허브일 때
            {
                //Icon.GetChild(0).GetComponent<Image>().sprite = itemdata.itemImage;
                // GetSlot.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = itemdata.name;

                Transform ccc = HerbInfo.GetChild(1).transform;
                GameObject[] proccessedState = { ccc.GetChild(0).gameObject, ccc.GetChild(1).gameObject, ccc.GetChild(2).gameObject, ccc.GetChild(3).gameObject };
                TypeText.text = "약초";

                if (item.ProcessWay != -1 && item.ProcessWay != 3)
                {
                    Icon.GetChild(1).gameObject.SetActive(true);
                    Icon.GetChild(1).GetComponent<Image>().sprite = inventoryManager.ProcessIcon[item.ProcessWay];
                }
                else
                {
                    Icon.GetChild(1).gameObject.SetActive(false);
                }

                HerbInfo.gameObject.SetActive(true);
                for (int i = 0; i < proccessedState.Length; i++)
                {
                    proccessedState[i].gameObject.SetActive(false);

                }

                PlusText.text = (item.ProcessWay == 0 ? "달이기" : item.ProcessWay == 1 ? "빻기" : item.ProcessWay == 2 ? "말리기" : "없음");
                HerbInfo.GetComponentInChildren<TextMeshProUGUI>().text = "가공 상태 : " + (item.ProcessWay == 0 ? "달이기" : item.ProcessWay == 1 ? "빻기" : item.ProcessWay == 2 ? "말리기" : "X");
                switch (item.ProcessWay)
                {
                    case 0: proccessedState[1].gameObject.SetActive(true); break;
                    case 1: proccessedState[2].gameObject.SetActive(true); break;
                    case 2: proccessedState[3].gameObject.SetActive(true); break;
                    default: proccessedState[0].gameObject.SetActive(true); break;
                }


            }
            //Icon.GetComponentInChildren<TextMeshProUGUI>().text = (item.ItemCount <= 1 ? "" : item.ItemCount.ToString("#,###"));

            //GetSlot.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemdata.Explanation;
            GetItem.Play("GetItem");
        }

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
                        inventoryManager.inventory[i].ItemCount += Item.ItemCount;

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
        if (Money >= price)
        {
            Money -= price;
            return true;
        }
        else
        {
            Money -= price;
            return true;
        }
    }

    public void playingday()
    {
        AddDayData();
        store?.Selling(); // 모든 물품 판매
        if (ReportDayChick()) // 리포트 날
        {
            
            CreateReport();
            return;
        }

        OnScreen(5);
        StartDay();

    }

    public void WarringEvent()
    {
        if (Random.Range(0, 101) <= darkstoreRisk)
        {
            _WarringCount++; // 위험도에 따라 경고 횟수 증가
            report.reportUI?.RiskText.gameObject.SetActive(true);
            //report.reportUI?.warringLog.gameObject.SetActive(true);
        }

        if (_WarringCount >= 3)
        {
            // 게임 오버 처리
            Debug.Log("게임 오버!");
        }
    }

    public void CreateReport()
    {
        Debug.Log("리포트 생성");

        report.GenerateReport(); // 리보트 생성
        WarringEvent();          // 경고 이벤트 체크
        //dayani.SetTrigger("ReportDay"); // ----------
        //SkipReport();
    }

    [ContextMenu("리포트 스킵")]
    public void SkipReport() // 나중에는 button으로
    {
        report.ResetReport();
        dayani.SetTrigger("Close");
        OnScreen(5);
        StartDay();

        darkstoreRisk = 0; // 리포트 날에 리스크 초기화
    }

    public void AddDayData() //Day리스트 추가
    {


        DayList reportList = new DayList();
        reportList.DayNumder = Day;
        reportList.UseMoney = (int)Money;
        Debug.Log("다음날");
        report.SetDayList = reportList;

    }
    public void NextDay()
    {
        dayani.gameObject.SetActive(true);
        dayani.SetTrigger("Open");

        if (!ReportDayChick()) // 리포트 날
        {
            dayani.SetTrigger("Close");
        }
           
    }
    public bool ReportDayChick()
    {
        if ((Day - 2) % 7 == 0) // 리포트 날 확인
        {
            return true;
        }
        else return false;
    }

    public void NextDaying()
    {
        NextDayText.text = Day.ToString() + "일차";
        NextNextText.text = weekText.text;
    }

    public void NextDayEnd()
    {
        //dayani.SetTrigger("Close");
        dayani.gameObject.SetActive(false);
    }

    public void OnScreen(int screen)
    {
        if (!stopping)
        {
            ScreenType screen1 = (ScreenType)screen;
            nowscreen = screen1;
            stopping = true;

            ScreenFade.SetTrigger("On");
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
            if (t != null)
            {
                t.gameObject.SetActive(false);
            }
        }

       X.gameObject.SetActive(nowscreen != ScreenType.도감 && nowscreen != ScreenType.설정&& nowscreen != ScreenType.제조실 && nowscreen != ScreenType.실험실 && nowscreen != ScreenType.침실 && nowscreen != ScreenType.제조 && nowscreen != ScreenType.상점);

        Background.gameObject.SetActive(nowscreen != ScreenType.제조실 && nowscreen != ScreenType.실험실 && nowscreen != ScreenType.침실);


        RoomControler.gameObject.SetActive(nowscreen == ScreenType.제조실 || nowscreen == ScreenType.실험실 || nowscreen == ScreenType.침실);

        InventoryUI.gameObject.SetActive(false);

        inventoryManager.SawPotion = false;
        inventoryManager.SawProcessed = false;
        inventoryManager.Sawherb = false;
        inventoryManager.NotFaildPotion = false;
        switch (nowscreen)
        {
            case ScreenType.가공:
                ProcessController.UpdateHerb();
                inventoryManager.Sawherb = true;
                inventoryManager.UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
            case ScreenType.제조:
                inventoryManager.SawProcessed = true;
                inventoryManager.UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
            case ScreenType.실험:
                inventoryManager.NotFaildPotion = true;
                inventoryManager.SawPotion = true;
                inventoryManager.UpdateInventory();
                InventoryUI.gameObject.SetActive(true);
                break;
        }

        SetOb.gameObject.SetActive(!SetOb.gameObject.activeSelf);

        if(Screens[(int)nowscreen] != null)
        {
            Screens[(int)nowscreen].gameObject.SetActive(true);
        }
    }


    public void moveRoom(int count)
    {
        if (!stopping)
        {
            nowRoom += count;
            nowRoom = (3 + nowRoom) % 3;//0  3
            room = (Room)nowRoom;
            stopping = true;
            RoomText.text = room.ToString(); // UI적 요소들
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

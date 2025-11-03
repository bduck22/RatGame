using UnityEngine;


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

    [Header("기능관련")]
    public Room room;
    public int nowRoom;
    public float Money;
    public ScreenType nowscreen;

    public int Day;

    [Header("UI 관련")]
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
            case ScreenType.제조:
            case ScreenType.실험:
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

using UnityEngine;


public enum Room
{
    ���ʹ�,
    �����,
    ħ��
}

public enum ScreenType
{
    ������ü,
    ����,
    ����,
    ������ü,
    ����,
    ħ��,
    ����
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Room room;
    public int nowRoom;
    public float Money;
    public ScreenType nowscreen;

    public bool stopping = false;

    public Animator ScreenFade;

    public Transform[] Screens;

    public Transform RoomControler;

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

            RoomControler.gameObject.SetActive(nowscreen == ScreenType.������ü || nowscreen == ScreenType.������ü);

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
    }
}

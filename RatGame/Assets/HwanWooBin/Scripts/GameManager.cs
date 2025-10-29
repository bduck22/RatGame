using UnityEngine;


public enum Room
{
    약초방,
    실험실,
    침실
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Room room;
    public int nowRoom;


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

    public void moveRoom(int count)
    {
        nowRoom += count;
        nowRoom = (3 + nowRoom)%3;//0  3
        room = (Room)nowRoom;
    }

    public Transform[] backgrounds;

    public void RefreshBackground()
    {
        for(int i = 0; i < 3; i++)
        {
            backgrounds[i].gameObject.SetActive(false);
        }
        backgrounds[nowRoom].gameObject.SetActive(true);
    }
}

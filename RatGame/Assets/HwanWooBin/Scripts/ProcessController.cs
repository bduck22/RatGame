using UnityEngine;
using UnityEngine.UI;

public class ProcessController : MonoBehaviour
{
    public ItemClass[] itemslots = new ItemClass[9];
    public float[] itemtimes = new float[9];

    public int[] ProcessLevel = new int[3];

    ItemDatas ItemDatas;

    public Sprite ProcessTypeIcon;
    public Transform ProcessWindow;

    public Image[] Slots = new Image[9];
    public Image[] slotProcessIcon = new Image[9];
    void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;

        for (int i = 0; i < 9; i++)
        {
            itemtimes[i] = -1f;
            Slots[i].transform.parent.GetComponent<DropSlot>().Load += UpdateHerb;
            slotProcessIcon[i] = Slots[i].transform.GetChild(0).GetComponent<Image>();
        }
        UpdateHerb();
    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (ProcessLevel[i % 3] >= i % 3) // 해당 칸이 해금되어있는가
            {
                if (itemslots[i].itemNumder != -1 && itemtimes[i]!=0)
                {
                    if (itemtimes[i] >0)
                    {
                        itemtimes[i] -= Time.deltaTime;
                    }
                    else if(itemtimes[i] < 0)
                    {
                        itemtimes[i] = 0;

                        bool correct=false;
                        HerbData herbData = ItemDatas.items[itemslots[i].itemNumder] as HerbData;
                        foreach (int j in herbData.itemProcessedWay)
                        {
                            if(j == (i / 3))
                            {
                                correct = true;
                                break;
                            }
                        }
                        if (correct)
                        {
                            Slots[i].transform.parent.GetComponent<DropSlot>().Item.ProcessWay = (i / 3);
                            slotProcessIcon[i].sprite = GameManager.Instance.ProcessIcon[i/3];
                        }
                        else
                        {
                            Slots[i].transform.parent.GetComponent<DropSlot>().Item.itemNumder = 7;
                            Slots[i].sprite = ItemDatas.items[7].itemImage;
                            slotProcessIcon[i].gameObject.SetActive(false);
                        }

                        slotProcessIcon[i].fillAmount = 1;
                        continue;
                    }
                    slotProcessIcon[i].fillAmount = (itemtimes[i] / (10 - ProcessLevel[i / 3]));
                }
            }
        }
    }

    public void UpdateHerb()
    {

        for (int i = 0; i < 9; i++)
        {
            itemslots[i] = new ItemClass();
            if (ProcessLevel[i % 3] >= i % 3) // 해당 칸이 해금되어있는가
            {
                itemslots[i] = Slots[i].transform.parent.GetComponent<DropSlot>().Item;
                Slots[i].gameObject.SetActive(true);
                if (itemslots[i].itemNumder != -1)
                {
                    Slots[i].sprite = ItemDatas.items[itemslots[i].itemNumder].itemImage;
                    slotProcessIcon[i].gameObject.SetActive(true);
                    slotProcessIcon[i].sprite = ProcessTypeIcon;
                    if (itemtimes[i] == -1)
                    {
                        itemtimes[i] = 10 - ProcessLevel[i / 3];
                    }
                    else if (itemtimes[i] == 0)
                    {
                        //가공완료
                    }
                }
                else
                {
                    Slots[i].gameObject.SetActive(false);
                }
            }
            else
            {
                Slots[i].sprite = GameManager.Instance.NeedLevel;
                slotProcessIcon[i].gameObject.SetActive(false);
                continue;
            }
        }
    }
}

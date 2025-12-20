using Unity.VisualScripting;
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

    public float ProcessTime;
    void Start()
    {
        ItemDatas = GameManager.Instance.itemDatas;

        for (int i = 0; i < 9; i++)
        {
            itemtimes[i] = -1f;
            Slots[i].transform.parent.GetComponent<DropSlot>().Load += UpdateHerb;
            slotProcessIcon[i] = Slots[i].transform.parent.GetChild(1).GetComponent<Image>();
        }
        UpdateHerb();
    }

    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (ProcessLevel[i / 3] >= i % 3 && itemtimes[i] != 1) // 해당 칸이 해금되어있는가 0 0 0 >= 0 1 2
            {
                if (itemslots[i].itemNumber != -1)
                {
                    if (itemtimes[i] >0)
                    {
                        itemtimes[i] -= Time.deltaTime;
                    }
                    else if(itemtimes[i] <= 0)
                    {
                        Done(i);
                        continue;
                    }
                    slotProcessIcon[i].fillAmount = (itemtimes[i] / (ProcessTime - ProcessLevel[i / 3]));
                }
            }
        }
    }

    public void UpdateHerb()
    {
        for (int i = 0; i < 9; i++)
        {
            itemslots[i] = new ItemClass();
            if (ProcessLevel[i / 3] >= i % 3) // 해당 칸이 해금되어있는가
            {
                itemslots[i] = Slots[i].transform.parent.GetComponent<DropSlot>().Item;
                Slots[i].gameObject.SetActive(true);
                if (itemslots[i].itemNumber != -1)
                {
                    Slots[i].sprite = ItemDatas.items[itemslots[i].itemNumber].itemImage;
                    if(itemslots[i].ProcessWay == -1)
                    {
                        slotProcessIcon[i].gameObject.SetActive(true);
                        slotProcessIcon[i].sprite = ProcessTypeIcon;
                        if (itemtimes[i] == -1)
                        {
                            itemtimes[i] = ProcessTime - ProcessLevel[i / 3];

                            // 가공
                            Debug.Log("---가공---" + itemslots[i].itemNumber.ToString());
                            GameManager.Instance.dicManager.SettingHerbProcessedCount(itemslots[i]); // 약초 가공 횟수
                        }
                    }
                }
                else
                {
                 
                    Slots[i].gameObject.SetActive(false);
                }
              
            }
            else
            {
                
                Slots[i].sprite = GameManager.Instance.inventoryManager.NeedLevel;
                slotProcessIcon[i].gameObject.SetActive(false);
                continue;
            }
        }
    }

    public void SetProcessTime(float Time, bool onetype, int type)
    {
        for(int i=0;i<itemtimes.Length; i++)
        {            
            if (itemtimes[i] != -1)
            {
                if (onetype)
                {
                    
                    if (i/ 3 == type)
                    {

                        itemtimes[i] -= Time;
                        if (itemtimes[i] <= 0)
                        {

                            Done(i);
                        }
                    }
                }
                else
                {
                    itemtimes[i] -= Time;
                    if (itemtimes[i] <= 0)
                    {
                        Done(i);
                    }
                }
            }
        }
    }

    public void Done(int number) // 가공 완료
    {
        itemtimes[number] = -1;
        bool correct = false;
        HerbData herbData = ItemDatas.items[itemslots[number].itemNumber] as HerbData;
        foreach (int j in herbData.itemProcessedWay)
        {
            if (j == (number / 3))
            {
                correct = true;
                break;
            }
        }
        DropSlot nowitem = Slots[number].transform.parent.GetComponent<DropSlot>();
        ItemClass newitem = new ItemClass();
        if (correct)
        {
            newitem.itemNumber = nowitem.Item.itemNumber;
            newitem.itemType = nowitem.Item.itemType;
            newitem.ProcessWay = (number / 3);
            slotProcessIcon[number].sprite = GameManager.Instance.inventoryManager.ProcessIcon[number / 3];
        }
        else
        {
            newitem.itemNumber = 11;
            newitem.itemType = ItemType.Herb;
            newitem.ProcessWay = 3;
            Slots[number].sprite = ItemDatas.items[11].itemImage;
            slotProcessIcon[number].gameObject.SetActive(false);
        }

        newitem.ItemCount = 1;
        nowitem.Get = true;
        nowitem.Item = newitem;
        slotProcessIcon[number].fillAmount = 1;

    }
}

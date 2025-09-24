using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ProductionSlots
{
    public List<string> herbSlot; // ���߿��� ������ ������ �߰��ؾ� ��
    public string herbSlotContect
    {
        set
        {
            if (herbSlot.Count < 2)
            {
                herbSlot.Add(value);
            }
            else
            {
                UnityEngine.Debug.LogWarning("������ �� á���ϴ�!");
            }
        }

    }
    public string ResultSlot;
}
public class ProductionTable : MonoBehaviour
{
    public List<Button> HerbButtons;
    public List<Herb> herbList;

    public ProductionSlots Slots;
    public int slotCount;
    private void Start()
    {

        foreach (var button in HerbButtons) // ��ư�� Ŭ���� �� ������ �ż��� ����
        {
            Herb herb = button.GetComponent<Herb>();
            herbList.Add(herb);
            button.onClick.AddListener(() => SetTheHerb(herb.HerbName));
        }
    }

    public void SetTheHerb(string HerbName)
    {
        Debug.Log(HerbName + " ��ġ��");
        Slots.herbSlotContect = HerbName;
    }

    public void CombineHerb()
    {
        if (Slots.herbSlot.Count < slotCount) // ��� ������ �� ä������ ���� ���
        {
            Debug.Log("���ʰ� ���µ��???");
            return;
        }


        Debug.Log(Slots.herbSlot[0] + ", " + Slots.herbSlot[1] + " �����Ϸ�");
        Slots.ResultSlot = Slots.herbSlot[0] + Slots.herbSlot[1];
    }



}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ProductionSlots
{
    public List<string> herbSlot; // 나중에는 약초의 비율도 추가해야 함
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
                UnityEngine.Debug.LogWarning("슬롯이 꽉 찼습니다!");
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

        foreach (var button in HerbButtons) // 버튼을 클릭할 때 실행한 매서드 저장
        {
            Herb herb = button.GetComponent<Herb>();
            herbList.Add(herb);
            button.onClick.AddListener(() => SetTheHerb(herb.HerbName));
        }
    }

    public void SetTheHerb(string HerbName)
    {
        Debug.Log(HerbName + " 배치됨");
        Slots.herbSlotContect = HerbName;
    }

    public void CombineHerb()
    {
        if (Slots.herbSlot.Count < slotCount) // 허브 슬롯이 다 채워지지 않은 경우
        {
            Debug.Log("약초가 없는디요???");
            return;
        }


        Debug.Log(Slots.herbSlot[0] + ", " + Slots.herbSlot[1] + " 제조완료");
        Slots.ResultSlot = Slots.herbSlot[0] + Slots.herbSlot[1];
    }



}

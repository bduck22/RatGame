using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ISetTheItems : MonoBehaviour
{

    public virtual void SetTheItem(GameObject setObject, GameObject getObject)
    {

    }

}

[Serializable]
public class PotionsCreateUI
{
    public Image SlotImage;

    public void UpdateSlotImage(Sprite itemSprite)
    {
        SlotImage.sprite = itemSprite;
    }
}

public class PotionsCreate : ISetTheItems
{
    [Header("약물 제작법")]
    public PotionData[] PotionDatas;
    public PotionData faultPotionData;
    public PotionsCreateUI[] potionsCreateUI = new PotionsCreateUI[3];

    public ItemClass[] HerbSlot = new ItemClass[3];   // 약초 슬롯
    public int[] HerbSlotAnount = new int[2];       // 허브 비율
    //public ItemClass resultPotion;

    public Slider HerbAmountSlider;
    public CreateItems CreateItem;
    public Transform SlotPos;


    PotionData ListPotions; // 조건들을 확인한 후 가장 높은 등급의 아이템만 생성
    int Listlevel;


    private void Start()
    {
        HerbAmountSlider = GetComponentInChildren<Slider>();
    }



    public override void SetTheItem(GameObject setItem, GameObject getObject)
    {
        for (int i = 0; i < potionsCreateUI.Length; i++)
        {
            if (setItem == potionsCreateUI[i].SlotImage.gameObject)
            {
                Debug.Log("아이템 설정");
                //HerbSlot[i] = setItem.GetComponent<ItemClass>();
                potionsCreateUI[i].UpdateSlotImage(getObject.GetComponent<Image>().sprite);
                break;
            }
        }
    }










    //public void CreatePotion() // 물약 생성 함수
    //{
    //    if (HerbSlot[0].inventorySlot.item == null || HerbSlot[1].inventorySlot.item == null) return; // 아이템이 두곳중 한곳이라도 비어 있으면 

    //    HerbData herb1 = HerbSlot[0].inventorySlot.item as HerbData;
    //    HerbData herb2 = HerbSlot[1].inventorySlot.item as HerbData;

    //    if (herb1 == null || herb2 == null) return; // 약초 아이템이 아닐 경우 제작 불가
    //    if (!herb1.IsProcessed || !herb2.IsProcessed) return; // 가공되지 않은 약초일 경우 제작 불가

    //    HerbSlotAnount[0] = (int)((HerbAmountSlider.value + 0.05f) * 10f);            // 1번 슬롯 비율
    //    HerbSlotAnount[1] = 10 - (int)((HerbAmountSlider.value + 0.05f) * 10f);       // 2번 슬롯 비율




    //    for (int i = 0; i < PotionDatas.Length; i++)
    //    {
    //        var herbA = HerbSlot[0].inventorySlot.item;
    //        var herbB = HerbSlot[1].inventorySlot.item;
    //        var data = PotionDatas[i];

    //        bool isSamePair = // 재료가 매칭된지 확인
    //             (data.Herb1 == herbA && data.Herb2 == herbB) ||
    //             (data.Herb1 == herbB && data.Herb2 == herbA);



    //        // 모든 재료가 맞지 않을 때
    //        if (!isSamePair)
    //        {

    //            // 한재료만 맞을때
    //            if ((data.Herb1 == herbA || data.Herb2 == herbB) || (data.Herb1 == herbB || data.Herb2 == herbA))
    //            {

    //                PotionLevel(1, data);
    //            }
    //            // 둘 다 틀릴 때
    //            else
    //            {
    //                PotionLevel(1, faultPotionData); // 실패한 물약
    //            }
    //            continue;

    //        }

    //        int slotHerb1Ratio, slotHerb2Ratio;

    //        // 순서가 반대일 경우 비율도 반대로 설정
    //        if (data.Herb1 == herbA && data.Herb2 == herbB)
    //        {
    //            slotHerb1Ratio = HerbSlotAnount[0];
    //            slotHerb2Ratio = HerbSlotAnount[1];
    //        }
    //        else
    //        {
    //            // 순서 반대
    //            slotHerb1Ratio = HerbSlotAnount[1];
    //            slotHerb2Ratio = HerbSlotAnount[0];
    //        }

    //        bool herb1Match = Mathf.Abs(data.HerbAmount1 - slotHerb1Ratio) <= 2;
    //        bool herb2Match = Mathf.Abs(data.HerbAmount2 - slotHerb2Ratio) <= 2;
    //        bool isSameHerb = herbA == herbB;

    //        if (isSameHerb) // 동일한 재료일 때
    //        {
    //            if (herb1Match || herb2Match)
    //                PotionLevel(3, data);
    //            else
    //                PotionLevel(2, data);
    //        }
    //        else
    //        {
    //            if (herb1Match && herb2Match)
    //                PotionLevel(3, data);
    //            else if (herb1Match || herb2Match)
    //                PotionLevel(2, data);
    //            else
    //                PotionLevel(1, data);
    //        }

    //    }

    //    GameObject item = CreateItem.CreateItem(ListPotions, ResultSlot.transform);
    //    PotionData potion = item.GetComponent<DragItem>().itemData as PotionData;

    //    // 약물의 완성도 설정
    //    SetPotionCompletness(potion);


    //    HerbSlot[0].draggedItem.gameObject.SetActive(false);
    //    HerbSlot[0].ReSet();
    //    HerbSlot[1].draggedItem.gameObject.SetActive(false);
    //    HerbSlot[1].ReSet();

    //    Listlevel = 0;
    //    ListPotions = null;

    //}

    //public void PotionLevel(int level, PotionData potion)
    //{

    //    if (Listlevel < level)      // 약물 등급이 더 높은 아이템만 취급
    //    {
    //        Listlevel = level;
    //        ListPotions = potion;
    //    }




    //}

    //public float SetPotionCompletness(PotionData potion)
    //{
    //    // 현재 약초 
    //    var herbA = HerbSlot[0].inventorySlot.item;
    //    var herbB = HerbSlot[1].inventorySlot.item;

    //    int amount1 = HerbSlotAnount[0];
    //    int amount2 = HerbSlotAnount[1];

    //    int data1 = potion.HerbAmount1;
    //    int data2 = potion.HerbAmount2;


    //    bool SameHerbs = (potion.Herb1 == herbA && potion.Herb2 == herbB) || (potion.Herb1 == herbB && potion.Herb2 == herbA);// 두재료가 일치할 때
    //    bool SameHerbAmount = potion.Herb1 == herbA && potion.Herb2 == herbB ? (amount1 == data1 && amount2 == data2) : (amount1 == data2 && amount2 == data1); // 두 재료의 비율이 일치할 때
    //    bool SameHerbSlot1Amount = potion.Herb1 == herbA && potion.Herb2 == herbB ? (amount1 == data1) : (amount1 == data2); // 1번 슬롯 재료의 비율이 일치할 때
    //    bool SameHerbSlot2Amount = potion.Herb1 == herbA && potion.Herb2 == herbB ? (amount2 == data2) : (amount2 == data1); // 2번 슬롯 재료의 비율이 일치할 때
    //    bool Diff5orLess = (potion.Herb1 == herbA && potion.Herb2 == herbB) ? (Mathf.Abs(amount1 - data1) + Mathf.Abs(amount2 - data2) <= 5) : (Mathf.Abs(amount1 - data2) + Mathf.Abs(amount2 - data1) <= 5); // 5이하 차이일 때



    //    Debug.Log($"재료 일치: {SameHerbs}, 비율 일치: {SameHerbAmount}, 1번 슬롯 비율 일치: {SameHerbSlot1Amount}, 2번 슬롯 비율 일치: {SameHerbSlot2Amount}, 5이하: {Diff5orLess}");


    //    if (potion.itemLevel == 1) // 일반 상황일 때
    //    {
    //        Debug.Log("일반 완성도");
    //        potion.Completeness = 50 + (SameHerbs ? 15 : 0) + (SameHerbSlot1Amount || SameHerbSlot2Amount ? 15 : 0) + (SameHerbAmount ? 20 : 0);



    //    }
    //    else if (potion.itemLevel == 2) // 희귀 상황일 때
    //    {
    //        Debug.Log("희귀 완성도");
    //        potion.Completeness = 50 + (Diff5orLess ? 10 : 0) + (SameHerbSlot1Amount ? 20 : 0) + (SameHerbSlot2Amount ? 20 : 0);

    //    }
    //    else if (potion.itemLevel == 3) // 전설 상황일 때
    //    {
    //        Debug.Log("전설 완성도");
    //        potion.Completeness = 50 + (SameHerbSlot1Amount ? 10 : 0) + (SameHerbSlot2Amount ? 20 : 0) + (SameHerbAmount ? 20 : 0);

    //    }
    //    return 0;


    //}
}

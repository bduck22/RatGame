//using System;
//using UnityEngine;


//[Serializable]
//public class ItemProcessed
//{
//    public DropInventory processedSlot;   // 실험 슬롯
//    public DropInventory resulSlot;         // 결과 슬롯
//    public int possibility;


//}
//public class ProcessedManager : MonoBehaviour
//{
//    public HerbData[] itemDatas; // 가공된 아이템 데이터
//    public HerbData failedItem; // 가공 실패한 아이템
//    public CreateItems CreateItems; // 아이템 제작 메서드
//    public ItemProcessed[] itemProcessed;
 



//    public void ProcessedItem(int proceddedNum) // 아이템 가공 매서드
//    {
//        HerbData item;
//        HerbData chickItem;

//        item = itemProcessed[proceddedNum - 1].processedSlot.inventorySlot.item as HerbData;
//        if (item != null && !item.IsProcessed && itemProcessed[proceddedNum - 1].resulSlot.inventorySlot.item == null) // 아이템이 존재한다면 또한 아이템이 가공되지 않았다면, 또한 결과 창이 비어 있으면
//        {
//            int count = 0;
//            for (int i = 0; i < 3; i++)
//            {

//                if (itemProcessed[proceddedNum - 1].possibility == item.itemProcessedWay[i]) // 자신의 가공방식이랑 맞을 경우
//                {
//                    chickItem = item;
                    
//                    Debug.Log("아이템 가공");
//                    ProcessedListChick(chickItem ,item.itemProcessedWay[i], itemProcessed[proceddedNum - 1], false);
//                    break;
//                }
//                count++;
//            }


//            if (count >= 3) // 그래도 찾지 못했으면
//            {
                
//                ProcessedListChick(failedItem, 0, itemProcessed[proceddedNum - 1], true);
//                Debug.Log("쓰래기");
              
//            }


          

//        }
//        else
//        {
//            Debug.Log("아이템을 가공할 수 없습니다");
//        }
//    }

//    public void ProcessedListChick(HerbData item,int processedNum, ItemProcessed process, bool isfailed) // 가공 리스트 확인
//    {

//        process.processedSlot.draggedItem.gameObject.SetActive(false); // 가공 슬롯 정리
//        process.processedSlot.ReSet();

//        if (!isfailed) // 제작에 성공 했을 때
//        {
//            for (int i = 0; i < itemDatas.Length; i++)
//            {
//                if (item.itemName == itemDatas[i].itemName && itemDatas[i].itemProcessedWay[0] == processedNum) // 서로 이름과 아이템 가공 가능한 형식이 같아야 한다
//                {
//                    process.resulSlot.inventorySlot.item = itemDatas[i];
//                    process.resulSlot.inventorySlot.count += 1;
//                    CreateItems.CreateItem(itemDatas[i], process.resulSlot.transform);

//                    break;
//                }
//            }
//        }
//        else // 실패 했을 때
//        {
//            process.resulSlot.inventorySlot.item = failedItem;
//            process.resulSlot.inventorySlot.count += 1;
//            CreateItems.CreateItem(failedItem, process.resulSlot.transform); // 제조 실패 아이템 생성
//        }

      
//    }



//}

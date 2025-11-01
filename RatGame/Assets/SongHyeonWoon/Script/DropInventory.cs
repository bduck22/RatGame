using UnityEngine;
using UnityEngine.EventSystems;

public class DropInventory : DropSlot
{
    public ItemSlot inventorySlot;

    private void Start()
    {
        inventorySlot = GetComponent<ItemSlot>();
    }

    public override void OnDrop(PointerEventData eventData) // 드래그가 끝났을 때, 이 UI 오브젝트 위에 마우스가 있으면 자동으로 OnDrop() 이벤트를 실행
    {
        if (eventData.pointerDrag != null)
        {
            draggedItem = eventData.pointerDrag.GetComponent<DragItem>();
            if (draggedItem.itemData == inventorySlot.item || inventorySlot.item == null) // 아이템 칸이 비어있거나 다른 아이템이 차있으면
            {

                if (draggedItem == null) return;

                originSlot = draggedItem.OriginalParent.GetComponentInParent<ItemSlot>();
                targetSlot = transform;



                if (inventorySlot.MaxCount > inventorySlot.count) // 아이템이 최대 저장 공간을 넘지 못하도록
                {
                    if (originSlot != targetSlot)
                    {
                        ItemExit();
                    }

                    ItemAdd();
                }
                else
                {
                    Debug.Log("저장 공간이 가득 한디예");
                }
            }

        }
    }


    public override void ItemAdd()
    {
       
            if (targetSlot.childCount > 0)
            {
                draggedItem.transform.SetParent(targetSlot.GetChild(0));
            }
            else
            {
                draggedItem.transform.SetParent(targetSlot);
            }
            draggedItem.retPos.localPosition = Vector3.zero;
            inventorySlot.AddItem(draggedItem.itemData);

            

        

    }

    public override void ItemExit()
    {

        if (originSlot != null)
        {
            
            if (originSlot != null)
            {
                originSlot.RemoveItem();
              
            }
        }


        base.ItemExit();
     
    }

    public  void ReSet()
    {
        inventorySlot.count = 0;
        inventorySlot.item = null;
        
    }
}

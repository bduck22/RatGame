using UnityEngine;
using UnityEngine.EventSystems;

public class DropInventory : DropSlot
{
    public InventoryManager invMan;
    public ItemSlot inventorySlot;

    private void Start()
    {
        inventorySlot = GetComponent<ItemSlot>();
    }

    public override void OnDrop(PointerEventData eventData) // �巡�װ� ������ ��, �� UI ������Ʈ ���� ���콺�� ������ �ڵ����� OnDrop() �̺�Ʈ�� ����
    {
        if (eventData.pointerDrag != null)
        {
            draggedItem = eventData.pointerDrag.GetComponent<DragItem>();
            if (draggedItem.itemData == inventorySlot.item || inventorySlot.item == null) // ������ ĭ�� ����ְų� �ٸ� �������� ��������
            {

                if (draggedItem == null) return;

                originSlot = draggedItem.OriginalParent.GetComponentInParent<ItemSlot>();
                targetSlot = transform;




                if (originSlot != targetSlot)
                {
                    ItemExit();
                }

                ItemAdd();
            }

        }
    }


    public override void ItemAdd()
    {
        base.ItemAdd();
      
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
}

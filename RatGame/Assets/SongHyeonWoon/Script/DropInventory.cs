using UnityEngine;
using UnityEngine.EventSystems;

public class DropInventory : DropSlot
{
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



                if (inventorySlot.MaxCount > inventorySlot.count) // �������� �ִ� ���� ������ ���� ���ϵ���
                {
                    if (originSlot != targetSlot)
                    {
                        ItemExit();
                    }

                    ItemAdd();
                }
                else
                {
                    Debug.Log("���� ������ ���� �ѵ�");
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

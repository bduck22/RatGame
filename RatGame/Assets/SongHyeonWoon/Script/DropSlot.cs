
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{

    public ItemSlot originSlot;
    public Transform targetSlot;
    public DragItem draggedItem;

    public virtual void OnDrop(PointerEventData eventData) // �巡�װ� ������ ��, �� UI ������Ʈ ���� ���콺�� ������ �ڵ����� OnDrop() �̺�Ʈ�� ����
    {
        if (eventData.pointerDrag != null)
        {
            draggedItem = eventData.pointerDrag.GetComponent<DragItem>();

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

    public virtual void ItemExit() // �������� ���� ���� ��
    {
    
    }

    public virtual void ItemAdd() // ������ �߰� �� ��
    {

      
    }

    
}

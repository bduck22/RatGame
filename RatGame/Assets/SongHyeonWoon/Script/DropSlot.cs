
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{

    public ItemSlot originSlot;
    public Transform targetSlot;
    public DragItem draggedItem;

    public virtual void OnDrop(PointerEventData eventData) // 드래그가 끝났을 때, 이 UI 오브젝트 위에 마우스가 있으면 자동으로 OnDrop() 이벤트를 실행
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

    public virtual void ItemExit() // 아이템이 빠져 나갈 때
    {
    
    }

    public virtual void ItemAdd() // 아이템 추가 할 때
    {

      
    }

    
}

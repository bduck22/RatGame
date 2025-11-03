using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    public Button[] buttons;

    public Transform[] tabs;

    public PotionData selectpotion;
    public Transform potioninfo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void movetab(int number)
    {
        if (tabs[number].gameObject.activeSelf)
        {
            return;
        }
        else
        {
            potioninfo.gameObject.SetActive(false);
        }

            for (int i = 0; i < 3; i++)
            {
                buttons[i].interactable = true;
                tabs[i].gameObject.SetActive(false);
                buttons[i].GetComponent<Image>().color = Color.white;
            }

        buttons[number].interactable=false;
        tabs[number].gameObject.SetActive(true);
        buttons[number].GetComponent<Image>().color = Color.gray;
    }

    public void openinfo()
    {
        potioninfo.gameObject.SetActive(true);
    }
}

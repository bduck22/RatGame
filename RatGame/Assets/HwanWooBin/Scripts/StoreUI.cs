using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    public Button[] buttons;

    public Transform[] tabs;

    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void movetab(int number)
    {
        for(int i = 0; i < 3; i++)
        {
            buttons[i].interactable = true;
            tabs[i].gameObject.SetActive(false);
            buttons[i].GetComponent<Image>().color = Color.white;
        }

        buttons[number].interactable=false;
        tabs[number].gameObject.SetActive(true);
        buttons[number].GetComponent<Image>().color = Color.gray;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Animator Fade;

    public Animator[] UIs;
    public TextMeshProUGUI Day;

    void Start()
    {
        Fade = GetComponent<Animator>();
        //roomName = UIs[0].GetComponent<TextMeshProUGUI>();
    }


    public void CloseUi()
    {
        foreach ( Animator ani in UIs)
        {
            ani.SetTrigger("Close");
        }
    }

    public void OnUi()
    {
        foreach ( Animator ani in UIs)
        {
            ani.SetTrigger("Open");
        }
    }

    public void Refreshroom()
    {
        GameManager.Instance.RefreshBackground();
    }

    public void OnFade()
    {
        if (!GameManager.Instance.stopping)
        {
            Fade.SetTrigger("Play");
        }
    }

    public void UpdateDayText()
    {
        Day.text = (GameManager.Instance.Day).ToString()+"ÀÏÂ÷";
    }
}
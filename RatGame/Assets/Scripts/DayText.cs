using TMPro;
using UnityEngine;

public class DayText : MonoBehaviour
{
    public bool tomorrow;

    TextMeshProUGUI daytext;
    private void Awake()
    {
        daytext = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        if (tomorrow)
        {
            daytext.text = (GameManager.Instance.Day).ToString();
        }
        else
        {
            daytext.text = (GameManager.Instance.Day-1).ToString();
        }
    }
}

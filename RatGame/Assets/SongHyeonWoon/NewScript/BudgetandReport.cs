using UnityEngine;

public class BudgetandReport : MonoBehaviour
{
    public GameManager gameManager;
    public DicManager dicManager;
    public int Day;

    [Header("¼º°ú")]
    public float Successed;
    private void Start()
    {
        Day = gameManager.Day;
        gameManager.StartDay();
    }


}

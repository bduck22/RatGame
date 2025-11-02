using UnityEngine;

public class animeFunc : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void stoppingfalse()
    {
        GameManager.Instance.falsestop();
    }

    public void screenon()
    {
        GameManager.Instance.RealOnScreen();
    }
}

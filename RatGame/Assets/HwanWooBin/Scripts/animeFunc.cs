using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Dayplaying()
    {
        
        GameManager.Instance.playingday();
    }
    public void Dayplaying_Run()
    {

        GameManager.Instance.NextDaying();
    }
    public void EndDay()
    {
        GameManager.Instance.NextDayEnd();
    }

    public void GoGame()
    {
        SceneManager.LoadScene(1);
    }
}

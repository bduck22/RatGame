using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunces : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ToMainScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void ToQuit()
    {
        Application.Quit();
    }
}

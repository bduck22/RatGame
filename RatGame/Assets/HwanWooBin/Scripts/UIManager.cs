using UnityEngine;

public class UIManager : MonoBehaviour
{
    Animator Fade;

    public Animator[] UIs;

    public Transform touchVfx;

    void Start()
    {
        Fade = GetComponent<Animator>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
        {
            touchVfx.position = Input.mousePosition;
            touchVfx.GetComponent<Animator>().SetTrigger("Play");
            //touchVfx.GetComponent<ParticleSystem>().Play();
        }
#endif
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                touchVfx.position = touch.position;
                touchVfx.GetComponent<Animator>().SetTrigger("Play");
                //touchVfx.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    public void CloseUi()
    {

        foreach ( Animator ani in UIs)
        {
            ani.SetTrigger("Play");
        }
    }

    public void OnUi()
    {
        foreach ( Animator ani in UIs)
        {
            ani.SetTrigger("Back");
        }
    }

    public void OnFade()
    {
        Fade.SetTrigger("Play");
    }
}
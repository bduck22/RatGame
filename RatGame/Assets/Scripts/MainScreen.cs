using UnityEngine;

public class MainScreen : MonoBehaviour
{
    public Animator fade;

    public bool move = false;
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0)&&!move)
        {
            fade.Play("In");
            move = true;
        }
#endif
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended&&!move)
            {
                fade.Play("In");
                move = true;
            }
        }


    }
}

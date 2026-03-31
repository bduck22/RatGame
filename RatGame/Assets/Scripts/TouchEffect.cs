using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = Input.mousePosition;
            animator.SetTrigger("Play");
            //touchVfx.GetComponent<ParticleSystem>().Play();
        }
#endif
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                transform.position = touch.position;
                animator.SetTrigger("Play");
                //touchVfx.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}

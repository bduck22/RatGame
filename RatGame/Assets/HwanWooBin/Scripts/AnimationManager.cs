using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    public Animator[] Animators;
    public bool IsDoubleSpeed;

    private void Start()
    {
        instance = this;

        SetAniamtors();
    }

    public void SetAniamtors()
    {
        foreach(Animator animator in Animators)
        {
                animator.SetFloat("Speed", (IsDoubleSpeed?2:1));
        }
    }
}

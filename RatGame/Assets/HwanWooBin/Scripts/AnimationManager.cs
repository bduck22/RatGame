using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    public Animator[] Animators;
    public bool IsDoubleSpeed;

    public Image Check;

    private void Start()
    {
        instance = this;

        Animators = FindObjectsOfType<Animator>();

        SetAniamtors();
    }

    public void SetAniamtors()
    {
        foreach(Animator animator in Animators)
        {
            if(animator!=null) animator.SetFloat("Speed", (IsDoubleSpeed?2:1));
        }
    }

    public void OnOffDoubleSpeed()
    {
        IsDoubleSpeed = !IsDoubleSpeed;
        Check.gameObject.SetActive(IsDoubleSpeed);
        SetAniamtors();
    }
}

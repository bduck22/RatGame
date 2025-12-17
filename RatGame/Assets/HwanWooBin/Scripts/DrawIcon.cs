using System.Collections;
using UnityEngine;

public class TestDraw : MonoBehaviour
{
    [SerializeField] private float Progress;
    [SerializeField] private float fillpower;
    [SerializeField] private float startvalue;
    [SerializeField] private float endvalue;

    public Material SepiaFilter;
    void Start()
    {

    }

    void Update()
    {

    }

    [ContextMenu("OnIcon")]
    public void IconAnimationStart()
    {
        StartCoroutine(OnIcon());
    }

    private IEnumerator OnIcon()
    {
        Progress = startvalue;
        while (Progress < endvalue)
        {
            Progress += fillpower;
            SepiaFilter.SetFloat("_Progress", Progress);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
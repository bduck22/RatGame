using UnityEngine;

public class CanvasFix : MonoBehaviour
{
    private void Start()
    {
        
        //UpdateCamera();
    }

    private void Update()
    {
        Debug.Log(Screen.width);
    }

    public Transform Background;

    void UpdateCamera()
    {
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = Camera.main;

        if (scaleHeight < 1.0f)
            cam.orthographicSize = cam.orthographicSize / scaleHeight;
    }
}

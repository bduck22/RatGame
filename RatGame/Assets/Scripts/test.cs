using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    bool s=false;

    // Update is called once per frame
    void Update()
    {
        if (s)
        {

#if UNITY_EDITOR
            transform.position = Input.mousePosition;
#endif
            transform.position = Input.GetTouch(0).position;
        }
    }

    public void move()
    {
        s = true;
    }
    public void end()
    {
        s = false;
    }
}

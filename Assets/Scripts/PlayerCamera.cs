using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera myCamera;
    [SerializeField] private bool rotate = false;

    public void SetConfig(int screens, int index)
    {
        float x = 0, y = 0, width = 1, height = 1;
        if (screens == 2)
        {
            x = index / 2f;
            y = 0;
            width = 0.5f;
            height = 1;
        }
        else if (screens == 3)
        {
            x = index / 3f;
            y = 0;
            width = 1 / 3f;
            height = 1;
        }
        else if (screens == 4)
        {
            x = (index % 2) / 2f;
            y = index < 2 ? 0 : 0.5f;
            width = 0.5f;
            height = 0.5f;
        }
        Debug.Log($"{x} {y} {width} {height}");
        myCamera.rect = new Rect(x, y, width, height);
    }

    private void Update()
    {
        if (rotate)
            myCamera.transform.rotation = transform.rotation;
        else
            myCamera.transform.rotation = Quaternion.identity;

    }
}

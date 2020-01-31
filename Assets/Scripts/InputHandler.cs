using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandler
{
    public static float HorizontalMovement => Input.GetAxis("Horizontal");

    public static float VerticalMovement => Input.GetAxis("Vertical");

    public static bool IsInteracting
    {
        get
        {
            return Input.GetAxis("Interact") > 0.05;
        }
    }
}

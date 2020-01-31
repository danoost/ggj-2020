using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputHandler
{
    public static Vector3 MovementDirection
    {
        get
        {
            Vector3 result = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );

            if (result.sqrMagnitude > 0.05)
            {
                return result.normalized;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    public static bool IsInteracting
    {
        get
        {
            return Input.GetAxis("Interact") > 0.05;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private DynamicMaterialColor[] thingsToColor;
    private Color myColor;

    public void SetColor(Color newColor)
    {
        myColor = newColor;
        Debug.Log(myColor);
        foreach (DynamicMaterialColor thing in thingsToColor)
            thing.SetColor(myColor);
    }
}

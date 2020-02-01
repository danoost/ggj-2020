using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private DynamicMaterialColor[] thingsToColor;
    private Color myColor;

    protected void Start()
    {
        myColor = DynamicMaterialColor.CreateRandomHue();
        foreach (DynamicMaterialColor thing in thingsToColor)
            thing.SetColor(myColor);
    }
}

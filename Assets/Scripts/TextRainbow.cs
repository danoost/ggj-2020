using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextRainbow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private float currentHue;

    private void Start()
    {
        currentHue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Color color = Color.HSVToRGB(currentHue, 1, 1);
        currentHue = (currentHue + Time.deltaTime) % 1f;
        text.color = color;
    }
}

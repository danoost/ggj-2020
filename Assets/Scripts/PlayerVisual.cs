using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField]
    MeshRenderer mr;
    [SerializeField] MeshRenderer[] boosterMrs;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Damageable myDamage;

    private int currentHealth = 0;

    public Color Color { get; private set; }
    public void SetPlayerIndex(int index)
    {
        mr.material = PlayerJoin.instance.playerMats[index];
        Color = PlayerJoin.instance.playerColors[index];
        text.color = Color;
        foreach (MeshRenderer bmr in boosterMrs)
        {
            bmr.material = PlayerJoin.instance.playerBoosterMats[index];
        }
    }

    private void Update()
    {
        if (myDamage.currentHealth != currentHealth)
        {
            currentHealth = myDamage.currentHealth;
            text.SetText("");
            for (int i = 0; i < currentHealth; i++)
            {
                text.SetText(text.text + $"<sprite=\"heart\" color=\"#{ColorUtility.ToHtmlStringRGB(Color)}\" index=0>");
            }
        }
    }
}

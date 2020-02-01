using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Color Color { get; private set; }
    public float Index { get; private set; }

    void Start()
    {
        GameFlowManager.instance.RegisterPlayer(this);
    }

    private void OnDestroy()
    {
        GameFlowManager.instance.DeadPlayer(this);
    }

    public void SetStats(Color color, int playerIndex)
    {
        Color = color;
        Index = playerIndex;
    }
}

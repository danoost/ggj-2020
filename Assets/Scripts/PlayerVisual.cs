using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField]
    MeshRenderer mr;

    public Color Color { get; private set; }

    public void SetPlayerIndex(int index)
    {
        mr.material = PlayerJoin.instance.playerMats[index];
        Color = PlayerJoin.instance.playerColors[index];
    }
}

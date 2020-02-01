using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField]
    Material[] mats;

    [SerializeField]
    MeshRenderer mr;

    public void SetPlayerIndex(int index)
    {
        mr.material = mats[index];
    }
}

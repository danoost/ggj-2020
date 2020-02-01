using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RandomMaterial : MonoBehaviour
{
    [SerializeField]
    private Material[] mats;

    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        meshRenderer.material = mats[Random.Range(0, mats.Length)];
    }
}

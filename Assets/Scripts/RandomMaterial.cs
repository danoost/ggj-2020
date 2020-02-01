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

        if (mats.Length != 0)
        {
            meshRenderer.material = mats[Random.Range(0, mats.Length)];
        }
    }

    public void SetMaterial(int index)
    {
        meshRenderer.material = mats[index];
    }
}

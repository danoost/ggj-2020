using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DynamicMaterialColor : MonoBehaviour
{
    [SerializeField] private bool randomiseHue = true;

    private Material mat;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mat = new Material(meshRenderer.material);
        meshRenderer.material = mat;
        if (randomiseHue) RandomiseHue();
    }

    private void RandomiseHue()
    {
        mat.SetColor("_Color", Random.ColorHSV(0, 1, 1, 1, 1, 1));
    }
}

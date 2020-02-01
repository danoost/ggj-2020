using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    [SerializeField] private float minScaleFactor = 0.9f;
    [SerializeField] private float sf = 1;
    private float maxScaleFactor;
    private Vector3 baseScale;
    private float currentScaleFactor = 1;

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        maxScaleFactor = 1 / minScaleFactor;
    }

    // Update is called once per frame
    void Update()
    {
        currentScaleFactor = Mathf.Lerp(minScaleFactor, maxScaleFactor, (Mathf.Sin(sf * Time.realtimeSinceStartup) + 1) / 2);
        transform.localScale = currentScaleFactor * baseScale;
    }
}

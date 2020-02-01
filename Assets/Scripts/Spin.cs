using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float sf = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, sf * Time.deltaTime);
    }
}

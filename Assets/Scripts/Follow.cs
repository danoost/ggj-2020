using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private GameObject followObject;
    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - followObject.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = followObject.transform.position + offset;
    }
}

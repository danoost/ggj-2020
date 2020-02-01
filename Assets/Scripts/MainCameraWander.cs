using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraWander : MonoBehaviour
{
    [SerializeField] float radius = 30;
    [SerializeField] float speed = 2;

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Cos(speed * Time.realtimeSinceStartup) * radius;
        float y = Mathf.Sin(speed * Time.realtimeSinceStartup) * radius;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}

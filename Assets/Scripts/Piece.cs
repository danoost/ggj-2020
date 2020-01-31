﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    protected GameObject root;
    protected Rigidbody2D rootRb;
    protected Rigidbody2D selfRb;
    private Rigidbody2D selfRbBackup;

    [SerializeField]
    protected float baseScale = 0.3f;
    [SerializeField]
    protected float scaleMinFactor = 0.5f;
    [SerializeField]
    protected float scaleMaxFactor = 2f;
    [SerializeField]
    private bool randomiseRotation = true;

    protected void Start()
    {
        selfRb = GetComponent<Rigidbody2D>();

        if (transform.parent != null && transform.parent.CompareTag("CommandCentre"))
        {
            SetRoot(transform.parent.gameObject);
        }

        if (root == null)
        {
            // Scale and rotation randomisation
            transform.localScale = Vector3.one * baseScale * Random.Range(scaleMinFactor, scaleMaxFactor);
            if (randomiseRotation)
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180, 180));
        }
    }

    private GameObject FindRoot(GameObject other)
    {
        return other.GetComponentInParent<Player>().gameObject;
    }

    private void SetRoot(GameObject newRoot)
    {
        root = newRoot;
        rootRb = root.GetComponent<Rigidbody2D>();
        selfRbBackup = new Rigidbody().GetCopyOf(selfRb);
        Destroy(selfRb);
    }

    public void Detach()
    {
        root = null;
        rootRb = null;
        transform.parent = null;
        foreach (Transform child in transform)
        {
            child.GetComponent<Piece>().Detach();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (root == null &&
            (collision.collider.CompareTag("CommandCentre")
            || collision.collider.CompareTag("Piece"))
            )
        {
            SetRoot(FindRoot(collision.collider.gameObject));
            transform.parent = collision.collider.transform;
        }
    }
}

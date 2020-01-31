
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    protected GameObject root;
    protected Rigidbody rootRb;
    protected Rigidbody selfRb;
    private Rigidbody selfRbBackup;

    private void Start()
    {
        selfRb = GetComponent<Rigidbody>();

        if (transform.parent != null && transform.parent.CompareTag("CommandCentre"))
        {
            SetRoot(transform.parent.gameObject);
        }
    }

    private GameObject FindRoot(GameObject other)
    {
        return other.GetComponentInParent<Player>().gameObject;
    }

    private void SetRoot(GameObject newRoot)
    {
        root = newRoot;
        rootRb = root.GetComponent<Rigidbody>();
        selfRbBackup = new Rigidbody().GetCopyOf(selfRb);
        Destroy(selfRb);
        transform.parent = newRoot.transform;
    }

    // TODO Detach

    private void OnCollisionEnter(Collision collision)
    {
        if (root == null && 
            (collision.collider.CompareTag("CommandCentre") 
            || collision.collider.CompareTag("Piece"))
            )
        {
            SetRoot(FindRoot(collision.collider.gameObject));
        }
    }
}

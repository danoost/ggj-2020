
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    protected GameObject root;
    protected Rigidbody2D rootRb;
    protected PlayerController rootController;
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
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            return player.gameObject;
        }
        else
        {
            return null;
        }
    }

    private void SetRoot(GameObject newRoot)
    {
        root = newRoot;
        rootRb = root.GetComponent<Rigidbody2D>();
        rootController = root.GetComponent<PlayerController>();
        Debug.Log(rootController);
        selfRbBackup = new Rigidbody2D().GetCopyOf(selfRb);
        Destroy(selfRb);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void Detach(bool recursive = true)
    {
        if (recursive)
        {
            foreach (Piece child in GetComponentsInChildren<Piece>())
            {
                if (child.TryGetComponent(out Piece piece))
                {
                    piece.Detach(recursive = false);
                }
            }
        }
        root = null;
        rootRb = null;
        rootController = null;
        transform.parent = null;
        gameObject.layer = LayerMask.NameToLayer("Spooky Ghosts");
        selfRb = gameObject.AddComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (root == null &&
            (collision.collider.CompareTag("CommandCentre")
            || collision.collider.CompareTag("Piece"))
            )
        {
            GameObject newRoot = FindRoot(collision.collider.gameObject);
            if (newRoot != null)
            {
                SetRoot(newRoot);
                transform.parent = collision.collider.transform;
            }
        }
    }
}

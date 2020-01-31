using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Exploding : MonoBehaviour
{
    [SerializeField]
    private int damage = 40;

    [SerializeField]
    private float radius = 0.5f;

    [SerializeField]
    private float pushForce = 100f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Piece") || collision.collider.CompareTag("CommandCentre"))
        {
            Explode(damage);
            Destroy(gameObject);
        }
    }

    public void Explode(int damageAmount)
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, radius);

        List<Rigidbody2D> uniqueBodies = new List<Rigidbody2D>();

        foreach (Collider2D collision in collisions)
        {
            if (collision.TryGetComponent(out Damageable dother))
            {
                dother.DealDamage(damage);
            }

            Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
            if (!uniqueBodies.Contains(rb))
            {
                uniqueBodies.Add(rb);
            }
        }
    }
}

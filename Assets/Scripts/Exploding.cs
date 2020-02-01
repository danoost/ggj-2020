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

    private int numberOfRays = 32;

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
        float angleDifference = 2 * Mathf.PI / numberOfRays;
        foreach (float k in Enumerable.Range(0, numberOfRays))
        {
            float angle = k * angleDifference;
            Vector2 direction = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * Vector2.right;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, radius);
            Debug.DrawRay(transform.position, direction * radius, Color.green, 3f);

            foreach (RaycastHit2D hit in hits)
            {
                if (!hit.collider.CompareTag("Damaging"))
                {
                    Vector2 difference = hit.collider.ClosestPoint(transform.position) - (Vector2)transform.position;
                    hit.collider.attachedRigidbody.AddForceAtPosition(difference * pushForce, transform.position);
                    hit.collider.GetComponent<Damageable>().DealDamage(damageAmount);
                }
            }
        }
    }
}

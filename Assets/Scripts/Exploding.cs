using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Exploding : MonoBehaviour
{
    public int Damage { get; set; } = 40;

    public float PushForceScale { get; set; } = 1f;

    [SerializeField]
    private float radius = 0.5f;

    [SerializeField]
    private float basePushForce = 50f;

    private readonly int numberOfRays = 32;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Piece") || collision.collider.CompareTag("CommandCentre"))
        {
            Explode(Damage);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        TrailRenderer tr = GetComponentInChildren<TrailRenderer>();
        if (tr != null)
        {
            Destroy(tr.gameObject, tr.time);
        }
        gameObject.transform.DetachChildren();
    }

    public void Explode(int damageAmount)
    {
        float angleDifference = 2 * Mathf.PI / numberOfRays;

        List<Damageable> haveDealtDamage = new List<Damageable>();
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
                    Vector2 difference = (hit.collider.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
                    if (hit.collider.TryGetComponent(out Damageable d))
                    {
                        if (!haveDealtDamage.Contains(d))
                        {
                            d.DealDamage(damageAmount);
                            haveDealtDamage.Add(d);
                        }
                    }
                    if (hit.collider.attachedRigidbody != null)
                    {
                        hit.collider.attachedRigidbody.AddForceAtPosition(difference * basePushForce * PushForceScale, transform.position);
                    }
                }
            }
        }
    }
}

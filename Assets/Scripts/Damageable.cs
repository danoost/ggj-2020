using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int currentHealth;

    public float HealthScale { get; set; } = 1f;

    private void Start()
    {
        currentHealth = (int)(maxHealth * HealthScale);
    }

    public void DealDamage(int amount)
    {
        Debug.Log($"oof. {amount} damage, {currentHealth} -> {currentHealth - amount}");
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (TryGetComponent(out Piece piece))
        {
            piece.Detach();
        }
        Destroy(gameObject);
    }
}

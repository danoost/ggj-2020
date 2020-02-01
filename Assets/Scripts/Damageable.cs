using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [HideInInspector]
    public int currentHealth;

    [SerializeField]
    private bool player = false;
    private bool cooledDown = true;

    public float HealthScale { get; set; } = 1f;

    private void Start()
    {
        if (player)
        {
            currentHealth = 3;
            Debug.Log(currentHealth);
        }
        else
        {
            currentHealth = (int)(maxHealth * HealthScale);
        }
    }

    public void DealDamage(int amount)
    {
        if (player)
        {
            if (!cooledDown) return;

            currentHealth--;
            Debug.Log("Player Damaged");
            cooledDown = false;
            StartCoroutine(CoolDown());
        }
        else
        {
            Debug.Log($"oof. {amount} damage, {currentHealth} -> {currentHealth - amount}");
            currentHealth -= amount;
        }
        if (currentHealth <= 0)
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

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(2);
        cooledDown = true;
    }
}

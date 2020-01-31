using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int amount)
    {
        Debug.Log("oof");
        currentHealth -= amount;
        if (currentHealth < 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        foreach (Transform child in gameObject.transform)
        {
            child.GetComponent<Piece>().Detach();
        }
        Destroy(gameObject);
    }
}

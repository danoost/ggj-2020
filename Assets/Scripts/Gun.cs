using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Piece
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private int damage = 40;

    [SerializeField]
    private float shotsPerSecond = 1;

    [SerializeField]
    private float bulletSpeed = 5f;

    [SerializeField]
    private float bulletLifetime = 20f;

    private Coroutine shootingCoroutine;
    private bool isShooting = false;

    private void FixedUpdate()
    {
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootLots());
        }

        if (!isShooting && InputHandler.IsInteracting)
        {
            isShooting = true;
        }
        if (isShooting && !InputHandler.IsInteracting)
        {
            isShooting = false;
        }
    }

    IEnumerator ShootLots()
    {
        while (true)
        {
            if (isShooting && root != null)
            {
                Shoot();
            }
            yield return new WaitForSeconds(1 / shotsPerSecond);
        }
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, transform.position + (transform.up * 0.5f), transform.rotation);
        newBullet.GetComponent<Exploding>().Damage = damage;
        newBullet.GetComponent<Rigidbody2D>().velocity = newBullet.transform.up * bulletSpeed;
        Destroy(newBullet, bulletLifetime);
    }
}

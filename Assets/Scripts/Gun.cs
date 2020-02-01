﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Piece
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float bulletLifetime = 20f;

    [SerializeField]
    private float baseBulletScale = 0.4f;

    [SerializeField]
    private int baseDamage = 40;

    [SerializeField]
    private float baseShotsPerSecond = 1.5f;

    [SerializeField]
    private float baseBulletSpeed = 5f;

    [SerializeField]
    private float baseRecoilForce = 10f;

    float scaleModifier;

    float inverseScaleModifier;

    protected new void Start()
    {
        base.Start();
        StartCoroutine(ShootLots());
        scaleModifier = transform.lossyScale.x / baseScale;
        inverseScaleModifier = 1 / scaleModifier;
    }

    IEnumerator ShootLots()
    {
        while (true)
        {
            if (root == null)
            {
                yield return new WaitUntil(() => root != null);
            }
            if (!rootController.Interacting)
            {
                yield return new WaitUntil(() => rootController.Interacting);
            }
            Shoot();
            yield return new WaitForSeconds(1 / baseShotsPerSecond * scaleModifier);
        }
    }

    private void Shoot()
    {
        Vector2 bulletSpawnPoint = transform.position + (transform.up * 2f * transform.lossyScale.x);
        GameObject newBullet = Instantiate(bullet, bulletSpawnPoint, transform.rotation);

        newBullet.transform.localScale = transform.lossyScale * baseBulletScale;

        newBullet.GetComponent<Exploding>().Damage = (int)(baseDamage * scaleModifier);
        newBullet.GetComponent<Exploding>().PushForceScale = scaleModifier;
        newBullet.GetComponent<Rigidbody2D>().velocity = newBullet.transform.up * baseBulletSpeed * inverseScaleModifier;
        Destroy(newBullet, bulletLifetime);

        Debug.Log(newBullet.GetComponent<Exploding>().Damage);

        rootRb.AddForceAtPosition(baseRecoilForce * -transform.up * scaleModifier, bulletSpawnPoint);
    }
}
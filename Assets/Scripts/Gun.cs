using System.Collections;
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

    protected new void Awake()
    {
        base.Awake();
        StartCoroutine(ShootLots());
        scaleModifier = transform.lossyScale.x / baseScale;
        inverseScaleModifier = 1 / scaleModifier;
    }

    IEnumerator ShootLots()
    {
        while (true)
        {
            if (rootController == null || !rootController.Interacting)
            {
                yield return new WaitUntil(() =>
                    rootController != null && rootController.Interacting
                );
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
        newBullet.GetComponent<DynamicMaterialColor>().SetColor(GetComponent<DynamicMaterialColor>().GetColor());

        newBullet.GetComponent<Exploding>().Damage = (int)(baseDamage * scaleModifier);
        newBullet.GetComponent<Exploding>().PushForceScale = scaleModifier;
        newBullet.GetComponent<Rigidbody2D>().velocity = (Vector2)newBullet.transform.up * baseBulletSpeed * inverseScaleModifier + rootRb.velocity;
        Destroy(newBullet, bulletLifetime);

        rootRb.AddForceAtPosition(baseRecoilForce * -transform.up * scaleModifier, bulletSpawnPoint);
    }
}

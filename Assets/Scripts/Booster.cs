using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Piece
{
    [SerializeField]
    private float boostStrength = 1f;

    [SerializeField]
    private ParticleSystem flameSystem;
    ParticleSystem.MainModule main;

    protected new void Awake()
    {
        base.Awake();
        main = flameSystem.main;
        main.startSize = main.startSize.constant * transform.lossyScale.x / baseScale;
    }

    private void Start()
    {
        UpdateColor();
    }

    private void UpdateColor()
    {

        if (rootVisual != null)
        {
            var main = flameSystem.main;
            main.startColor = rootVisual.Color;
        }
    }

    private void FixedUpdate()
    {
        bool flame = false;

        if (rootController == null)
            return;

        // Forward and backward pushiness
        float angleBetween = Quaternion.Angle(root.transform.rotation, transform.rotation);
        float cos = Mathf.Cos(Mathf.Deg2Rad * angleBetween);
        float boostAmount = rootController.Movement.y * boostStrength * cos;

        if (boostAmount > 0.3)
        {
            BoostMassaged(boostAmount);
            flame = true;
        }

        // Torquiness
        float cross = Vector3.Cross((transform.position - (Vector3)rootRb.worldCenterOfMass).normalized, transform.up).z;
        float sign = Mathf.Sign(cross);

        float horizontalMovement = rootController.Movement.x;
        if (Mathf.Abs(cross) > 0.05 && Mathf.Abs(horizontalMovement) > 0.05f && Mathf.Sign(horizontalMovement) == sign)
        {
            Boost(boostStrength * Mathf.Abs(horizontalMovement));
            flame = true;
        }

        var emission = flameSystem.emission;
        emission.enabled = flame;
    }

    protected new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        UpdateColor();
    }

    private void BoostMassaged(float amount)
    {
        if (amount < 0) return;
        amount *= transform.lossyScale.x / baseScale;
        rootRb.AddForceAtPosition(amount * ((root.transform.up * Mathf.Sign(rootController.Movement.y)) + transform.up) / 2, transform.position);
    }

    private void Boost(float amount)
    {
        if (amount < 0) return;
        rootRb.AddForceAtPosition(amount * transform.up, transform.position);
    }
}

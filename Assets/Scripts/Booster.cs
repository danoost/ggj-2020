using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Piece
{
    [SerializeField]
    private float boostStrength = 1f;

    [SerializeField]
    private ParticleSystem flameSystem;

    protected new void Start()
    {
        base.Start();
        var main = flameSystem.main;
        main.startSize = main.startSize.constant * transform.lossyScale.x / baseScale;
    }

    private void FixedUpdate()
    {
        if (rootRb == null)
        {
            return;
        }

        bool flame = false;

        // Forward and backward pushiness
        float angleBetween = Quaternion.Angle(root.transform.rotation, transform.rotation);
        float cos = Mathf.Cos(Mathf.Deg2Rad * angleBetween);
        float boostAmount = InputHandler.VerticalMovement * boostStrength * cos;

        if (boostAmount > 0.05)
        {
            BoostMassaged(boostAmount);
            flame = true;
        }

        // Torquiness

        float cross = Vector3.Cross((transform.position - root.transform.position).normalized, transform.forward).y;
        //Debug.Log(cross);
        float sign = Mathf.Sign(cross);

        float horizontalMovement = InputHandler.HorizontalMovement;
        if (Mathf.Abs(cross) > 0.05 && Mathf.Abs(horizontalMovement) > 0.05f && Mathf.Sign(horizontalMovement) == sign)
        {
            Boost(0.5f * boostStrength * Mathf.Abs(horizontalMovement));
            flame = true;
        }

        var emission = flameSystem.emission;
        emission.enabled = flame;
    }

    private void BoostMassaged(float amount)
    {
        if (amount < 0) return;
        amount *= transform.lossyScale.x / baseScale;
        rootRb.AddForceAtPosition(amount * ((root.transform.forward * Mathf.Sign(InputHandler.VerticalMovement)) + transform.forward) / 2, transform.position);
    }

    private void Boost(float amount)
    {
        if (amount < 0) return;
        rootRb.AddForceAtPosition(amount * transform.forward, transform.position);
    }
}

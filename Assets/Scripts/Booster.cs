﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : Piece
{
    [SerializeField]
    private float boostStrength = 1f;

    private void FixedUpdate()
    {
        if (rootRb == null)
        {
            return;
        }

        // Forward pushiness

        float angleBetween = Quaternion.Angle(root.transform.rotation, transform.rotation);
        float cos = Mathf.Cos(Mathf.Deg2Rad * angleBetween);
        float boostAmount = InputHandler.VerticalMovement * boostStrength * cos;

        BoostMassaged(boostAmount);

        // Torquiness

        float cross = Vector3.Cross((transform.position - root.transform.position).normalized, transform.up).z;
        float sign = Mathf.Sign(cross);

        float horizontalMovement = InputHandler.HorizontalMovement;
        if (Mathf.Abs(cross) > 0.05 && Mathf.Abs(horizontalMovement) > 0.05f && Mathf.Sign(horizontalMovement) == sign)
        {
            Boost(0.5f * boostStrength * Mathf.Abs(horizontalMovement));
        }
    }

    private void BoostMassaged(float amount)
    {
        if (amount < 0) return;
        amount *= transform.lossyScale.x / baseScale;
        rootRb.AddForceAtPosition(amount * ((root.transform.up * Mathf.Sign(InputHandler.VerticalMovement)) + transform.up) / 2, transform.position);
    }

    private void Boost(float amount)
    {
        if (amount < 0) return;
        rootRb.AddForceAtPosition(amount * -transform.up, transform.position);
    }
}

﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Horizontal input multiplier.")]
    private float horizontalSpeedMult = 20f;

    [SerializeField]
    [Tooltip("Vertical input multiplier.")]
    private float verticalSpeedMult = 15f;

    [SerializeField]
    [Tooltip("Limit in either direction of horizontal movement.")]
    private float horizontalLimit = 15f;

    [SerializeField]
    [Tooltip("Limit in the up direction for vertical movement.")]
    private float verticalLimitUp = 10f;

    [SerializeField]
    [Tooltip("Limit in the down direction for vertical movement.")]
    private float verticalLimitDown = 10f;

    [SerializeField]
    [Tooltip("Compensate for the odd viewing angle when the ship is at the edge of the screen.")]
    private float horizontalYawAdjustment = 7.44f;

    [SerializeField]
    [Tooltip("Compensate for the odd viewing angle when the ship is at the edge of the screen.")]
    private float verticalPitchAdjustment = 22.943f;

    [SerializeField]
    [Tooltip("Compensate for the odd viewing angle when the ship is at the edge of the screen.")]
    private float horizontalRollAdjustment = 10.385f;

    [SerializeField]
    [Tooltip("Roll effect added when player is moving horizontally.")]
    private float horizontalRollWhileMoving = -20f;

    [SerializeField]
    [Tooltip("Yaw effect added when player is moving horizontally.")]
    private float horizontalYawWhileMoving = 20f;

    [SerializeField]
    [Tooltip("Pitch effect added when player is moving vertically.")]
    private float verticalPitchWhileMoving = -20f;

    [SerializeField]
    [Tooltip("Lerp rate used when moving the ship from its current rotation to the target rotation.")]
    private float rotationLerpRate = .2f;

    private Camera shipCamera;

    // Start is called before the first frame update
    void Start()
    {
        this.shipCamera = this.GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontalThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        var verticalThrow = CrossPlatformInputManager.GetAxis("Vertical");

        var localPosition = this.transform.localPosition;

        var newLocalPosition = new Vector3(
            Mathf.Clamp(localPosition.x + (horizontalThrow * horizontalSpeedMult * Time.deltaTime), -horizontalLimit, horizontalLimit),
            Mathf.Clamp(localPosition.y + (verticalThrow * verticalSpeedMult * Time.deltaTime), -verticalLimitDown, verticalLimitUp),
            localPosition.z);

        this.transform.localPosition = newLocalPosition;

        float xSign = Mathf.Sign(this.transform.localPosition.x);
        float ySign = Mathf.Sign(this.transform.localPosition.y);

        var distanceFromLocalOrigin = new Vector2(localPosition.x, localPosition.y).magnitude;

        var targetRotation = Quaternion.Euler(
                ySign 
                    * Mathf.Lerp(0, verticalPitchAdjustment, Mathf.Abs(newLocalPosition.y) / verticalLimitDown)
                    + verticalThrow * verticalPitchWhileMoving,
                xSign 
                    * Mathf.Lerp(0, horizontalYawAdjustment, Mathf.Abs(newLocalPosition.x) / horizontalLimit)
                    + horizontalThrow * horizontalYawWhileMoving,
                Mathf.Abs(newLocalPosition.x * newLocalPosition.y) 
                    * xSign
                    * ySign
                    * Mathf.Lerp(0, horizontalRollAdjustment, newLocalPosition.magnitude / new Vector2(horizontalLimit, verticalLimitDown).magnitude)
                    + horizontalThrow * horizontalRollWhileMoving
                );

        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, targetRotation, .2f);
    }
}

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private LevelChanger levelChanger;

    // Start is called before the first frame update
    void Start()
    {
        this.shipCamera = this.GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void OnPlayerDeath()
    {
        this.isControlEnabled = false;

        levelChanger.OnReloadLevel();
    }

    private void ProcessInput()
    {
        if(!isControlEnabled)
        {
            return;
        }

        var horizontalThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        var verticalThrow = CrossPlatformInputManager.GetAxis("Vertical");

        HandlePositionInput(horizontalThrow, verticalThrow);

        HandleRotationInput(horizontalThrow, verticalThrow);
    }

    private Vector3 HandlePositionInput(float horizontalThrow, float verticalThrow)
    {
        var localPosition = this.transform.localPosition;

        var newLocalPosition = new Vector3(
            Mathf.Clamp(localPosition.x + (horizontalThrow * horizontalSpeedMult * Time.deltaTime), -horizontalLimit, horizontalLimit),
            Mathf.Clamp(localPosition.y + (verticalThrow * verticalSpeedMult * Time.deltaTime), -verticalLimitDown, verticalLimitUp),
            localPosition.z);

        this.transform.localPosition = newLocalPosition;

        return newLocalPosition;
    }

    private void HandleRotationInput(float horizontalThrow, float verticalThrow)
    {
        var newLocalPosition = this.transform.localPosition;

        // Start calculating the local rotation we should apply to correct for the perspective of the camera, 
        // and to make the ship rotate when the player controls it
        float xSign = Mathf.Sign(this.transform.localPosition.x);
        float ySign = Mathf.Sign(this.transform.localPosition.y);

        var pitchPerspectiveDelta = ySign
                    * Mathf.Lerp(0, verticalPitchAdjustment, Mathf.Abs(newLocalPosition.y) / verticalLimitDown);

        var pitchMovingDelta = verticalThrow * verticalPitchWhileMoving;

        float yawPerspectiveDelta = xSign
                    * Mathf.Lerp(0, horizontalYawAdjustment, Mathf.Abs(newLocalPosition.x) / horizontalLimit);

        float yawMovingDelta = horizontalThrow * horizontalYawWhileMoving;

        float rollPerspectiveDelta = Mathf.Abs(newLocalPosition.x * newLocalPosition.y)
                    * xSign
                    * ySign
                    * Mathf.Lerp(0, horizontalRollAdjustment, newLocalPosition.magnitude / new Vector2(horizontalLimit, verticalLimitDown).magnitude);

        float rollMovingDelta = horizontalThrow * horizontalRollWhileMoving;

        var targetRotation = Quaternion.Euler(
                pitchPerspectiveDelta + pitchMovingDelta,
                yawPerspectiveDelta + yawMovingDelta,
                rollPerspectiveDelta + rollMovingDelta
                );

        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, targetRotation, this.rotationLerpRate);
    }

    [Header("Input Movement")]

    [SerializeField] [Tooltip("Horizontal input multiplier.")]
    private float horizontalSpeedMult = 11f;

    [SerializeField] [Tooltip("Vertical input multiplier.")]
    private float verticalSpeedMult = 8f;

    [SerializeField] [Tooltip("Limit in either direction of horizontal movement.")]
    private float horizontalLimit = 7f;

    [SerializeField] [Tooltip("Limit in the up direction for vertical movement.")]
    private float verticalLimitUp = 3f;

    [SerializeField] [Tooltip("Limit in the down direction for vertical movement.")]
    private float verticalLimitDown = 4;

    [Header("Input Rotation")]

    [SerializeField] [Tooltip("Roll effect added when player is moving horizontally.")]
    private float horizontalRollWhileMoving = -20f;

    [SerializeField] [Tooltip("Yaw effect added when player is moving horizontally.")]
    private float horizontalYawWhileMoving = 25f;

    [SerializeField] [Tooltip("Pitch effect added when player is moving vertically.")]
    private float verticalPitchWhileMoving = -20f;

    [SerializeField] [Tooltip("Lerp rate used when moving the ship from its current rotation to the target rotation.")]
    private float rotationLerpRate = .2f;

    [Header("Perspective Compensation")]

    [SerializeField] [Tooltip("Compensate for the odd viewing angle when the ship is at the edge of the screen.")]
    private float horizontalYawAdjustment = 30;

    [SerializeField] [Tooltip("Compensate for the odd viewing angle when the ship is at the edge of the screen.")]
    private float verticalPitchAdjustment = -22.943f;

    [SerializeField] [Tooltip("Compensate for the odd viewing angle when the ship is at the edge of the screen.")]
    private float horizontalRollAdjustment = -.8f;

    private Camera shipCamera;
    private bool isControlEnabled = true;
}

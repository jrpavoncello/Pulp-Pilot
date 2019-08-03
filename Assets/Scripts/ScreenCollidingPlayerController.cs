using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ScreenCollidingPlayerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Horizontal input multiplier.")]
    private float hortizontalSpeedMult = 20f;

    [SerializeField]
    [Tooltip("Vertical input multiplier.")]
    private float verticalSpeedMult = 15f;

    [SerializeField]
    private List<Transform> screenBumpers;

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

        var localDelta = new Vector3(
            horizontalThrow * hortizontalSpeedMult * Time.deltaTime,
            verticalThrow * verticalSpeedMult * Time.deltaTime,
            0f);

        List<Vector3> bumperScreenPositions = new List<Vector3>(this.screenBumpers.Count);

        bool xCollision = false;
        bool yCollision = false;

        foreach (var bumper in this.screenBumpers)
        {
            var newWorldBumperPos = bumper.TransformPoint(bumper.localPosition + localDelta);
            var oldScreenPos = this.shipCamera.WorldToScreenPoint(bumper.position);
            var newScreenPos = this.shipCamera.WorldToScreenPoint(newWorldBumperPos);

            if (// Previous position was inside the bounds, let this next one go. It prevents jitters at the edge of the screen.
                (oldScreenPos.x <= 0 || oldScreenPos.x >= Screen.width) &&
                // Allow the ship to move back on screen, even from off screen
                ((localDelta.x < 0 && newScreenPos.x < 0) ||
                (localDelta.x > 0 && newScreenPos.x > Screen.width)))
            {
                xCollision = true;
            }

            if (// Previous position was inside the bounds, let this next one go. It prevents jitters at the edge of the screen.
                (oldScreenPos.y <= 0 || oldScreenPos.y >= Screen.height) &&
                // Allow the ship to move back on screen, even from off screen
                ((localDelta.y < 0 && newScreenPos.y < 0) ||
                (localDelta.y > 0 && newScreenPos.y > Screen.height)))
            {
                yCollision = true;
            }

            //Debug.Log("Screen: " + newScreenPos);
            //Debug.Log("Delta: " + localDelta);
            //Debug.Log("Screen Delta: " + (newScreenPos - oldScreenPos));
        }

        if(xCollision)
        {
            localDelta = new Vector3(0f, localDelta.y, localDelta.z);
        }

        if(yCollision)
        {
            localDelta = new Vector3(localDelta.x, 0f, localDelta.z);
        }

        this.transform.localPosition += localDelta;

        foreach (var bumper in this.screenBumpers)
        {
            bumper.transform.localPosition += localDelta;
        }
    }
}

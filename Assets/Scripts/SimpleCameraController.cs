using UnityEngine;

namespace UnityTemplateProjects
{
    public class SimpleCameraController : MonoBehaviour
    {
        class CameraState
        {
            public Quaternion rotation;
            public float x;
            public float y;
            public float z;

            public void SetFromTransform(Transform t)
            {
                rotation = t.rotation;
                x = t.position.x;
                y = t.position.y;
                z = t.position.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = rotation * translation;

                x += rotatedTranslation.x;
                y += rotatedTranslation.y;
                z += rotatedTranslation.z;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                rotation = Quaternion.Lerp(rotation, target.rotation, rotationLerpPct);

                x = Mathf.Lerp(x, target.x, positionLerpPct);
                y = Mathf.Lerp(y, target.y, positionLerpPct);
                z = Mathf.Lerp(z, target.z, positionLerpPct);
            }

            public void UpdateTransform(Transform t)
            {
                t.rotation = rotation;
                t.position = new Vector3(x, y, z);
            }
        }

        CameraState m_TargetCameraState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        [Header("Movement Settings")]
        [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
        public float boost = 3.5f;

        [Header("Movement Settings")]
        [Tooltip("Base move speed.")]
        public float speed = 10.0f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float positionLerpTime = 0.2f;

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = false;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
        }

        Vector3 GetInputTranslationDirection()
        {
            Vector3 direction = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                direction += -Vector3.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                direction += -Vector3.right;
            }

            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                direction += -Vector3.up;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                direction += Vector3.up;
            }

            return direction;
        }

        void Update()
        {
            // Exit Sample  
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }

            // Hide and lock cursor when right mouse button pressed
            if (Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Unlock and show cursor when right mouse button released
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // x=Pitch, y=Yaw, z=Roll
            Vector3 keyRotation = new Vector3();

            float roll = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                roll = 1f;
            }

            if (Input.GetKey(KeyCode.E))
            {
                roll = -1f;
            }

            // Rotation
            if (Input.GetMouseButton(1))
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                m_TargetCameraState.rotation *= Quaternion.Euler(mouseMovement.y * mouseSensitivityFactor, mouseMovement.x * mouseSensitivityFactor, roll);
            }

            // Translation
            var translation = GetInputTranslationDirection() * Time.deltaTime * this.speed;

            // Speed up movement when shift key held
            if (Input.GetKey(KeyCode.LeftShift))
            {
                translation *= 3.0f;
            }

            // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel)
            boost += Input.mouseScrollDelta.y;
            translation *= boost;

            m_TargetCameraState.Translate(translation);

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }
    }

}
using UnityEngine;

namespace DinosaurGame.Product.Dinosaurs
{
    public enum DinosaurCameraMode
    {
        Home,
        Pet,
        Feed,
        Wash,
        Play
    }

    public sealed class DinosaurInteractionCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform focusTarget;
        [SerializeField] private Vector3 homeOffset = new Vector3(0f, 2.5f, -6f);
        [SerializeField] private Vector3 closeOffset = new Vector3(0f, 1.8f, -3.8f);
        [SerializeField] private float positionSharpness = 8f;
        [SerializeField] private float rotationSharpness = 10f;

        private Vector3 _targetOffset;
        public DinosaurCameraMode Mode { get; private set; } = DinosaurCameraMode.Home;

        private void Awake()
        {
            if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
            _targetOffset = homeOffset;
        }

        private void LateUpdate()
        {
            if (cameraTransform == null || focusTarget == null) return;
            var t = 1f - Mathf.Exp(-positionSharpness * Time.unscaledDeltaTime);
            var r = 1f - Mathf.Exp(-rotationSharpness * Time.unscaledDeltaTime);
            var desiredPosition = focusTarget.TransformPoint(_targetOffset);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, t);
            var look = focusTarget.position + Vector3.up * 0.8f - cameraTransform.position;
            if (look.sqrMagnitude > 0.001f)
                cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(look.normalized, Vector3.up), r);
        }

        public void SetMode(DinosaurCameraMode mode)
        {
            Mode = mode;
            _targetOffset = mode == DinosaurCameraMode.Home ? homeOffset : closeOffset;
        }

        public void SetFocusTarget(Transform target) => focusTarget = target;
    }
}

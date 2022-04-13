using UnityEngine;

namespace Farm.CameraSystem
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 CurrentCameraOffset;

        [SerializeField] private Vector3 _origin;
        [SerializeField] private Vector3 _targetPoint;

        [SerializeField] private bool _drag = false;

        private void Awake()
        {
            CurrentCameraOffset = Camera.main.transform.position;
        }

        private void LateUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                _targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
                if (_drag == false)
                {
                    _drag = true;
                    _origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                _drag = false;
            }

            if (_drag)
            {
                Camera.main.transform.position = _origin - _targetPoint;
            }
        }
    }
}

using UnityEngine;

namespace Farm.CameraSystem
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float MaxZoomValue;
        [SerializeField] private float MinZoomValue;

        [SerializeField] private float zoom;

        private void Start()
        {
            zoom = Camera.main.orthographicSize;
            Debug.Log(zoom);
        }

        private void LateUpdate()
        {
            var scroolValue = Input.mouseScrollDelta.y;

            zoom += scroolValue * -1;

            zoom = Mathf.Clamp(zoom, MinZoomValue, MaxZoomValue);
            Camera.main.orthographicSize = zoom;
        }
    }
}

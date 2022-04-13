using UnityEngine;
using Farm.Grid;
using UnityEngine.UI;

namespace Farm.Buildings
{
    public class Building : MonoBehaviour
    {
        public bool isPlaced { get; private set; }
        public BoundsInt area;

        private const float ButtonUIPositionX = -0.5f;
        private const float ButtonUIPositionY = 0f;

        [SerializeField] private Transform GFX;
        [SerializeField] private Canvas UICanvas;
        [SerializeField] private Button approveButton;
        [SerializeField] private Button rejectButton;

        private void Update()
        {
            ButtonHandle();
        }

        #region Building
        private void Place()
        {
            Vector3Int position = GridBuildingManager.Instance.gridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = area;
            areaTemp.position = position;
            isPlaced = true;
            GridBuildingManager.Instance.TakeArea(areaTemp);
        }

        private bool CanBePlaced()
        {
            Vector3Int position = GridBuildingManager.Instance.gridLayout.LocalToCell(transform.position);
            BoundsInt areaTemp = area;
            areaTemp.position = position;

            if (GridBuildingManager.Instance.CanTakeArea(areaTemp))
                return true;
            return false;
        }
        #endregion

        #region Buttons
        private void ButtonHandle()
        {
            Vector3 approvePos = GFX.position + new Vector3(ButtonUIPositionX, ButtonUIPositionY, 0f);
            Vector3 rejectPos = GFX.position - new Vector3(ButtonUIPositionX, -ButtonUIPositionY, 0f);

            approveButton.transform.position = Camera.main.WorldToScreenPoint(approvePos);
            rejectButton.transform.position = Camera.main.WorldToScreenPoint(rejectPos);
        }

        public void ApproveButtonEvent()
        {
            if (CanBePlaced())
            {
                UICanvas.enabled = false;
                Place();
            }
        }

        public void RejectButtonEvent()
        {
            Destroy(gameObject);
            GridBuildingManager.Instance.ClearArea();
        }
        #endregion
    }
}
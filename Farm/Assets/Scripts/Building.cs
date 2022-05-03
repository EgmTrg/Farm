using UnityEngine.UI;
using Farm.UI.Shop;
using UnityEngine;
using Farm.Grid;
using TMPro;

namespace Farm.Buildings
{
    public class Building : MonoBehaviour
    {
        public bool isPlaced { get; private set; }
        public BoundsInt area;

        [Space(20)]
        [SerializeField] private Transform GFX;
        [SerializeField] private Canvas UICanvas;
        [SerializeField] private Button approveButton;
        [SerializeField] private Button rejectButton;
        [SerializeField] private ShopItem shopItem;
        private ShopItem.UISettings settings;

        #region Unity Methods
        private void Start()
        {
            settings = shopItem.InitSettings();
            ApplieButtonSettings();
        }

        private void Update()
        {
            ButtonHandle();
        }
        #endregion

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
            Vector3 approvePos = GFX.position + new Vector3(settings.positionX, settings.positionY, 0f);
            Vector3 rejectPos = GFX.position - new Vector3(settings.positionX, -settings.positionY, 0f);

            approveButton.transform.position = Camera.main.WorldToScreenPoint(approvePos);
            rejectButton.transform.position = Camera.main.WorldToScreenPoint(rejectPos);
        }

        private void ApplieButtonSettings()
        {
            Vector2 buttonSizes = new Vector2(settings.width, settings.height);
            Vector2 buttonPositions = new Vector2(settings.positionX, settings.positionY);

            approveButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = settings.fontSize;
            rejectButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = settings.fontSize;

            RectTransform approveButtonRect = approveButton.GetComponent<RectTransform>();
            approveButtonRect.sizeDelta = buttonSizes;
            approveButtonRect.position = buttonPositions;
            
            RectTransform rejectButtonRect = rejectButton.GetComponent<RectTransform>();
            rejectButtonRect.sizeDelta = buttonSizes;
            rejectButtonRect.position= buttonPositions;
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
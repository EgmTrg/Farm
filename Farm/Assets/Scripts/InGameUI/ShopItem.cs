using UnityEngine;

namespace Farm.UI.Shop
{
    [CreateAssetMenu(fileName = "Item", menuName = "Shop/Item")]
    public class ShopItem : ScriptableObject
    {
        #region Definations
        [System.Serializable]
        public struct UISettings
        {
            public UISettings(BuildingSize buildingSize, float positionX, float positionY, float width, float height, float fontSize)
            {
                this.buildingSize = buildingSize;
                this.positionX = positionX;
                this.positionY = positionY;
                this.width = width;
                this.height = height;
                this.fontSize = fontSize;
            }

            public BuildingSize buildingSize;
            public float positionX;
            public float positionY;
            public float width;
            public float height;
            public float fontSize;
        }
        public enum BuildingSize { Empty, Ground_or_Tile, Small_Building, Big_Building }
        #endregion

        public UISettings UISettingsProps
        {
            get { return uiSettings; }
            set
            {
                if (uiSettings.buildingSize != value.buildingSize)
                    UISizeHandler();
            }
        }

        [Header("Shop Settings")]
        public GameObject prefab;
        public Sprite sprite;
        public new string name;
        public int price;

        [Header("InGameUI Canvas And Buttons Settings")]
        [SerializeField] private UISettings uiSettings;

        public UISettings InitSettings() => uiSettings = UISizeHandler();

        #region UISettings Operations
        private UISettings UISizeHandler()
        {
            switch (uiSettings.buildingSize)
            {
                case BuildingSize.Empty:
                    return new UISettings();
                case BuildingSize.Ground_or_Tile:
                    return SetUISettings(
                        buildingSize: uiSettings.buildingSize,
                        posX: -0.3f,
                        posY: -0.5f,
                        width: 25,
                        height: 25,
                        fontSize: 15);
                case BuildingSize.Small_Building:
                    return SetUISettings(
                        buildingSize: uiSettings.buildingSize,
                        posX: -0.5f,
                        posY: -0.2f,
                        width: 30,
                        height: 30,
                        fontSize: 20);
                case BuildingSize.Big_Building:
                    return SetUISettings(
                        buildingSize: uiSettings.buildingSize,
                        posX: -0.7f,
                        posY: -0.5f,
                        width: 35,
                        height: 35,
                        fontSize: 25);
                default:
                    return new UISettings();
            }
        }

        private UISettings SetUISettings(BuildingSize buildingSize, float posX, float posY, int width, int height, int fontSize)
        {
            UISettings newSettings = new UISettings();
            newSettings.buildingSize = buildingSize;
            newSettings.positionX = posX;
            newSettings.positionY = posY;
            newSettings.width = width;
            newSettings.height = height;
            newSettings.fontSize = fontSize;
            return newSettings;
        }
        #endregion
    }
}

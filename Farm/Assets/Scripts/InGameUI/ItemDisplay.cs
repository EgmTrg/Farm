using UnityEngine.UI;
using UnityEngine;
using Farm.Grid;
using TMPro;
using UnityEngine.SceneManagement;

namespace Farm.UI.Shop
{
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private ShopItem shopItem;

        public Image image;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI priceText;

        void Start()
        {
            image.sprite = shopItem.sprite;
            nameText.text = shopItem.name;
            priceText.text = shopItem.price.ToString();
        }

        public void InstantiateToGameplayScene()
        {
            GridBuildingManager.Instance.InitializeWithBuilding(shopItem.prefab);
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("InGameUI"));
        }
    }
}
using UnityEngine;

namespace Farm.UI.Shop
{
    [CreateAssetMenu(fileName = "Item",menuName ="Shop/Item")]
    public class ShopItem : ScriptableObject
    {
        public GameObject prefab;
        public Sprite sprite;
        public new string name;
        public int price;
    }
}

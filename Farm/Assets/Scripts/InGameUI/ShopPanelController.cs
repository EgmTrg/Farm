using UnityEngine.SceneManagement;
using UnityEngine;

namespace Farm.UI.Shop
{
    public class ShopPanelController : MonoBehaviour
    {
        public void QuitButton() =>
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("InGameUI"));
    }
}

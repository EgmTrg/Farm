using UnityEngine.SceneManagement;
using UnityEngine;

namespace Farm.UI
{
    public class UILoader : MonoSingleton<UILoader>
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                if (!SceneManager.GetSceneByName("InGameUI").isLoaded)
                    SceneManager.LoadSceneAsync("InGameUI", LoadSceneMode.Additive);
                else
                    SceneManager.UnloadSceneAsync("InGameUI");
        }
    }
}

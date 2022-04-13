using System.Collections;
using UnityEngine;

namespace Farm
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public Vector3 CenterOfTheScreen { get; private set; }

        private void LateUpdate()
        {
            CenterOfTheScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        }
    }
}
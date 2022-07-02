using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScene
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance { get { return _instance; } }



        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("TetrisMainScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

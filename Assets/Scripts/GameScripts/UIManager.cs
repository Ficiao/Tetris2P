using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameScene
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance { get { return _instance; } }

        public Transform ui;

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

        private void Start()
        {
            StartCoroutine(StartingGame());
        }

        private IEnumerator StartingGame()
        {
            ui.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            ui.GetChild(0).gameObject.SetActive(false);
            ui.GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            ui.GetChild(1).gameObject.SetActive(false);
            GameManager.Instance.GameStart();
        }

        public void GameEndScreen(string playerName)
        {
            ui.GetChild(2).gameObject.SetActive(true);
            ui.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(playerName + " WINS!");
            ui.GetChild(3).gameObject.SetActive(true);
            ui.GetChild(4).gameObject.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene("TetrisMainScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

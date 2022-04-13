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
        public TextMeshProUGUI player1Score;
        public TextMeshProUGUI player2Score;
        public TextMeshProUGUI player1SumLines;
        public TextMeshProUGUI player2SumLines;
        public TextMeshProUGUI player1PPS;
        public TextMeshProUGUI player2PPS;
        public Transform ui;
        public GameObject ready;
        public GameObject go;
        public GameObject playerWins;
        public GameObject playAgane;
        public GameObject quit;
        private int sumLines1;
        private int sumLines2;
        public int pieces1;
        public int pieces2;
        private IEnumerator countPPS;
        public float startTime;

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

        public void GameStart()
        {
            countPPS = PPSCounter();
            StartCoroutine(countPPS);
            startTime = Time.time;
            SetScores();
            StartCoroutine(StartingGame());
        }

        private IEnumerator PPSCounter()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                player1PPS.SetText("PIECES PER SECOND: " + String.Format("{0:0.00}", pieces1 / (Time.time - startTime)));
                player2PPS.SetText("PIECES PER SECOND: " + String.Format("{0:0.00}", pieces2 / (Time.time - startTime)));
            }
        }

        private IEnumerator StartingGame()
        {
            ready.SetActive(true);
            yield return new WaitForSeconds(1);
            ready.SetActive(false);
            go.SetActive(true);
            yield return new WaitForSeconds(1);
            go.SetActive(false);
            GameManager.Instance.GameStart();
        }

        public void SetScores()
        {
            player1Score.SetText(LoadData.Instance.Wins1().ToString());
            player2Score.SetText(LoadData.Instance.Wins2().ToString());
        }

        public void GameEndScreen(string playerName, int winner)
        {
            StopCoroutine(countPPS);

            if (winner == 1)
            {
                LoadData.Instance.Player1Wins();
                player1Score.SetText(LoadData.Instance.Wins1().ToString());
            }
            else
            {
                LoadData.Instance.Player2Wins();
                player2Score.SetText(LoadData.Instance.Wins2().ToString());
            }

            playerWins.SetActive(true);
            playerWins.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(playerName + " WINS!");
            playAgane.SetActive(true);
            quit.SetActive(true);
        }

        public void Restart()
        {
            SceneManager.LoadScene("TetrisMainScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        internal void SumLines1(int lines)
        {
            sumLines1 += lines;
            player1SumLines.SetText("LINES CLEARED: " + sumLines1);
            
        }

        internal void SumLines2(int lines)
        {
            sumLines2 += lines;
            player2SumLines.SetText("LINES CLEARED: " + sumLines2);
        }
    }
}

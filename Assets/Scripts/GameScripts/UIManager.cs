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

        [SerializeField]
        private TextMeshProUGUI player1Score;
        [SerializeField]
        private TextMeshProUGUI player2Score;
        [SerializeField]
        private TextMeshProUGUI player1LinesCleared;
        [SerializeField]
        private TextMeshProUGUI player2LinesCleared;
        [SerializeField]
        private TextMeshProUGUI player1PPS;
        [SerializeField]
        private TextMeshProUGUI player2PPS;
        [SerializeField]
        private Transform ui;
        [SerializeField]
        private GameObject ready;
        [SerializeField]
        private GameObject go;
        [SerializeField]
        private GameObject playerWins;
        [SerializeField]
        private GameObject playAgane;
        [SerializeField]
        private GameObject quit;

        public LoadData loadData;

        private int sumLinesCleared1;
        private int sumLinesCleared2;
        public int piecesPlaced1;
        public int piecesPlaced2;
        private IEnumerator countPPS;
        private float startTime;

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
                yield return new WaitForSeconds(0.01f);

                player1PPS.SetText("PIECES PER SECOND: " + String.Format("{0:0.00}", piecesPlaced1 / (Time.time - startTime)));
                player2PPS.SetText("PIECES PER SECOND: " + String.Format("{0:0.00}", piecesPlaced2 / (Time.time - startTime)));
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
            player1Score.SetText(loadData.score1.ToString());
            player2Score.SetText(loadData.score2.ToString());
        }

        public void GameEndScreen(string playerName, int winner)
        {
            StopCoroutine(countPPS);

            if (winner == 1)
            {
                loadData.score1++;
                player1Score.SetText(loadData.score1.ToString());
            }
            else
            {
                loadData.score2++;
                player2Score.SetText(loadData.score2.ToString());
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

        internal void SumLinesClearedPlayer1(int lines)
        {
            sumLinesCleared1 += lines;
            player1LinesCleared.SetText("LINES CLEARED: " + sumLinesCleared1);
            
        }

        internal void SumLinesClearedPlayer2(int lines)
        {
            sumLinesCleared2 += lines;
            player2LinesCleared.SetText("LINES CLEARED: " + sumLinesCleared2);
        }
    }
}

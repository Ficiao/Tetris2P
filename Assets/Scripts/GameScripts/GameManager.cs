using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public GameObject[] Tetrominos;
        public Transform spawner1;
        public Transform spawner2;
        public TetrominoController tetrominoController1;
        public TetrominoController tetrominoController2;
        public TetrisQueue queue1;
        public TetrisQueue queue2;

        public static GameManager Instance { get { return _instance; } }

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
            queue1.FillQueue();
            queue2.FillQueue();

            queue1.Next();
            queue2.Next();
        }

        internal void EndGame(string playerName)
        {
            if (string.Equals(tetrominoController1.playerName,playerName))
            {
                UIManager.Instance.GameEndScreen(tetrominoController2.playerName);
            }
            else
            {
                UIManager.Instance.GameEndScreen(tetrominoController1.playerName);
            }
            tetrominoController1.enabled = false;
            tetrominoController2.enabled = false;
        }
    }
}

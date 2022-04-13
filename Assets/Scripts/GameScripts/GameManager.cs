using System;
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
        private IEnumerator dropCoroutine;

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

        private void Start()
        {
            UIManager.Instance.GameStart();
        }

        public void GameStart()
        {
            queue1.FillQueue();
            queue2.FillQueue();

            queue1.Next();
            queue2.Next();

            dropCoroutine = DropTime();
            StartCoroutine(dropCoroutine);
        }

        private IEnumerator DropTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(.5f);
                tetrominoController1.DropOnTick();
                tetrominoController2.DropOnTick();
            }
        }

        internal void EndGame(string playerName)
        {
            StopCoroutine(dropCoroutine);
            
            if (string.Equals(tetrominoController1.playerName,playerName))
            {
                UIManager.Instance.GameEndScreen(tetrominoController2.playerName, 2);
            }
            else
            {
                UIManager.Instance.GameEndScreen(tetrominoController1.playerName, 1);
            }
            tetrominoController1.enabled = false;
            tetrominoController2.enabled = false;
        }

        public void SendGarbageLines(string playerName, int lines)
        {
            if (string.Equals(tetrominoController1.playerName, playerName))
            {
                for (int i = 0; i < lines; i++) 
                {
                    tetrominoController2.grid.CreateGarbage();
                }

                tetrominoController2.CheckLinesMovedAbovePiece();

                tetrominoController2.tetromino.GetComponent<Tetromino>().UpdateGhost();
            }
            else
            {
                for (int i = 0; i < lines; i++)
                {
                    tetrominoController1.grid.CreateGarbage();
                }

                tetrominoController2.CheckLinesMovedAbovePiece();

                tetrominoController1.tetromino.GetComponent<Tetromino>().UpdateGhost();
            }
        }

        public void SumLines(string playerName, int lines)
        {
            if (string.Equals(tetrominoController1.playerName, playerName))
            {
                UIManager.Instance.SumLines1(lines);
            }
            else
            {
                UIManager.Instance.SumLines2(lines);
            }
        }

        internal void SumPiece(string playerName)
        {
            if (string.Equals(tetrominoController1.playerName, playerName))
            {
                UIManager.Instance.pieces1++;
            }
            else
            {
                UIManager.Instance.pieces2++;
            }
        }
    }
}

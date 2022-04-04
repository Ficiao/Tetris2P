using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class TetrominoController : MonoBehaviour
    {
        public GameObject tetromino;
        public string left;
        public string right;
        public string down;
        public string drop;
        public string hold;
        public string rotateL;
        public string rotateR;
        public string playerName;
        private bool allowedToHold;

        private Transform tetrominoTransform;
        private Tetromino tetrominoStats;
        private Transform gridTransform;
        public Grid grid;
        public TetrisQueue queue;

        private void Start()
        {
            allowedToHold = true; 
        }

        internal void Set(GameObject created)
        {
            tetromino = created;
            tetrominoTransform = tetromino.transform;
            tetrominoStats = tetromino.GetComponent<Tetromino>();
            gridTransform = grid.transform;
        }

        void Update()
        {


            if (Input.GetKeyDown(left))
            {
                if (tetrominoTransform.localPosition.x > tetrominoStats.left)
                {
                    Vector2 pos = tetrominoTransform.position;
                    pos.x = tetrominoTransform.position.x - 1;
                    tetrominoTransform.position = pos;
                    tetrominoStats.UpdateGhost();
                }
            }

            if (Input.GetKeyDown(right))
            {
                if (tetrominoTransform.localPosition.x < grid.width - tetrominoStats.right)
                {
                    Vector2 pos = tetrominoTransform.position;
                    pos.x = tetrominoTransform.position.x + 1;
                    tetrominoTransform.position = pos;
                    tetrominoStats.UpdateGhost();
                }
            }

            if (Input.GetKeyDown(down))
            {
                int pass = 1;

                if (tetrominoTransform.localPosition.y <= tetrominoStats.down)
                {
                    tetrominoStats.LockTetromino();
                    SpawnNew();
                    pass = 0;
                }
                else if (pass == 1)
                {
                    foreach (Transform child in tetrominoStats.transform)
                    {
                        int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                        int y = (int)Mathf.Floor(child.position.y - transform.position.y);

                        if (!grid.Check(y - 1, x))
                        {
                            tetrominoStats.LockTetromino();
                            allowedToHold = true;
                            SpawnNew();
                            pass = 0;
                            break;
                        }
                    }
                }
                if (pass == 1)
                {
                    Vector2 pos = tetrominoTransform.position;
                    pos.y = tetrominoTransform.position.y - 1;
                    tetrominoTransform.position = pos;
                }


            }

            if (Input.GetKeyDown(drop))
            {
                int minY = int.MaxValue;

                foreach (Transform child in tetrominoStats.transform)
                {
                    int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                    int privY = (int)Mathf.Floor(child.position.y - transform.position.y) - grid.GetHighestY(x);
                    if (privY < minY)
                    {
                        minY = privY;
                    }

                }

                Vector2 pos = tetrominoTransform.position;
                pos.y = pos.y - minY;
                tetrominoTransform.position = pos;
                tetrominoStats.LockTetromino();
                allowedToHold = true;
                SpawnNew();
            }

            if (Input.GetKeyDown(hold))
            {
                if (allowedToHold)
                {
                    queue.Hold();
                    allowedToHold = false;
                }
            }

            if (Input.GetKeyDown(rotateL))
            {
                tetromino.transform.Rotate(0, 0, 90);

                int minY = int.MaxValue, minX = int.MaxValue, maxY = int.MinValue, maxX = int.MinValue;

                foreach (Transform child in tetrominoStats.transform)
                {
                    child.transform.Rotate(0, 0, -90);
                    int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                    if (minX > x) minX = x;
                    if (maxX < x) maxX = x;

                    int y = (int)Mathf.Floor(child.position.y - transform.position.y);
                    if (minY > y) minY = y;
                    if (maxY < y) maxY = y;
                }

                tetrominoStats.up = maxY - (int)tetromino.transform.localPosition.y + 1;
                tetrominoStats.down = (int)tetromino.transform.localPosition.y - minY;
                tetrominoStats.left = (int)tetromino.transform.localPosition.x - minX;
                tetrominoStats.right = maxX - (int)tetromino.transform.localPosition.x + 1;

                tetrominoStats.UpdateGhost();
            }

            if (Input.GetKeyDown(rotateR))
            {
                tetromino.transform.Rotate(0, 0, -90);

                int minY = int.MaxValue, minX = int.MaxValue, maxY = int.MinValue, maxX = int.MinValue;

                foreach (Transform child in tetrominoStats.transform)
                {
                    child.transform.Rotate(0, 0, 90);
                    int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                    if (minX > x) minX = x;
                    if (maxX < x) maxX = x;

                    int y = (int)Mathf.Floor(child.position.y - transform.position.y);
                    if (minY > y) minY = y;
                    if (maxY < y) maxY = y;
                }

                tetrominoStats.up = maxY - (int)tetromino.transform.localPosition.y + 1;
                tetrominoStats.down = (int)tetromino.transform.localPosition.y - minY;
                tetrominoStats.left = (int)tetromino.transform.localPosition.x - minX;
                tetrominoStats.right = maxX - (int)tetromino.transform.localPosition.x + 1;

                tetrominoStats.UpdateGhost();
            }

        }

        public void SpawnNew()
        {
            queue.Next();
        }
    }
}

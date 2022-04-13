using System;
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
        public TetrisGrid grid;
        public TetrisQueue queue;
        private int movedL;
        private int movedR;
        private float moveLTime;
        private float moveRTime;
        private float moveDTime;

        private void Start()
        {
            allowedToHold = true;
            movedL = 0;
            movedR = 0;
        }

        internal void Set(GameObject created)
        {
            tetromino = created;
            tetrominoTransform = tetromino.transform;
            tetrominoStats = tetromino.GetComponent<Tetromino>();
            gridTransform = grid.transform;
        }

        public void DropOnTick()
        {
            int pass = 1;

            if (tetrominoTransform.localPosition.y <= tetrominoStats.down)
            {
                tetrominoStats.LockTetromino();
                GameManager.Instance.SumPiece(this.playerName);
                SpawnNew();
                pass = 0;
            }
            else if (pass == 1)
            {
                foreach (Transform child in tetrominoStats.transform)
                {
                    int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                    int y = (int)Mathf.Floor(child.position.y - transform.position.y);

                    if (grid.CheckIfEmpty(y - 1, x) == false)
                    {
                        tetrominoStats.LockTetromino();
                        GameManager.Instance.SumPiece(this.playerName);
                        allowedToHold = true;
                        SpawnNew();
                        pass = 0;
                        break;
                    }
                }
            }
            if (pass == 1)
            {
                Vector2 position = tetrominoTransform.position;
                position.y = tetrominoTransform.position.y - 1;
                tetrominoTransform.position = position;
            }
        }

        void Update()
        {
            if (Input.GetKey(left))
            {
                if (tetrominoTransform.localPosition.x > tetrominoStats.left)
                {
                    if ((movedL == 0 && Input.GetKeyDown(left)) || (movedL == 1 && Time.time - moveLTime >= 0.23f) || (movedL == 2 && Time.time - moveLTime >= 0.013f)) 
                    {
                        if (movedL == 0)
                        {
                            movedL = 1;
                        }
                        else
                        {
                            movedL = 2;
                        }

                        moveLTime = Time.time;

                        bool pass = true;
                        foreach (Transform child in tetrominoStats.transform)
                        {
                            int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                            int y = (int)Mathf.Floor(child.position.y - transform.position.y);

                            if (grid.CheckIfEmpty(y, x - 1) == false)
                            {
                                pass = false;
                            }
                        }

                        if (pass)
                        {
                            Vector2 position = tetrominoTransform.position;
                            position.x = tetrominoTransform.position.x - 1;
                            tetrominoTransform.position = position;
                            tetrominoStats.UpdateGhost();
                        }

                        else
                        {
                            movedL = 0;
                        }
                    }
                }
                else
                {
                    movedL = 0;
                }
            }
            else
            {
                movedL = 0;
            }

            if (Input.GetKey(right))
            {                
                if (tetrominoTransform.localPosition.x < grid.width - tetrominoStats.right)
                {
                    if ((movedR == 0 && Input.GetKeyDown(right)) || (movedR == 1 && Time.time - moveRTime >= 0.23f) || (movedR == 2 && Time.time - moveRTime >= 0.013f)) 
                    {
                        if (movedR == 0)
                        {
                            movedR = 1;
                        }
                        else
                        {
                            movedR = 2;
                        }

                        moveRTime = Time.time;

                        bool pass = true;
                        foreach (Transform child in tetrominoStats.transform)
                        {
                            int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                            int y = (int)Mathf.Floor(child.position.y - transform.position.y);

                            if (grid.CheckIfEmpty(y, x + 1) == false)
                            {
                                pass = false;
                            }
                        }

                        if (pass)
                        {
                            Vector2 position = tetrominoTransform.position;
                            position.x = tetrominoTransform.position.x + 1;
                            tetrominoTransform.position = position;
                            tetrominoStats.UpdateGhost();
                        }
                        else
                        {
                            movedR = 0;
                        }
                    }
                }
                else
                {
                    movedR = 0;
                }
            }
            else
            {
                movedR = 0;
            }

            if (Input.GetKey(down))
            {
                bool pass = true;

                if (tetrominoTransform.localPosition.y <= tetrominoStats.down)
                {
                    tetrominoStats.LockTetromino();
                    GameManager.Instance.SumPiece(this.playerName);
                    SpawnNew();
                    pass = false;
                }
                else if (pass)
                {
                    foreach (Transform child in tetrominoStats.transform)
                    {
                        int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                        int y = (int)Mathf.Floor(child.position.y - transform.position.y);

                        if (grid.CheckIfEmpty(y - 1, x) == false)
                        {
                            tetrominoStats.LockTetromino();
                            GameManager.Instance.SumPiece(this.playerName);
                            allowedToHold = true;
                            SpawnNew();
                            pass = false;
                            break;
                        }
                    }
                }
                if (pass && Time.time - moveDTime >= 0.08f)
                {


                    moveDTime = Time.time; 

                    Vector2 position = tetrominoTransform.position;
                    position.y = tetrominoTransform.position.y - 1;
                    tetrominoTransform.position = position;
                }

            }

            if (Input.GetKeyDown(drop))
            {
                int minY = int.MaxValue;

                foreach (Transform child in tetrominoStats.transform)
                {
                    int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                    int y = (int)Mathf.Floor(child.position.y - transform.position.y);
                    int privY = (int)Mathf.Floor(child.position.y - transform.position.y) - grid.GetHighestY(y, x);
                    if (privY < minY)
                    {
                        minY = privY;
                    }

                }

                Vector2 position = tetrominoTransform.position;
                position.y = position.y - minY;
                tetrominoTransform.position = position;
                tetrominoStats.LockTetromino();
                allowedToHold = true;
                GameManager.Instance.SumPiece(this.playerName);
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
                if (string.Equals(tetrominoStats.tetName, "cube") == false)
                {
                    Rotate(90);
                    tetrominoStats.UpdateGhost();
                }
            }

            if (Input.GetKeyDown(rotateR))
            {
                if (string.Equals(tetrominoStats.tetName, "cube") == false)
                {
                    Rotate(-90);
                    tetrominoStats.UpdateGhost();
                }
            }

        }

        private void Rotate(float degrees)
        {
            RotateAroundPivot(degrees);

            bool rotated = true;
            if (CheckRotationAvailability() == false)
            {
                RotateAroundPivot(-degrees);
                rotated = false;
            }

            int minY = int.MaxValue, minX = int.MaxValue, maxY = int.MinValue, maxX = int.MinValue;

            foreach (Transform child in tetrominoStats.transform)
            {
                if (rotated)
                {
                    child.transform.Rotate(0, 0, -degrees);
                }
                int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                if (minX > x) minX = x;
                if (maxX < x) maxX = x;

                int y = (int)Mathf.Floor(child.position.y - transform.position.y);
                if (minY > y) minY = y;
                if (maxY < y) maxY = y;
            }

            tetrominoStats.up = maxY - (int)tetrominoTransform.localPosition.y + 1;
            tetrominoStats.down = (int)tetrominoTransform.localPosition.y - minY;
            tetrominoStats.left = (int)tetrominoTransform.localPosition.x - minX;
            tetrominoStats.right = maxX - (int)tetrominoTransform.localPosition.x + 1;
        }

        private bool CheckRotationAvailability()
        {
            Vector3 position = tetrominoTransform.position;
            Vector3 priv;

            if (CheckGrid())
            {
                return true;
            }

            priv = position;
            priv.x -= 1;
            tetrominoTransform.position = priv;

            if (CheckGrid())
            {
                return true;
            }

            priv = position;
            priv.x += 1;
            tetrominoTransform.position = priv;

            if (CheckGrid())
            {
                return true;
            }

            priv = position;
            priv.y -= 1;
            tetrominoTransform.position = priv;

            if (CheckGrid())
            {
                return true;
            }

            priv = position;
            priv.x += 1;
            tetrominoTransform.position = priv;

            if (CheckGrid())
            {
                return true;
            }

            if (string.Equals(tetrominoStats.tetName, "long") == true)
            {
                priv = position;
                priv.x += 2;
                tetrominoTransform.position = priv;

                if (CheckGrid())
                {
                    return true;
                }

                priv = position;
                priv.x -= 2;
                tetrominoTransform.position = priv;

                if (CheckGrid())
                {
                    return true;
                }
            }

            tetrominoTransform.position = position;
            return false;
        }

        private bool CheckGrid()
        {
            bool available = true;
            foreach (Transform child in tetrominoTransform)
            {
                int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                int y = (int)Mathf.Floor(child.position.y - transform.position.y);
                if (y >= grid.height)
                {
                    if (x >= grid.width || x < 0)
                    {
                        available = false;
                    }
                }
                else
                {
                    if (x >= 0 && x < grid.width && y >= 0)
                    {
                        if (grid.CheckIfEmpty(y, x) == false)
                        {
                            available = false;
                        }
                    }
                    else
                    {
                        available = false;
                    }
                }
            }

            return available;
        }

        private void RotateAroundPivot(float degrees)
        {
            if (string.Equals(tetrominoStats.tetName, "long") == false)
            {
                Vector3 position = tetrominoTransform.position;
                position.x -= 0.5f;
                position.y -= 0.5f;
                tetrominoTransform.position = position;
                foreach (Transform child in tetrominoTransform)
                {
                    position = child.position;
                    position.x += 0.5f;
                    position.y += 0.5f;
                    child.position = position;
                }

            }

            tetrominoTransform.Rotate(0, 0, degrees);

            if (string.Equals(tetrominoStats.tetName, "long") == false)
            {
                Vector3 position = tetrominoTransform.position;
                position.x += 0.5f;
                position.y += 0.5f;
                tetrominoTransform.position = position;
                foreach (Transform child in tetrominoTransform)
                {
                    position = child.position;
                    position.x -= 0.5f;
                    position.y -= 0.5f;
                    child.position = position;
                }

            }
        }

        public void SpawnNew()
        {
            queue.Next();

            foreach (Transform child in tetrominoTransform)
            {
                int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                int y = (int)Mathf.Floor(child.position.y - transform.position.y);
                if (grid.CheckIfEmpty(y, x) == false)
                {
                    GameManager.Instance.EndGame(playerName);
                    foreach (Transform child2 in tetrominoTransform)
                    {
                        Destroy(child2.gameObject);
                    }
                    break;
                }
            }
        }


        internal void CheckLinesMovedAbovePiece()
        {

            foreach (Transform child in tetrominoTransform)
            {
                int x = (int)Mathf.Floor(child.position.x - transform.position.x);
                int y = (int)Mathf.Floor(child.position.y - transform.position.y);
                if (grid.CheckIfEmpty(y, x) == false)
                {
                    int minY = int.MinValue;

                    foreach (Transform child2 in tetrominoStats.transform)
                    {
                        x = (int)Mathf.Floor(child2.position.x - transform.position.x);
                        y = (int)Mathf.Floor(child2.position.y - transform.position.y);
                        int privY = (int)transform.position.y + grid.GetHighestY(grid.height*2, x);
                        if (privY > minY)
                        {
                            minY = privY;
                        }

                    }

                    Vector2 position = tetrominoTransform.position;
                    position.y = minY+1;
                    tetrominoTransform.position = position;

                    break;
                }
            }
        }

    }
}

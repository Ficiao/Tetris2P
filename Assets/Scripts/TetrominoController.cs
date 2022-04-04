using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Transform tetrominoTransform;
    private Tetromino tetrominoStats;
    private Transform gridTransform;
    private Grid gridStats;

    internal void Set(GameObject created)
    {
        tetromino = created;
        tetrominoTransform = tetromino.transform;
        tetrominoStats = tetromino.GetComponent<Tetromino>();
        gridTransform = GameManager.Instance.grid.transform;
        gridStats = GameManager.Instance.grid;
    }

    void Update()
    {
        

        if (Input.GetKeyDown(left))
        {
            if (tetrominoTransform.position.x > tetrominoStats.left)
            {
                Vector2 pos = tetrominoTransform.position;
                pos.x = tetrominoTransform.position.x - 1;
                tetrominoTransform.position = pos;
            }
        }

        if (Input.GetKeyDown(right))
        {
            if (tetrominoTransform.position.x < gridStats.width-tetrominoStats.right)
            {
                Vector2 pos = tetrominoTransform.position;
                pos.x = tetrominoTransform.position.x + 1;
                tetrominoTransform.position = pos;
            }
        }

        if (Input.GetKeyDown(down))
        {
            int pass = 1;

            if (tetrominoTransform.position.y <= tetrominoStats.down)
            {
                tetrominoStats.LockTetromino();
                pass = 0;
            }
            else if (pass == 1)
            {
                foreach (Transform child in tetrominoStats.transform)
                {
                    int x = (int)Mathf.Floor(child.position.x);
                    int y = (int)Mathf.Floor(child.position.y);

                    if (!gridStats.Check(y - 1, x))
                    {
                        tetrominoStats.LockTetromino();
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
                int x = (int)Mathf.Floor(child.position.x);
                int privY = (int)Mathf.Floor(child.position.y)-gridStats.GetHighestY(x);
                if (privY < minY)
                {
                    minY = privY;
                }
                   
            }

            Vector2 pos = tetrominoTransform.position;
            pos.y = pos.y - minY;
            tetrominoTransform.position = pos;
            tetrominoStats.LockTetromino();
        }

        if (Input.GetKeyDown(hold))
        {

        }

        if (Input.GetKeyDown(rotateL))
        {
            tetromino.transform.Rotate(0, 0, -90);

            int minY = int.MaxValue, minX = int.MaxValue, maxY = int.MinValue, maxX = int.MinValue;

            foreach (Transform child in tetrominoStats.transform)
            {
                child.transform.Rotate(0, 0, 90);
                int x = (int)Mathf.Floor(child.position.x);
                if (minX > x) minX = x;
                if (maxX < x) maxX = x;

                int y = (int)Mathf.Floor(child.position.y);
                if (minY > y) minY = y;
                if (maxY < y) maxY = y;
            }

            tetrominoStats.up = maxY - (int)tetromino.transform.position.y + 1;
            tetrominoStats.down = (int)tetromino.transform.position.y - minY;
            tetrominoStats.left = (int)tetromino.transform.position.x - minX;
            tetrominoStats.right = maxX - (int)tetromino.transform.position.x + 1;



        }

        if (Input.GetKeyDown(rotateR))
        {
            tetromino.transform.Rotate(0, 0, 90);

            int minY = int.MaxValue, minX = int.MaxValue, maxY = int.MinValue, maxX = int.MinValue;

            foreach (Transform child in tetrominoStats.transform)
            {
                child.transform.Rotate(0, 0, -90);
                int x = (int)Mathf.Floor(child.position.x);
                if (minX > x) minX = x;
                if (maxX < x) maxX = x;

                int y = (int)Mathf.Floor(child.position.y);
                if (minY > y) minY = y;
                if (maxY < y) maxY = y;
            }

            tetrominoStats.up = maxY - (int)tetromino.transform.position.y + 1;
            tetrominoStats.down = (int)tetromino.transform.position.y - minY;
            tetrominoStats.left = (int)tetromino.transform.position.x - minX;
            tetrominoStats.right = maxX - (int)tetromino.transform.position.x + 1;
        }

    }
}

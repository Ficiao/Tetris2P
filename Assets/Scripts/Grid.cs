using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int height = 20;
    public int width = 10;

    private Transform[,] matrix; 

    private void Start()
    {
        matrix = new Transform[height, width];
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                matrix[i, j] = null;
            }
        }
    }

    public void Add(Transform tetromino, int y, int x)
    {
        matrix[y, x] = tetromino;
    }

    internal void Check(int y)
    {
        for (int i = 0; i < width; i++)
        {
            if (matrix[y, i] == null)
            {
                return;
            }
        }
        for (int i = y; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (i == y)
                {
                    if (matrix[i, j] != null)
                    {
                        Destroy(matrix[i, j].gameObject);
                    }
                }
                else
                {
                    if (matrix[i, j] != null)
                    {
                        Vector3 priv = matrix[i, j].transform.position;
                        priv.y -= 1;
                        matrix[i, j].transform.position = priv;
                    }
                    matrix[i - 1, j] = matrix[i, j];
                }
            }
        }
    }

    public bool Check(int y, int x)
    {
        if (matrix[y, x] == null)
        {
            return true;
        }

        return false;
    }

    public int GetHighestY(int x)
    {
        for(int i = height-1; i >= 0; i--)
        {
            if (matrix[i, x] != null)
            {
                return i+1;
            }
        }

        return 0;
    }
}

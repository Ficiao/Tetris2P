using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public int left;
    public int right;
    public int up;
    public int down;

    public GameObject ghostParent;
    public GameObject ghostChildren;

    private List<Transform> children;
    private List<int> ys;
    GameObject ghosting;

    private void Start()
    {
        ghosting = Instantiate(ghostParent, transform.position, Quaternion.identity, this.transform.parent);
        foreach (Transform child in transform)
        {
            Instantiate(ghostChildren, child.position, Quaternion.identity, ghosting.transform);
        }
    }

    private void Update()
    {
        int minY = int.MaxValue;

        ghosting.transform.position = this.transform.position;
        for (int i = 0; i < ghosting.transform.childCount; i++)
        {
            ghosting.transform.GetChild(i).position = this.transform.GetChild(i).position;
        }

        foreach (Transform child in transform)
        {
            int x = (int)Mathf.Floor(child.position.x);
            int privY = (int)Mathf.Floor(child.position.y) - GameManager.Instance.grid.GetHighestY(x);
            if (privY < minY)
            {
                minY = privY;
            }

        }

        ghosting.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - minY, -1f);
    }

    public void LockTetromino()
    {
        children = new List<Transform>();
        ys = new List<int>();
        foreach(Transform child in transform)
        {
            children.Add(child);
            int x = (int) Mathf.Floor(child.position.x);
            int y = (int) Mathf.Floor(child.position.y);
            if (!ys.Contains(y))
            {
                ys.Add(y);
            }
            Grid priv = GameManager.Instance.grid;
            priv.Add(child.transform, y, x);

        }

        foreach(Transform child in children)
        {
            child.parent = this.transform.parent;
        }

        foreach(int y in ys)
        {
            GameManager.Instance.grid.Check(y);
        }

        foreach(Transform child in ghosting.transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(ghosting.gameObject);

        GameManager.Instance.SpawnNew();
        Destroy(gameObject);
    }
}

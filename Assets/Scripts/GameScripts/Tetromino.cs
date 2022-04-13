using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class Tetromino : MonoBehaviour
    {
        public int left;
        public int right;
        public int up;
        public int down;
        public string tetName;

        public GameObject ghostParent;
        public GameObject ghostChildren;

        private List<Transform> children;
        public GameObject ghosting;

        private void Update()
        {
            foreach (Transform child in transform)
            {
                if (child.transform.position.y - transform.parent.position.y >= 20)
                {
                    child.gameObject.SetActive(false);
                }
                else if (child.gameObject.activeSelf == false)
                {
                    child.gameObject.SetActive(true);
                }
            }
            
        }

        public void UpdateGhost()
        {
            if (ghosting == null)
            {
                ghosting = Instantiate(ghostParent, transform.position, Quaternion.identity, this.transform.parent);
                foreach (Transform child in transform)
                {
                    Instantiate(ghostChildren, child.position, Quaternion.identity, ghosting.transform);
                }
            }

            int minY = int.MaxValue;

            ghosting.transform.position = this.transform.position;
            for (int i = 0; i < ghosting.transform.childCount; i++)
            {
                ghosting.transform.GetChild(i).position = this.transform.GetChild(i).position;
            }

            foreach (Transform child in transform)
            {
                int x = (int)(Mathf.Floor(child.position.x) - this.transform.parent.position.x);
                int y = (int)(Mathf.Floor(child.position.y) - this.transform.parent.position.y);
                if (x < 0)
                {
                    x = 0;
                }
                if (x > 9)
                {
                    x = 9;
                }
                int privY = (int)Mathf.Floor(child.position.y) - GetComponentInParent<TetrominoController>().grid.GetHighestY(y, x);
                if (privY < minY)
                {
                    minY = privY;
                }

            }

            ghosting.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - minY, -1f);

            foreach (Transform child in ghosting.transform)
            {
                if (child.transform.position.y - transform.parent.position.y >= 20)
                {
                    child.gameObject.SetActive(false);
                }
                else if (child.gameObject.activeSelf == false)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        public void LockTetromino()
        {
            children = new List<Transform>();
            foreach (Transform child in transform)
            {
                if (child.transform.position.y - transform.parent.position.y >= 20)
                {
                    child.gameObject.SetActive(false);
                }
                else if (child.gameObject.activeSelf == false)
                {
                    child.gameObject.SetActive(true);
                }

                children.Add(child);
                int x = (int)Mathf.Floor(child.position.x - transform.parent.position.x);
                int y = (int)Mathf.Floor(child.position.y - transform.parent.position.y);

                TetrisGrid priv = GetComponentInParent<TetrominoController>().grid;
                priv.Add(child.transform, y, x);

            }

            foreach (Transform child in children)
            {
                child.parent = this.transform.parent;
            }

            foreach (Transform child in ghosting.transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(ghosting.gameObject);
            Destroy(gameObject);

            GetComponentInParent<TetrominoController>().grid.CheckLines();
        }

    }
}

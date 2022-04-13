using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class TetrisQueue : MonoBehaviour
    {
        private List<GameObject> pieces;
        public Transform hold;
        private GameObject held;
        public GameObject[] tetrominos;
        public Transform spawner;
        public TetrominoController controller;
        bool full;

        public void FillQueue()
        {
            full = false;
            pieces = new List<GameObject>();
            for (int i = 0; i < 4; i++)
            {
                int random = Random.Range(0, tetrominos.Length);
                GameObject created = Instantiate(tetrominos[random], new Vector3(transform.position.x, transform.position.y - (i * 3), -3), Quaternion.identity, spawner.transform.parent);
                pieces.Add(created.gameObject);
            }
        }

        public void Next()
        {
            controller.Set(pieces[0]);
            pieces[0].transform.position = spawner.position;
            pieces[0].GetComponent<Tetromino>().UpdateGhost();
            pieces.RemoveAt(0);

            for (int i = 0; i < 3; i++)
            {
                pieces[i].transform.position = new Vector3(pieces[i].transform.position.x, pieces[i].transform.position.y + 3, pieces[i].transform.position.z);
            }
            int random = Random.Range(0, tetrominos.Length);
            GameObject created = Instantiate(tetrominos[random], new Vector3(transform.position.x, transform.position.y - (3 * 3), -3), Quaternion.identity, spawner.transform.parent);
            pieces.Add(created.gameObject);
        }

        public void Hold()
        {
            if (full)
            {
                foreach (Transform child in controller.tetromino.GetComponent<Tetromino>().ghosting.transform)
                {
                    Destroy(child.gameObject);
                }
                Destroy(controller.tetromino.GetComponent<Tetromino>().ghosting.gameObject);

                GameObject priv = held;
                priv.transform.position = spawner.position;

                foreach (Transform child in controller.tetromino.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (GameObject piece in tetrominos)
                {
                    if (string.Equals(piece.GetComponent<Tetromino>().tetName, controller.tetromino.GetComponent<Tetromino>().tetName))
                    {
                        Destroy(controller.tetromino.gameObject);
                        held = Instantiate(piece, hold.position, Quaternion.identity, spawner.transform.parent);
                    }
                }

                controller.Set(priv);
                priv.GetComponent<Tetromino>().UpdateGhost();
                
            }

            else
            {
                foreach (Transform child in controller.tetromino.GetComponent<Tetromino>().ghosting.transform)
                {
                    Destroy(child.gameObject);
                }
                Destroy(controller.tetromino.GetComponent<Tetromino>().ghosting.gameObject);

                foreach (Transform child in controller.tetromino.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (GameObject piece in tetrominos)
                {
                    if(string.Equals(piece.GetComponent<Tetromino>().tetName, controller.tetromino.GetComponent<Tetromino>().tetName))
                    {
                        Destroy(controller.tetromino.gameObject);
                        held = Instantiate(piece, hold.position, Quaternion.identity, spawner.transform.parent);
                    }
                }

                this.Next();
                full = true;
            }
        }
    }
}

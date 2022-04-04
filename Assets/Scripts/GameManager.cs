using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public GameObject[] Tetrominos;
    public Transform spawner;
    public TetrominoController tetrominoController;
    public Grid grid;

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
        SpawnNew();
    }

    public void SpawnNew()
    {
        int random = Random.Range(0, Tetrominos.Length);
        GameObject created= Instantiate(Tetrominos[random], spawner.position, Quaternion.identity, spawner.transform.parent);
        tetrominoController.Set(created);

    }
}

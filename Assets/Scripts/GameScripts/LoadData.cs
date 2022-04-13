using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class LoadData : MonoBehaviour
    {

        public static LoadData _instance;

        public int score1;
        public int score2;

        public static LoadData Instance { get { return _instance; } }

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

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            score1 = 0;
            score2 = 0;
        }

        public int Wins1()
        {
            return score1;
        }

        public int Wins2()
        {
            return score2;
        }

        public void Player1Wins()
        {
            score1 += 1;
        }

        public void Player2Wins()
        {
            score2 += 1;
        }
    }
}

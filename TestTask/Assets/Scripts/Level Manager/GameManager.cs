using System;
using UnityEngine;

namespace Level_Manager
{
    public class GameManager : MonoBehaviour
    {
        public bool gameOver;
        public event Action OnGameOver;

        private Player.Player _player;

        private void Awake()
        {
            _player = FindObjectOfType<Player.Player>();
        }

        private void Update()
        {
            if (gameOver)
            {
                OnGameOver?.Invoke();
            }
        }
    }
}
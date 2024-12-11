using System;
using Level_Manager;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float turnSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private TextMeshProUGUI hintText;
        [SerializeField] private GameManager _gameManager;
        private Animator _playerAnimator;

        public bool died;
        public int money;
        public bool gameStarted;

        private void OnEnable()
        {
            _gameManager.OnGameOver += OnDie;
        }

        private void Awake()
        {
            hintText.enabled = true;
            _playerAnimator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (!died && gameStarted)
            {
                HandleTurning();
                HandleMovement();
            }

            if (money <= 0)
            {
                _gameManager.gameOver = true;
            }

            if (gameStarted)
            {
                _playerAnimator.SetBool("GameStarted", true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
                hintText.enabled = false;
            }
        }

        private void HandleTurning()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.down, turnSpeed * Time.deltaTime);
                transform.Translate(Vector3.left * (walkSpeed * Time.deltaTime));
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                transform.Translate(Vector3.right * (walkSpeed * Time.deltaTime));
            }
        }

        private void HandleMovement()
        {
            if (!died)
            {
                transform.Translate(Vector3.forward * (walkSpeed * Time.deltaTime));
            }
        }

        private void OnDie()
        {
            died = true;
            _playerAnimator.SetBool("GameOver", true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Bill"))
            {
                var randomAmount = Random.Range(0, 4);
                money += randomAmount;
            }
            else if (other.gameObject.CompareTag("Bottle"))
            {
                money -= 20;
            }
        }

        private void OnDisable()
        {
            _gameManager.OnGameOver -= OnDie;
        }
    }
}
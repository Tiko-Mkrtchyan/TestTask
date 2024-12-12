using System;
using System.Collections;
using Level_Manager;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float turnSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private TextMeshProUGUI hintText;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip coinSound;
        [SerializeField] private AudioClip bottleSound;
        [SerializeField] private AudioClip stepSound;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI bottleText;
        [SerializeField] private GameObject avatarCasual;
        [SerializeField] private GameObject avatarMiddle;
        [SerializeField] private GameObject avatarBling;

        private Animator _playerAnimator;
        private event Action OnMoneyCollected;
        private event Action OnBottleCollected;
        private readonly float _stepSoundCooldown = 0.7f;
        private float _stepSoundTimer = 0f;
        private Vector2 _coinTextStartPosition;
        private Vector2 _bottleTextPosition;


        public bool died;
        public int money;
        public bool gameStarted;


        private void OnEnable()
        {
            gameManager.OnGameOver += OnDie;
            OnMoneyCollected += CollectMoney;
            OnBottleCollected += CollectBottle;
        }

        private void Awake()
        {
            _coinTextStartPosition=coinText.rectTransform.anchoredPosition;
            _bottleTextPosition = bottleText.rectTransform.anchoredPosition;
            coinText.gameObject.SetActive(false);
            bottleText.gameObject.SetActive(false);
            hintText.enabled = true;
            _playerAnimator = GetComponent<Animator>();
            avatarBling.SetActive(false);
            avatarMiddle.SetActive(false);
            avatarCasual.SetActive(true);
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
                gameManager.gameOver = true;
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

                if (_stepSoundTimer <= 0f)
                {
                    PlayStepSound();
                    _stepSoundTimer = _stepSoundCooldown;
                }
                else
                {
                    _stepSoundTimer -= Time.deltaTime;
                }
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
                OnMoneyCollected?.Invoke();
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Bottle"))
            {
                OnBottleCollected?.Invoke();
                Destroy(other.gameObject);
            }

            if (other.gameObject.CompareTag("Door"))
            {
                other.GetComponent<Animator>().enabled = true;
            }
        }

        private void CollectMoney()
        {
            var randomAmount = Random.Range(2, 6);
            money += randomAmount;
            PlayAudio(coinSound);
            coinText.gameObject.SetActive(true);
            coinText.text = "+ " + randomAmount + " $";
            coinText.rectTransform.DOAnchorPosY(coinText.rectTransform.anchoredPosition.y + 30, 0.5f);
            StartCoroutine(WaitToDestroy(coinText.gameObject));
            coinText.rectTransform.anchoredPosition = _coinTextStartPosition;
            if (money>=60)
            {
                avatarCasual.SetActive(false);
                avatarMiddle.SetActive(true);
            }

            if (money>=90)
            {
                avatarMiddle.SetActive(false);
                avatarBling.SetActive(true);
            }
        }

        private void CollectBottle()
        {
            money -= 20;
            PlayAudio(bottleSound);
            bottleText.gameObject.SetActive(true);
            bottleText.text = "- 20 $";
            bottleText.rectTransform.DOAnchorPosY(bottleText.rectTransform.anchoredPosition.y + 30, 0.5f);
            StartCoroutine(WaitToDestroy(bottleText.gameObject));
            bottleText.rectTransform.anchoredPosition = _bottleTextPosition;
        }

        private IEnumerator WaitToDestroy(GameObject objectToDestroy)
        {
            yield return new WaitForSeconds(1f);
            objectToDestroy.SetActive(false);
        }

        private void PlayAudio(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        private void PlayStepSound()
        {
            if (stepSound != null)
            {
                audioSource.clip = stepSound;
                audioSource.Play();
            }
        }

        private void OnDisable()
        {
            gameManager.OnGameOver -= OnDie;
            OnMoneyCollected -= CollectMoney;
            OnBottleCollected -= CollectBottle;
        }
    }
}
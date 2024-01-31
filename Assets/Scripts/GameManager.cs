using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int playerScore = 0;
    [SerializeField] private int coinAmount = 0;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject shop;
    [SerializeField] private WindSpawner windSpawner;
    [SerializeField] private GameObject movementSquad;
    [SerializeField] private float gameSpeedMoving = 10f;

    private bool gameOver = false;
    private bool started = false;
    private bool playedSound = false;
    private float gameSpeed = 0f;
    private bool muted = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOver = false;
        playerScore = 0;
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        coinAmount = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = coinAmount.ToString();
    }

    private void FixedUpdate()
    {
        if (started)
        {
            movementSquad.transform.position += Vector3.up * gameSpeed * Time.fixedDeltaTime;
        }
    }

    public void AddScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        scoreText.text = playerScore.ToString();

        if (playerScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", playerScore);
            highScoreText.text = playerScore.ToString();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ReturnShop()
    {
        windSpawner.SetCanSpawnWind(false);
        shop.SetActive(true);
        startScreen.SetActive(false);
    }

    public void ReturnStartScreen()
    {
        windSpawner.SetCanSpawnWind(true);
        shop.SetActive(false);
        startScreen.SetActive(true);
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverScreen.SetActive(true);
        PlaySoundEffect();
        windSpawner.SetCanSpawnWind(false);
    }

    private void PlaySoundEffect()
    {
        if (!playedSound && !muted)
        {
            audioSource.Play();
            playedSound = true;
        }
    }

    public void StartGame()
    {
        gameSpeed = gameSpeedMoving;
        started = true;
        startScreen.SetActive(false);
    }

    public bool IsStarted()
    {
        return started;
    }

    public void AddCoin(int coins)
    {
        coinAmount += coins;
        coinText.text = coinAmount.ToString();
        PlayerPrefs.SetInt("Coins", coinAmount);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        highScoreText.text = "0";
    }

    public float GetGameSpeed()
    {
        return gameSpeed;
    }

    public void Mute()
    {
        muted = !muted;
        windSpawner.SetMuted(muted);
    }
}

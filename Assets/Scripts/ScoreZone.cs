using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("ScoreZone: GameManager not found in scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flower") && gameManager != null && !gameManager.IsGameOver())
        {
            gameManager.AddScore(1);
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class Killzone : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flower"))
        {
            gameManager.GameOver();
        }
    }
}

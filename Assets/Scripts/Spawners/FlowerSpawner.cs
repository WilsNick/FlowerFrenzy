using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject flowerPrefab; // The prefab to spawn
    [SerializeField] private GameObject cloudPrefab; // The prefab to spawn
    [SerializeField] private Transform spawnPoint; // The point to spawn at
    [SerializeField] private Transform movementSquad; // The squad to move with
    [SerializeField] private GameManager gameManager;

    private GameObject cloud;

    private void Start()
    {
        SpawnFlower();
        SpawnCloud();
    }

    private void FixedUpdate()
    {
        if (gameManager.IsStarted() && cloud != null)
        {
            var color = cloud.GetComponent<SpriteRenderer>().color;
            color.a -= 0.05f * gameManager.GetGameSpeed() * Time.fixedDeltaTime;
            cloud.GetComponent<SpriteRenderer>().color = color;
            Destroy(cloud.GetComponent<Collider2D>());
        }
    }

    private void SpawnFlower()
    {
        Instantiate(flowerPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void SpawnCloud()
    {
        cloud = Instantiate(cloudPrefab, spawnPoint.position, Quaternion.identity, movementSquad);
    }
}

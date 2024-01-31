using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderSpawner : MonoBehaviour
{
    [SerializeField] private GameObject borderPrefab;
    [SerializeField] private Transform movementSquad;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float spawnRate = 2f;
    private float spawnTimer = 0f;
    private bool isGameStarted = false;

    private void Start()
    {
        float height = borderPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;
        spawnRate = height / gameManager.GetGameSpeed();
        SpawnBorder();
    }

    private void FixedUpdate()
    {
        if (!isGameStarted && gameManager.IsStarted())
        {
            Debug.Log("got there");
            float height = borderPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.y;

            isGameStarted = true;
            spawnRate = height / gameManager.GetGameSpeed();
            
            spawnTimer = Time.fixedDeltaTime;
        }

        if (isGameStarted)
        {
            spawnTimer += Time.fixedDeltaTime;

            if (spawnTimer >= spawnRate)
            {
                SpawnBorder();
                spawnTimer = Time.fixedDeltaTime;
            }
        }
    }

    private void SpawnBorder()
    {
        var position1 = movementSquad.GetChild(movementSquad.childCount - 1).GetChild(0).position;
        var position2 = movementSquad.GetChild(movementSquad.childCount - 2).GetChild(0).position;
        position1.y += position1.y - position2.y;
        Instantiate(borderPrefab, position1, Quaternion.identity, movementSquad);
    }
}

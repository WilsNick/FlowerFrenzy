using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs; // The array of prefabs to spawn
    [SerializeField] private GameObject scoreArea; // The prefab to spawn
    [SerializeField] private GameObject flower; // The prefab to spawn
    [SerializeField] private GameObject movementSquad; // The prefab to spawn
    [SerializeField] private float spawnRate = 2f; // The rate at which to spawn the prefab
    [SerializeField] private GameObject[] branchLimits; // The array of minimum and maximum branch limits
    [SerializeField] private GameObject[] coins; // The array of coin prefabs
    [SerializeField] private float openingWidth = 3.0f; // The width of the gap between branches
    [SerializeField] private float coinSpawnChance = 0.1f; // The chance of spawning a coin
    [SerializeField] private GameObject minCoin; // The minimum coin prefab
    private float spawnTimer = 0f; // Timer for spawning the prefab
    public GameManager gameManager;
    private int lastBranch = -1;
    private bool secondTime = false;

    // Start is called before the first frame update
    private void Start()
    {
        SpawnObject();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameManager.IsStarted())
        {
            // If the spawn timer has reached the spawn rate, spawn the prefab
            if (spawnTimer >= spawnRate)
            {
                SpawnObject();
                spawnTimer = 0f; // Reset the spawn timer
            }
            else
            {
                spawnTimer += Time.fixedDeltaTime; // Increment the spawn timer
            }
        }
    }

    // Spawns the prefab at a random position within the spawn radius

    private void SpawnObject()
    {
        GameObject parentObject = new GameObject("branchInstance");
        parentObject.transform.SetParent(movementSquad.transform);
        // Determine the limits for branch spawning
        float minX = branchLimits[0].transform.position.x;
        float maxX = branchLimits[1].transform.position.x - openingWidth * flower.GetComponent<Renderer>().bounds.size.x;
        float third = (maxX - minX) / 3;

        float randomIndex = DetermineBranchLimits(minX, maxX, third);

        GameObject[] BranchPrefabs = SelectBranchPrefab(randomIndex, minX, third);

        CreateFirstBranch(parentObject, BranchPrefabs[0], randomIndex);
        CreateLastBranch(parentObject, BranchPrefabs[1], randomIndex);
        CreateScore(parentObject);

        CheckCoinSpawnChance(randomIndex, BranchPrefabs[1]);
    }

    private float DetermineBranchLimits(float minX, float maxX, float third)
    {
        // Determine the index of the branch to spawn
        float randomIndex = 0;
        if (secondTime)
        {
            switch (lastBranch)
            {
                case 1:
                    randomIndex = Random.Range(minX + third, maxX);
                    break;
                case 2:
                    if (Random.value < 0.5f)
                    {
                        randomIndex = Random.Range(minX, minX + third);
                    }
                    else
                    {
                        randomIndex = Random.Range(minX + 2 * third, maxX);
                    }
                    break;
                case 3:
                    randomIndex = Random.Range(minX, minX + 2 * third);
                    break;
            }
            secondTime = false;
            lastBranch = -1;
        }
        else
        {
            randomIndex = Random.Range(minX, maxX);
        }
        return randomIndex;
    }

    private GameObject[] SelectBranchPrefab(float randomIndex, float minX, float third)
    {
        GameObject[] BranchPrefabs = { null, null };
        if (randomIndex < minX + third)
        {
            BranchPrefabs[0] = obstaclePrefabs[0];
            BranchPrefabs[1] = obstaclePrefabs[2];
            if (lastBranch == 1)
            {
                secondTime = true;
            }
            lastBranch = 1;
        }
        else if (randomIndex < minX + 2 * third)
        {
            // Debug.Log("med");
            BranchPrefabs[0] = obstaclePrefabs[1];
            BranchPrefabs[1] = obstaclePrefabs[1];
            if (lastBranch == 2)
            {
                secondTime = true;
            }
            lastBranch = 2;
        }
        else
        {
            // Debug.Log("large");
            BranchPrefabs[0] = obstaclePrefabs[2];
            BranchPrefabs[1] = obstaclePrefabs[0];
            if (lastBranch == 3)
            {
                secondTime = true;
            }
            lastBranch = 3;
        }
        return BranchPrefabs;
    }

    private void CreateFirstBranch(GameObject parentObject, GameObject firstBranchPrefab, float randomIndex)
    {
        var size = firstBranchPrefab.GetComponent<Renderer>().bounds.size.x;
        var fillsize = randomIndex - transform.position.x;
        var changeby = fillsize / size;
        var pos = transform.position + Vector3.right * fillsize / 2;

        GameObject first_branches = Instantiate(firstBranchPrefab, pos, Quaternion.identity, parentObject.transform);
        first_branches.transform.localScale = new Vector3(changeby, first_branches.transform.localScale.y, 1);
    }

    private void CreateLastBranch(GameObject parentObject, GameObject lastBranchPrefab, float randomIndex)
    {
        var size = lastBranchPrefab.GetComponent<Renderer>().bounds.size.x;
        var fillsize = -transform.position.x - (randomIndex + openingWidth * flower.GetComponent<Renderer>().bounds.size.x);
        var changeby = fillsize / size;
        var lastpos = new Vector3(randomIndex + openingWidth * flower.GetComponent<Renderer>().bounds.size.x, transform.position.y, 0);
        var pos = (lastpos) + Vector3.right * fillsize / 2;

        GameObject last_branches = Instantiate(lastBranchPrefab, pos, Quaternion.identity, parentObject.transform);
        last_branches.transform.localScale = new Vector3(changeby, last_branches.transform.localScale.y, 1);
        last_branches.GetComponent<SpriteRenderer>().flipX = true;
    }

    private void CreateScore(GameObject parentObject)
    {
        var scorePlace = new Vector3(0, transform.position.y, 0);
        GameObject score = Instantiate(scoreArea, scorePlace, Quaternion.identity, parentObject.transform);
        score.transform.localScale = new Vector3((2.0f / 6.0f) * (-transform.position.x), 0.5f, 1);
    }

    private void CheckCoinSpawnChance(float randomIndex, GameObject branch)
    {
        if (!gameManager.IsGameOver())
        {
            float chance = Random.Range(0f, 1f);
            if (chance < coinSpawnChance)
            {
                SpawnCoin(randomIndex, randomIndex + openingWidth * flower.GetComponent<Renderer>().bounds.size.x, branch.GetComponent<Renderer>().bounds.size.y);
            }
        }
    }


    private void SpawnCoin(float firstBranch, float finalBranch, float branchSize)
    {
        float coinPosX = CalculateCoinXPosition(firstBranch, finalBranch);
        float coinPosY = CalculateCoinYPosition(branchSize);
        GameObject coinToSpawn = ChooseRandomCoinPrefab();
        Vector3 spawnPos = new Vector3(coinPosX, coinPosY, 0f);
        GameObject newCoin = Instantiate(coinToSpawn, spawnPos, Quaternion.identity, movementSquad.transform);
    }

    private float CalculateCoinXPosition(float firstBranch, float finalBranch)
    {
        float coinOffset = coins[0].GetComponent<Renderer>().bounds.size.x / 2;
        float minPosX = 0;
        float maxPosX = 0;
        int placing = lastBranch;
        if (placing == 2)
        {
            placing = (Random.Range(0f, 1f) < 0.5f) ? 1 : 3;
        }
        if (placing == 1)
        {
            minPosX = coinOffset + finalBranch;
            maxPosX = -(coinOffset + minCoin.transform.position.x);
        }
        else if (placing == 3)
        {
            maxPosX = firstBranch - coinOffset;
            minPosX = coinOffset + minCoin.transform.position.x;
        }
        return Random.Range(minPosX, maxPosX);
    }

    private float CalculateCoinYPosition(float branchSize)
    {
        float coinOffsetY = coins[0].GetComponent<Renderer>().bounds.size.y / 2;
        float posY = transform.position.y;
        bool shouldFlip = (Random.Range(0f, 1f) < 0.5f);
        if (shouldFlip)
        {
            posY += (branchSize / 2) + coinOffsetY;
        }
        else
        {
            posY -= (branchSize / 2) + coinOffsetY;
        }
        return posY;
    }

    private GameObject ChooseRandomCoinPrefab()
    {
        return (Random.Range(0f, 1f) < 0.5f) ? coins[0] : coins[1];
    }

}
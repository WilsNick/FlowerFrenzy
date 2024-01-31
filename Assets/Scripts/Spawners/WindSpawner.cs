using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawner : MonoBehaviour
{
    [SerializeField] private GameObject windPrefab;
    [SerializeField] private float minVelocity = 20f;
    [SerializeField] private float maxVelocity = 100f;

    private Camera mainCamera;
    private Vector2 screenSize;
    private GameObject currentWindObject;
    private AudioSource currentAudioSource;
    private bool canSpawnWind = true;
    private bool muted = false;

    private void Start()
    {
        mainCamera = Camera.main;
        CalculateScreenSize();
    }

    private void Update()
    {
        if (!canSpawnWind) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartWindSpawn();
        }
        else if (Input.GetMouseButton(0))
        {
            ContinueWindSpawn();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            EndWindSpawn();
        }
    }

    private void StartWindSpawn()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0;

        currentWindObject = Instantiate(windPrefab, worldPosition, Quaternion.identity);
        currentWindObject.TryGetComponent(out currentAudioSource);
    }

    private void ContinueWindSpawn()
    {
        if (currentWindObject == null) return;

        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0f;

        Vector3 direction = targetPosition - currentWindObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentWindObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void EndWindSpawn()
    {
        if (currentWindObject == null) return;
        if(!muted)
        {
            currentAudioSource?.Play();
        }
        
        Vector3 startPosition = currentWindObject.transform.position;
        Vector3 endPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0f;

        if (startPosition == endPosition)
        {
            Destroy(currentWindObject);
            return;
        }

        float distance = Vector2.Distance(startPosition, endPosition);
        float speed = Mathf.Lerp(minVelocity, maxVelocity, distance / Mathf.Sqrt(screenSize.x * screenSize.x + screenSize.y * screenSize.y));

        Vector2 direction = (endPosition - startPosition).normalized;
        Vector2 velocity = direction * speed;
        currentWindObject.GetComponent<Rigidbody2D>().velocity = velocity;

        currentWindObject.GetComponent<BoxCollider2D>().enabled = true;
        currentWindObject = null;
    }

    private void CalculateScreenSize()
    {
        Rect cameraRect = mainCamera.pixelRect;
        float cameraHeight = mainCamera.orthographicSize * 2f;
        screenSize = new Vector2(cameraHeight * cameraRect.width / cameraRect.height, cameraHeight);
    }

    public void SetCanSpawnWind(bool value)
    {
        canSpawnWind = value;

        if (!canSpawnWind && currentWindObject != null)
        {
            Destroy(currentWindObject);
            currentWindObject = null;
        }
    }
    public void SetMuted(bool mute)
    {
        muted = mute;
    }
}

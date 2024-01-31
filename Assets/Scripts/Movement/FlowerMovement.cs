using UnityEngine;

public class FlowerMovement : MonoBehaviour
{
    [SerializeField] private float windInfluence = 0.8f;
    [SerializeField] private float maxRotationAngle = 50f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float rotationMulti = 4f;
    [SerializeField] private Animator animator;

    private Rigidbody2D _rb;
    private Quaternion _targetRotation;
    private bool _isRotating;

    private GameManager _gameManager;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _targetRotation = transform.rotation;
        _isRotating = false;
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if (_gameManager.IsStarted() && !_gameManager.IsGameOver())
        {
            animator.SetBool("Started", true);
        }

        if (_isRotating)
        {
            if (Mathf.Abs(_rb.angularVelocity) < 10f)
            {
                _isRotating = false;
                _rb.angularVelocity = 0f;
            }
            _rb.rotation = Mathf.Clamp(_rb.rotation, -maxRotationAngle, maxRotationAngle);
        }
        else if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
        {
            // Do nothing
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        _rb.angularVelocity = 0f;
        _isRotating = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wind"))
        {
            if (!_gameManager.IsStarted())
            {
                _gameManager.StartGame();
            }

            WindMovement windMovement = other.GetComponent<WindMovement>();
            if (windMovement.CanCollide)
            {
                Vector2 otherVelocity = other.GetComponent<Rigidbody2D>().velocity;
                _rb.velocity += otherVelocity * windInfluence;
                _rb.angularVelocity += otherVelocity.x * rotationMulti;
                _isRotating = true;
                windMovement.CanCollide = false;
            }
        }
        else if (other.CompareTag("Coin"))
        {
            if (!_gameManager.IsGameOver())
            {
                _gameManager.AddCoin(1);
                Destroy(other.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _gameManager.GameOver();
            animator.SetBool("Started", false);
        }
    }
}

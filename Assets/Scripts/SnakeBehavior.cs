using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // To use UI Text for countdown
using System.Collections.Generic;
using TMPro;

public class SnakeBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.up;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    public float speed = 0.1f;
    private float speedIncreaseTimer = 0f;
    private float speedIncreaseInterval = 1f;
    private float speedIncreaseAmount = 0.25f;
    
    private Vector2 touchStart;
    private Vector2 touchEnd;
    private bool isSwiping = false;

    private LifeSystem lifeSystem;
    private Score score;

    private TextMeshProUGUI countdownText;  // Reference to Text UI element
    private float countdownTimer = 3f;  // Start countdown at 3 seconds
    private bool gameStarted = false;  // Flag to check if game has started

    void Start()
    {
        _segments = new List<Transform> { this.transform };

        // Dynamically find LifeSystem and Score in the scene
        lifeSystem = FindFirstObjectByType<LifeSystem>();
        score = FindFirstObjectByType<Score>();

        if (lifeSystem == null)
        {
            Debug.LogWarning("LifeSystem not found in the scene!");
        }

        if (score == null)
        {
            Debug.LogWarning("Score system not found in the scene!");
        }

         if (countdownText == null)
        {
            // Try finding by name (make sure your countdown Text GameObject has a unique name, e.g., "CountdownText")
            GameObject countdownObject = GameObject.Find("countdownSnake");
            if (countdownObject != null)
            {
                countdownText = countdownObject.GetComponent<TextMeshProUGUI>();
            }

            if (countdownText == null)
            {
                Debug.LogWarning("No TextMeshProUGUI found for countdown in the scene! Make sure it is named 'CountdownText' and has a TextMeshProUGUI component.");
            }
        }
    }

    private void Update()
    {
        // If game hasn't started, run countdown
        if (!gameStarted)
        {
            RunCountdown();
            return;
        }

        // Game logic runs when the game starts
        speedIncreaseTimer += Time.deltaTime;

        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            speed += speedIncreaseAmount;
            speedIncreaseTimer = 0f;
        }

        DetectSwipe();
    }

    private void RunCountdown()
{
    countdownTimer -= Time.deltaTime;

    if (countdownTimer > 0)
    {
        countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
    }
    else
    {
        countdownText.text = "Go!";
        gameStarted = true;
        SpawnSnakeBody();
        Invoke("StartGame", 1f);

        // Call SetCountdownFinished() to allow score updates
        if (score != null)
        {
            score.SetCountdownFinished();
        }
    }
}


    private void StartGame()
    {
        // Start the game after countdown finishes
        countdownText.text = "";  // Clear the countdown text
    }

    private void SpawnSnakeBody()
    {
        // Spawn the body segments after the countdown ends
        for (int i = 0; i < 2; i++)
        {
            Grow();
        }
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                touchEnd = touch.position;
                isSwiping = false;
                DetermineDirection();
            }
        }
    }

    private void DetermineDirection()
    {
        Vector2 swipeDirection = touchEnd - touchStart;

        if (swipeDirection.magnitude < 50f) // Ignore small swipes
            return;

        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
        {
            ChangeDirection(swipeDirection.x > 0 ? Vector2.right : Vector2.left);
        }
        else
        {
            ChangeDirection(swipeDirection.y > 0 ? Vector2.up : Vector2.down);
        }
    }

    private void ChangeDirection(Vector2 newDirection)
    {
        if ((_direction == Vector2.up || _direction == Vector2.down) && (newDirection == Vector2.left || newDirection == Vector2.right))
        {
            _direction = newDirection;
        }
        else if ((_direction == Vector2.left || _direction == Vector2.right) && (newDirection == Vector2.up || newDirection == Vector2.down))
        {
            _direction = newDirection;
        }
    }

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;  // Don't move snake until game has started

        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        transform.position = new Vector3(
            Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            0.0f);
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    void GameOver()
    {
        GameData.score = FindFirstObjectByType<Score>()?.GetScore() ?? 0;
        GameData.lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Win Screen");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
        }
        else if (other.CompareTag("Segments"))
        {
            Debug.Log("Hit a segment!");
            Time.timeScale = 0;
            GameOver();
        }
        else if (other.CompareTag("Walls"))
        {
            if (lifeSystem != null && lifeSystem.GetCurrentLives() > 0)
            {
                lifeSystem.RemoveLife();
                _direction = (_direction == Vector2.left || _direction == Vector2.right) ? Vector2.up : Vector2.left;
            }
            else
            {
                Debug.Log("Game Over!");
                GameOver();
            }
        }
        else if (other.CompareTag("Stars"))
        {
            if (score != null)
            {
                score.AddScore(1000f);
            }
            GameData.stars++;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Life"))
        {
            if (lifeSystem != null)
            {
                lifeSystem.AddLife();
            }
            Destroy(other.gameObject);
        }
    }

    public int getSegmentCount()
    {
        return _segments.Count;
    }
}

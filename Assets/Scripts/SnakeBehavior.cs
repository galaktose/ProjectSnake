//SFX sourced from FREE Casual Game SFX Pack by Dustyroom in Unity Asset Store
//BGM sourced from 8Bit Music - 062022 by GWriterStudio in Unity Asset Store

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // To use UI Text for countdown
using System.Collections.Generic;
using System.Collections;
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
    private bool wallCollisionCooldown = false; // Cooldown flag
    private float cooldownDuration = 0.5f; // 1 second cooldown
    private AudioSource audioSource;
    public AudioClip swipeSound;
    public AudioClip hitWallSound;
    public AudioClip eatFoodSound;
    public AudioClip collectStarSound;
    public AudioClip collectLifeSound;
    public AudioClip bgMusic;


    void Start()
    {
        _segments = new List<Transform> { this.transform };

        // Dynamically find LifeSystem and Score in the scene
        lifeSystem = FindFirstObjectByType<LifeSystem>();
        score = FindFirstObjectByType<Score>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set up BGM
        audioSource.clip = bgMusic;
        audioSource.loop = true; // Ensures the BGM loops
        audioSource.volume = 0.25f ; // Set BGM volume
        audioSource.playOnAwake = false;
        

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
            // Try finding countdown by name
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

        audioSource.Play();

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
            PlaySound(swipeSound);
        }
        else if ((_direction == Vector2.left || _direction == Vector2.right) && (newDirection == Vector2.up || newDirection == Vector2.down))
        {
            _direction = newDirection;
            PlaySound(swipeSound);
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

        if (score != null)
        {
            score.multiplier += 0.25f;
        }
    }

    void GameOver()
    {
        GameData.score = FindFirstObjectByType<Score>()?.GetScore() ?? 0;
        GameData.lastScene = SceneManager.GetActiveScene().name;
        score.multiplier = 1f;
        SceneManager.LoadScene("Win Screen");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("Collided with: " + other.gameObject.name); // Log the object name
        if (other.CompareTag("Food"))
        {
            Grow();
            PlaySound(eatFoodSound);
        }
        else if (other.CompareTag("Segments"))
        {
            Debug.Log("Hit a segment!");
            GameOver();
            
        }
        else if (other.CompareTag("Walls"))
        {
        if (wallCollisionCooldown) return; // Prevent multiple collisions during cooldown

        PlaySound(hitWallSound);

        if (lifeSystem != null && lifeSystem.GetCurrentLives() > 0)
        {
            lifeSystem.RemoveLife();
            
            if (_direction == Vector2.up) // Moving up and hit a horizontal wall
            {
                _direction = Vector2.left; // Turn left
                StartCoroutine(DelayedTurn(Vector2.down));
            }
            else if (_direction == Vector2.down) // Moving down and hit a horizontal wall
            {
                _direction = Vector2.right; // Turn right
                StartCoroutine(DelayedTurn(Vector2.up));
            }
            else if (_direction == Vector2.right) // Moving right and hit a vertical wall
            {
                _direction = Vector2.up; // Turn up
                StartCoroutine(DelayedTurn(Vector2.left));
            }
            else if (_direction == Vector2.left) // Moving left and hit a vertical wall
            {
                _direction = Vector2.down; // Turn down
                StartCoroutine(DelayedTurn(Vector2.right));
            }

            // Prevent immediate re-collision
            StartCoroutine(WallCollisionCooldown());
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
            PlaySound(collectStarSound);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Life"))
        {
            if (lifeSystem != null)
            {
                lifeSystem.AddLife();
            }
            PlaySound(collectLifeSound);
            Destroy(other.gameObject);
        }
    }

    // Coroutine to apply the second turn after a delay
    private IEnumerator DelayedTurn(Vector2 newDirection)
    {
        yield return new WaitForSeconds(0.1f); // Short delay before second turn
        _direction = newDirection;
    }

    // Coroutine to disable wall collision briefly
    private IEnumerator WallCollisionCooldown()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(cooldownDuration); // 0.5 second cooldown
        GetComponent<Collider2D>().enabled = true;
    }


    public int getSegmentCount()
    {
        return _segments.Count;
    }

    // audio helper function
        private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}

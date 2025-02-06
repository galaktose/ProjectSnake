using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SnakeBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.up;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    public float speed = 0.1f;
    private float speedIncreaseTimer = 0f;
    private float speedIncreaseInterval = 1f;
    private float speedIncreaseAmount = 0.25f;
    public LifeSystem lifeSystem;
    public Score score;

    private Vector2 touchStart;
    private Vector2 touchEnd;
    private bool isSwiping = false;

    void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);

        for (int i = 0; i < 2; i++)
        {
            Grow();
        }
    }

    private void Update()
    {
        speedIncreaseTimer += Time.deltaTime;

        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            speed += speedIncreaseAmount;
            speedIncreaseTimer = 0f;
        }

        DetectSwipe();
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
            if (swipeDirection.x > 0)
                ChangeDirection(Vector2.right);
            else
                ChangeDirection(Vector2.left);
        }
        else
        {
            if (swipeDirection.y > 0)
                ChangeDirection(Vector2.up);
            else
                ChangeDirection(Vector2.down);
        }
    }

    private void ChangeDirection(Vector2 newDirection)
    {
        if (_direction == Vector2.up || _direction == Vector2.down)
        {
            if (newDirection == Vector2.left || newDirection == Vector2.right)
                _direction = newDirection;
        }
        else if (_direction == Vector2.left || _direction == Vector2.right)
        {
            if (newDirection == Vector2.up || newDirection == Vector2.down)
                _direction = newDirection;
        }
    }

    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(transform.position.x) + _direction.x,
            Mathf.Round(transform.position.y) + _direction.y,
            0.0f);
    }

    public void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    void GameOver()
    {
        GameData.score = FindFirstObjectByType<Score>().GetScore();
        GameData.lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Death Screen");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }

        if (other.tag == "Segments")
        {
            Debug.Log("Hit a segment!");
            Time.timeScale = 0;
            GameOver();
        }

        if (other.tag == "Walls")
        {
            if (lifeSystem.GetCurrentLives() > 0)
            {
                FindFirstObjectByType<LifeSystem>().RemoveLife();

                if (_direction == Vector2.left || _direction == Vector2.right)
                    _direction = Vector2.up;
                else
                    _direction = Vector2.left;
            }
            else
            {
                Debug.Log("Game Over!");
                GameOver();
            }
        }

        if (other.tag == "Stars")
        {
            score.AddScore(1000f);
            GameData.stars++;
            Destroy(other.gameObject);
        }

        if (other.tag == "Life")
        {
            FindFirstObjectByType<LifeSystem>().AddLife();
            Destroy(other.gameObject);
        }
    }

    public int getSegmentCount()
    {
        return _segments.Count;
    }
}

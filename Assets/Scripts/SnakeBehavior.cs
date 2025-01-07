using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SnakeBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.up;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    public float speed = 0.1f;
    private float speedIncreaseTimer = 0f; // Timer to track time
    private float speedIncreaseInterval = 1f; // Speed increase interval in seconds
    private float speedIncreaseAmount = 1f;
    public LifeSystem lifeSystem;
    public Score score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // Update the timer for speed increase
        speedIncreaseTimer += Time.deltaTime;

        // If 1 second has passed, increase the speed
        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            speed += speedIncreaseAmount; // Increase the speed
            speedIncreaseTimer = 0f; // Reset the timer
        }

        Vector2 newDirection = _direction;

        if (Input.GetKey(KeyCode.D))
        {
            newDirection = Vector2.right;
        } else if (Input.GetKey(KeyCode.A))
        {
            newDirection = Vector2.left;
        } else if (Input.GetKey(KeyCode.W))
        {
            newDirection = Vector2.up;
        } else if (Input.GetKey(KeyCode.S))
        {
            newDirection = Vector2.down;
        }

        // Check if the new direction is not opposite to the current direction
        if (newDirection != -_direction)
        {
            _direction = newDirection;
        }

        transform.Translate(_direction * speed * Time.deltaTime);
        //Debug.Log("Speed: " + speed);
    }

    
    private void FixedUpdate()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i-1].position;
        };

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
            Time.timeScale = 0; // Stop the game
            GameOver();
        }

        if(other.tag == "Walls")
        {
            if (lifeSystem.GetCurrentLives() > 0)
            {
                FindFirstObjectByType<LifeSystem>().RemoveLife();

                if (_direction == Vector2.left) //bounces off walls towards the opposite direction when snakes have extra lives
                {
                    _direction = Vector2.up;
                }
                else if (_direction == Vector2.right)
                {
                    _direction = Vector2.up;
                }
                else if (_direction == Vector2.up)
                {
                    _direction = Vector2.left;
                }
                else if (_direction == Vector2.down)
                {
                    _direction = Vector2.left;
                }
            }
            else
            {
                Debug.Log("Game Over!");
                GameOver();
                //Time.timeScale = 0; // Stop the game
            }
            
        }

        if ( other.tag == "Stars")
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

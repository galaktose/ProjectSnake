using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SnakeBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.up;
    private List<Transform> _segments;
    public Transform segmentPrefab;
    public float speed = 0.14f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }

    private void Update() 
    {
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
        finalScore.score = FindFirstObjectByType<Score>().GetScore();
        finalScore.lastScene = SceneManager.GetActiveScene().name;
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
            Debug.Log("Game Over!");
            Time.timeScale = 0; // Stop the game
            Invoke("GameOver", 1.5f);
        }

        if(other.tag == "Walls")
        {
            Debug.Log("Game Over!");
            GameOver();
            //Time.timeScale = 0; // Stop the game
        }

    }

    public int getSegmentCount() 
    {
        return _segments.Count;
    }
        
    
    
}

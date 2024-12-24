using UnityEngine;
using System.Collections.Generic;

public class SnakeBehavior : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments;
    public Transform segmentPrefab;

    public float speed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }

    private void Update() 
    {
        
        if (Input.GetKey(KeyCode.D))
        {
            _direction = Vector2.right;
        } else if (Input.GetKey(KeyCode.A))
        {
            _direction = Vector2.left;
        } else if (Input.GetKey(KeyCode.W))
        {
            _direction = Vector2.up;
        } else if (Input.GetKey(KeyCode.S))
        {
            _direction = Vector2.down;
        }
        
        //NEED TO MAKE THE SPEED ABLE TO CHANGE WITH THE INPUT
        transform.Translate(_direction * speed * Time.deltaTime);
        Debug.Log("Speed: " + speed);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }

        if (other.tag == "Segments") 
        {
            Debug.Log("Game Over");
        }
    }
    
}

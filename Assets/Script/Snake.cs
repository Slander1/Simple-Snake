using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mathf;

public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private float speed = 1f;
    private void Awake()
    {
        ResetState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && _direction!= Vector2.down)
            _direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.A)&& _direction!= Vector2.right)
            _direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)&& _direction!= Vector2.left)
            _direction = Vector2Int.right;
        else if (Input.GetKeyDown(KeyCode.S)&& _direction!= Vector2.up)
            _direction = Vector2Int.down;
    }

    private void FixedUpdate()
    {
        var newPos = new Vector3( Round(transform.position.x) +  _direction.x ,
            Round(transform.position.y) + _direction.y, 0.0f
        );
        for (int i = _segments.Count - 1; i > 0; i--)
            _segments[i].position = Vector3.MoveTowards(_segments[i-1].transform.position, _segments[i].transform.position, 
                speed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, newPos, 
            speed * Time.deltaTime);
    }

    private void Grow()
    {
        var segment = Instantiate(segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
        speed+=0.5f;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
            Grow();
        else if (other.tag == "Obstacle" )
            ResetState();
    }

    private void ResetState()
    {
        for (int i = 1; i < _segments.Count; i++)
            Destroy(_segments[i].gameObject);

        _segments.Clear();
        _segments.Add(transform);
        
        transform.position = Vector3.zero;
    }
}

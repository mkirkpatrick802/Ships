using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector2 _moveDir;
    private PlayerController _controller;
    private Camera _camera;


    private void Awake()
    {
        _camera = Camera.main;
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        _moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        float angle = AngleBetweenPoints(pos, transform.position);
        _controller.Rotate(angle);
    }

    private void FixedUpdate()
    {
        _controller.Move(_moveDir.normalized);
    }

    private float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}

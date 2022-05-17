using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[RequireComponent(typeof(PlayerController))]
public class PlayerInput : NetworkBehaviour
{
    private Vector2 _moveDir;
    private float _rotateAngle;
    private PlayerController _controller;
    private Camera _camera;


    private void Awake()
    {
        _camera = Camera.main;
        _controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;

        //Movement Input
        _moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //Rotation Input
        Vector2 pos = _camera.ScreenToWorldPoint(Input.mousePosition);
        _rotateAngle = AngleBetweenPoints(pos, transform.position);

        //Shoot Input
        if (!Input.GetMouseButton(0)) return;
        _controller.Fire();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        _controller.Rotate(_rotateAngle);
        _controller.Move(_moveDir.normalized);
    }

    private float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}

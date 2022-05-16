using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] [Range(5, 20)] private float _acceleration;
    [SerializeField] private float _maxSpeed;

    private Rigidbody2D _rb;
    private Transform _visuals;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _visuals = transform.GetChild(0);
    }

    public void Move(Vector2 dir)
    {
        //print(_rb.velocity.magnitude);
        if (_rb.velocity.magnitude > _maxSpeed) return;
        _rb.velocity += _acceleration * Time.deltaTime * dir;
    }

    public void Rotate(float angle)
    {
        _visuals.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}

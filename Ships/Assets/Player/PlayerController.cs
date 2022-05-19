using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    public static event Action<Transform> LocalPlayerJoined; 


    //TO-DO: Make these a scriptable object
    [Header("Movement Settings")]
    [SerializeField] [Range(5, 20)] private float _acceleration;
    [SerializeField] private float _maxSpeed;

    [Header("Health Settings")]
    [SerializeField] private int _startingHealth;

    [Header("Weapon Settings")]
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _projectileLife;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _firepoint;

    private Rigidbody2D _rb;
    private Health _health;
    private bool _canFire;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _health = new Health(_startingHealth);
        _canFire = true;
    }

    private void Start()
    {
        if (!isLocalPlayer) return;
        LocalPlayerJoined?.Invoke(transform);
    }

    [Client]
    public void Move(Vector2 dir)
    {
        if (_rb.velocity.magnitude > _maxSpeed) return;
        _rb.velocity += _acceleration * Time.deltaTime * dir;
    }

    [Client]
    public void Rotate(float angle)
    {
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    [Client]
    public void Fire()
    {
        if (!_canFire) return;
        _canFire = false;

        SpawnProjectile();
        StartCoroutine(Cooldown());
    }

    [Command]
    private void SpawnProjectile()
    {
        Projectile projectile = Instantiate(_projectile, _firepoint.position, _firepoint.rotation).GetComponent<Projectile>();
        projectile.GetComponent<Rigidbody2D>().AddForce(_projectileSpeed * transform.up, ForceMode2D.Impulse);

        projectile.Spawned(_damage, _projectileLife, transform);
        NetworkServer.Spawn(projectile.gameObject, connectionToClient);
    }

    [Client]
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_fireRate);
        _canFire = true;
    }

    [ClientRpc]
    public void TakeDamage(int damage)
    {
        _health.TakeDamage(damage);
        if (_health.CurrentHealth > 1) return;

        //Player is Dead
        NetworkServer.Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AsteroidSpawner : NetworkBehaviour
{
    [Header("Spawning Settings")]
    [SerializeField][Range(0, 3)] private float _spawnTime;
    [SerializeField] private int _radius;
    [SerializeField] private int _maxAmountOfAsteroids;

    [Header("Linking")]
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private Transform _asteroidContainer;


    private List<GameObject> _currentAsteroids = new List<GameObject>();
    private bool _isSpawning;

    private void OnValidate()
    {
        transform.localScale = new Vector2(_radius, _radius);
    }

    private void Start()
    {
        if (!isServer) return;
        Spawn();
    }

    private void Update()
    {
        if (!isServer) return;
        if (_isSpawning) return;
        if (_currentAsteroids.Count > _maxAmountOfAsteroids) return;
        Spawn();
    }

    [Server]
    private void Spawn()
    {
        if (_currentAsteroids.Count > _maxAmountOfAsteroids) { _isSpawning = false; return; };
        _isSpawning = true;

        GameObject spawned = Instantiate(_asteroidPrefab, (Random.insideUnitCircle * _radius/2) + (Vector2)transform.position, Quaternion.identity, _asteroidContainer);

        _currentAsteroids.Add(spawned);
        NetworkServer.Spawn(spawned);
        StartCoroutine(SpawnCooldown());
    }

    [Server]
    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(_spawnTime);
        Spawn();
    }
}

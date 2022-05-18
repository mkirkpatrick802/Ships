using Mirror;
using System.Collections;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    private int _damage;
    private Rigidbody2D _rb;
    private Transform _owner;

    [Server]
    public void Spawned(int damage, float speed, float life, Transform owner)
    {
        _owner = owner;
        _damage = damage;
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(speed * transform.up, ForceMode2D.Impulse);
        StartCoroutine(KillTimer(life));
    }

    [Server]
    private IEnumerator KillTimer(float life)
    {
        yield return new WaitForSeconds(life);
        NetworkServer.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        PlayerHit(collision.transform);
    }

    [Command(requiresAuthority = false)]
    private void PlayerHit(Transform player)
    {
        if (player == _owner) return;

        print(player.name + _owner.name);

        PlayerController controller = player.GetComponent<PlayerController>();
        controller.TakeDamage(_damage);
        NetworkServer.Destroy(gameObject);
    }
}

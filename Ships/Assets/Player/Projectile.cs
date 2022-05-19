using Mirror;
using System.Collections;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    private int _damage;
    private Transform _owner;

    [Server]
    public void Spawned(int damage, float life, Transform owner)
    {
        _owner = owner;
        _damage = damage;

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

        PlayerController controller = player.GetComponent<PlayerController>();
        controller.TakeDamage(_damage);
        NetworkServer.Destroy(gameObject);
    }
}

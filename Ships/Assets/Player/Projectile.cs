using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int _damage;
    private Rigidbody2D _rb;
    private PlayerController _owner;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetValues(int damage, float speed, float life, PlayerController self)
    {
        _owner = self;
        _damage = damage;
        _rb.AddForce(speed * transform.up, ForceMode2D.Impulse);
        StartCoroutine(KillTimer(life));
    }

    private IEnumerator KillTimer(float life)
    {
        yield return new WaitForSeconds(life);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        PlayerController controller = collision.transform.GetComponent<PlayerController>();
        if (controller == _owner) return;

        controller.TakeDamage(_damage);
        StopCoroutine(KillTimer(0));
        Destroy(gameObject);
    }
}

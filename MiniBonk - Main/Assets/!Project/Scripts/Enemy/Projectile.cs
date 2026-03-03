using Minibonk.Management;
using UnityEngine;
using Minibonk.Player;
public class Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;

    public void Setup(float damage, float speed, float lifetime)
    {
        _damage = damage;
        _speed = speed;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.root.TryGetComponent<PlayerHandler>(out var handler))
            {
                handler.PlayerEvents.OnPlayerDamaged?.Invoke(_damage * GameManager.Instance.ChaosMultiplier());
            }
            Destroy(gameObject);
        }
    }
}

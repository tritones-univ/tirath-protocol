using UnityEngine;

public class Arma : MonoBehaviour
{
    [Header("Daño del arma")]
    public float damage = 10f;
    public float hitCooldown = 0.5f;


    private float lastHitTime;

    void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastHitTime < hitCooldown) return;
        lastHitTime = Time.time;

        IDamageable target = other.GetComponent<IDamageable>();

        if (target != null && !target.isDead)
        {
            target.TakeDamage(damage);
        }
    }
}
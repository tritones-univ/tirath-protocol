using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    [Header("Salud")]
    [SerializeField] protected float maxHealth = 100f;
    protected float currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    public bool isDead => currentHealth <= 0f;

    protected abstract void Die();
}

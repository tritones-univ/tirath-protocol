using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    [Header("Salud")]
    // Hacemos el campo serializado público para que el Slider pueda leerlo.
    // Esto es más seguro que solo hacerlo 'public' si quieres que se mantenga 'readonly'.
    [SerializeField] private float _maxHealth = 100f; // Renombramos el campo privado

    // Propiedad pública de solo lectura para obtener el valor máximo.
    public float maxHealth => _maxHealth;

    // Hacemos 'currentHealth' pública (solo lectura) para que el Slider pueda leerla
    public float currentHealth { get; protected set; } // Puede ser leído por otros, pero solo modificado por clases hijas o la propia clase

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

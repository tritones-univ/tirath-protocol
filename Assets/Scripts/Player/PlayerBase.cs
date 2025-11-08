using UnityEngine;
using UnityEngine.Events;

public class Player : LivingEntity
{
    public UnityEvent onDeath;

    protected override void Die()
    {
        // Ejecuta eventos de muerte (UI, respawn, etc.)
        onDeath?.Invoke();
        gameObject.SetActive(false);
    }
}

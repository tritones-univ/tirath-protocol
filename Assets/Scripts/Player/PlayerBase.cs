using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : LivingEntity
{
    public UnityEvent onDeath;

    protected override void Die()
    {
        // Ejecuta eventos de muerte (UI, respawn, etc.)
        onDeath?.Invoke();
        Destroy(gameObject);
        SceneManager.LoadScene("BadEndScene");
        // gameObject.SetActive(false);
    }
}

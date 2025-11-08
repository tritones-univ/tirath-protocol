using UnityEngine;

public class Structure : LivingEntity
{
    [Header("Opciones")]
    public bool destroyOnDeath = true;

    protected override void Die()
    {
        // Aquí puedes poner efectos de destrucción, partículas, sonidos
        if (destroyOnDeath)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}

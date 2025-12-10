using UnityEngine;
using UnityEngine.UI; // Importante para usar el componente Slider

public class HealthBarUI : MonoBehaviour
{
    // 1. Referencia al componente Slider en este GameObject
    private Slider healthSlider;

    // 2. Referencia al script del Jugador. Usamos 'LivingEntity' 
    //    porque ahí están las variables de salud.
    [Tooltip("Arrastra el GameObject del Jugador aquí.")]
    public LivingEntity targetEntity;

    void Start()
    {
        // Obtener el componente Slider adjunto
        healthSlider = GetComponent<Slider>();

        if (healthSlider == null)
        {
            Debug.LogError("Error: HealthBarUI requiere un componente Slider.");
            return;
        }

        if (targetEntity == null)
        {
            Debug.LogError("Error: ¡targetEntity (el Jugador) no ha sido asignado!");
            return;
        }

        // Inicializa los valores del Slider usando los valores del LivingEntity
        // Usamos la propiedad 'maxHealth' que es 'protected', pero podemos acceder a ella
        // si la hacemos 'public' o le damos un método de acceso (getter). 
        // Para simplificar, asumiremos que tiene acceso, pero si da error,
        // mira el paso 3 (Ajuste en LivingEntity).
        healthSlider.maxValue = targetEntity.maxHealth;

        // El valor actual
        healthSlider.value = targetEntity.currentHealth;
    }

    void Update()
    {
        if (targetEntity == null || healthSlider == null) return;

        // ¡Paso clave! Actualiza el valor del Slider en cada frame
        // para que siempre coincida con la vida actual del jugador.
        healthSlider.value = targetEntity.currentHealth;
    }
}
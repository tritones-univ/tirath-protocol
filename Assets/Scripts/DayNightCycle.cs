using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
  // Duración de un día completo (día y noche) en segundos reales.
  // Puedes ajustarlo en el Inspector de Unity.
  [Tooltip("Duración de un día completo (en segundos reales).")]
  public float fullDayDurationSeconds = 480f;
  public float hourInit = 0.3f;

  // Velocidad de rotación del sol/luna
  private float rotationSpeed;

  [Header("UI")]
  public TextMeshProUGUI clockText;
  public TextMeshProUGUI dayText;

  public Image imageClock;

  public Sprite[] statusClock;
  // El tiempo actual del ciclo (entre 0 y 1)
  [HideInInspector] public float timeOfDay;
  [HideInInspector] public float dayRest = 4;
  void Start()
  {
    timeOfDay = hourInit;
    dayText.text = $"{dayRest + 1} Dias restantes";
    imageClock.sprite = statusClock[0];

    // Calculamos la velocidad de rotación: 360 grados por la duración total.
    // Dividimos por Time.deltaTime en Update para que sea consistente.
    rotationSpeed = 360f / fullDayDurationSeconds;
  }

  void Update()
  {
    timeOfDay += Time.deltaTime / fullDayDurationSeconds;

    if (timeOfDay >= 1f)
    {
      timeOfDay = 0f;
      dayRest--;
      dayText.text = $"{dayRest + 1} Dias restantes";
      if (dayRest < 0)
      {
        SceneManager.LoadScene("BadEndScene");
      }
    }
    float xRotation = timeOfDay * 360f - 90f;

    transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    UpdateClock();
  }

  void UpdateClock()
  {
    // Calcular la hora virtual
    float totalMinutesInDay = 24 * 60;
    float currentMinutes = timeOfDay * totalMinutesInDay;

    int hours = Mathf.FloorToInt(currentMinutes / 60f);
    int minutes = Mathf.FloorToInt(currentMinutes % 60f);

    if (hours >= 6 && hours < 18)
    {
      imageClock.sprite = statusClock[0];
    }
    else
    {
      imageClock.sprite = statusClock[1];
    }

    clockText.text = $"{hours:00}:{minutes:00}";
  }

  // Método opcional para cambiar el ambiente y el color de la luz
  void AdjustLighting()
  {
    // Necesitas una referencia al componente Light de este GameObject
    Light sunLight = GetComponent<Light>();

    // Usamos una curva o gradiente para cambiar el color de la luz
    // Puedes definir un Gradiente en el Inspector de Unity para un control fino
    // Por ahora, usaremos una interpolación simple (Lerp) para el color de la luz:

    // Color de día: Blanco brillante/amarillo
    Color dayColor = new Color(1f, 0.95f, 0.8f, 1f);
    // Color de noche: Azul oscuro
    Color nightColor = new Color(0.1f, 0.2f, 0.4f, 1f);

    // Una forma simple de saber si es de día o de noche (aproximadamente)
    if (timeOfDay > 0.25f && timeOfDay < 0.75f) // Si timeOfDay está entre amanecer y atardecer
    {
      // Transición suave hacia el color de día
      sunLight.color = Color.Lerp(sunLight.color, dayColor, Time.deltaTime * 0.5f);
      RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, 1f, Time.deltaTime * 0.5f);
    }
    else // Noche
    {
      // Transición suave hacia el color de noche
      sunLight.color = Color.Lerp(sunLight.color, nightColor, Time.deltaTime * 0.5f);
      RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, 0.2f, Time.deltaTime * 0.5f);
    }

    // Esto es una simplificación. Para un efecto más realista, usa un `Gradient` en el Inspector.
  }
}
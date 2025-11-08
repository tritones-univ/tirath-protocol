using UnityEngine;
using UnityEngine.UI; // Aunque Rotate no lo necesita, es buena práctica para scripts de UI

public class UIRotator : MonoBehaviour
{
  [Tooltip("Velocidad de rotación en grados por segundo. Positivo = Sentido horario, Negativo = Antihorario.")]
  public float rotationSpeed = 50f;

  private RectTransform rectTransform;

  void Start()
  {
    rectTransform = GetComponent<RectTransform>();

    if (rectTransform == null)
    {
      Debug.LogError("UIRotator requiere un RectTransform (elemento de UI). Script desactivado.");
      enabled = false;
    }
  }

  void Update()
  {
    float rotationAmount = rotationSpeed * Time.deltaTime;
    rectTransform.Rotate(0f, 0f, -rotationAmount);
  }
}
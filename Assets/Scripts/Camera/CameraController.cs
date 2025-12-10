using UnityEngine;

public class CameraController : MonoBehaviour
{
    // El objetivo (personaje) que la cámara debe seguir
    public Transform target;

    // Velocidad de suavizado del seguimiento (ajusta a tu gusto)
    public float followSpeed = 5f;

    // Distancia vertical (altura) *por encima* del objetivo
    public float relativeHeight = 5f;

    // Ángulo de inclinación de la cámara (pitch)
    public float angle = 35f;

    // Desplazamiento horizontal (hacia atrás y a un lado) desde el objetivo
    // Nota: El 'y' de este vector se usa ahora solo para el desplazamiento horizontal
    public Vector3 horizontalOffset = new Vector3(-5f, 0, -5f);

    // La rotación de la cámara, asumimos que es fija al inicio
    private Quaternion initialRotation;

    void Start()
    {
        // Almacena la rotación inicial basada en el ángulo (pitch) y el yaw (45f)
        initialRotation = Quaternion.Euler(angle, 45f, 0f);
        transform.rotation = initialRotation;

        // Coloca la cámara en la posición inicial del objetivo + offset + altura
        if (target != null)
        {
            Vector3 startPosition = target.position + horizontalOffset;
            startPosition.y = target.position.y + relativeHeight;
            transform.position = startPosition;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calcula la posición horizontal deseada (objetivo.x/z + offset.x/z)
        Vector3 desiredPosition = target.position + horizontalOffset;

        // 2. Ajusta la coordenada 'y' de la posición deseada para que esté 
        //    siempre a 'relativeHeight' *por encima* de la posición 'y' del objetivo.
        //    Esto hace que la cámara suba y baje con el objetivo.
        desiredPosition.y = target.position.y + relativeHeight;

        // 3. Suaviza la transición a la posición deseada
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        // Asegura que la rotación se mantenga constante (el ángulo que le diste)
        transform.rotation = initialRotation;
    }
}
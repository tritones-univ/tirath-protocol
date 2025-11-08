using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TurretController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform modeloTorreta;        // El modelo hijo que rota
    public Transform puntoDisparo;         // Desde donde se dispara el proyectil
    public GameObject proyectilPrefab;     // Prefab del proyectil

    [Header("Ajustes de torre")]
    public float rango = 10f;
    public float velocidadRotacion = 5f;
    public float cadenciaDisparo = 1f;     // Un disparo por segundo

    private float tiempoDisparo;
    private List<Transform> enemigosEnRango = new List<Transform>();
    private Transform objetivoActual;

    private SphereCollider rangoCollider;

    private void Awake()
    {
        rangoCollider = GetComponent<SphereCollider>();
        if (rangoCollider == null)
        {
            rangoCollider = gameObject.AddComponent<SphereCollider>();
            rangoCollider.isTrigger = true;
        }
    }

    private void Start()
    {
        rangoCollider.radius = rango; // sincroniza el rango con el collider
    }

    private void Update()
    {
        LimpiarListaDeEnemigos();

        if (enemigosEnRango.Count > 0)
        {
            objetivoActual = ObtenerObjetivoMasCercano();
            RotarHaciaObjetivo();

            if (PuedeDisparar())
                Disparar();
        }
        else
        {
            objetivoActual = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemigosEnRango.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemigosEnRango.Remove(other.transform);
        }
    }

    private void LimpiarListaDeEnemigos()
    {
        enemigosEnRango = enemigosEnRango
            .Where(e => e != null) // eliminar los que murieron
            .ToList();
    }

    private Transform ObtenerObjetivoMasCercano()
    {
        return enemigosEnRango
            .OrderBy(e => Vector3.Distance(transform.position, e.position))
            .FirstOrDefault();
    }

    private void RotarHaciaObjetivo()
    {
        if (objetivoActual == null) return;

        Vector3 direccion = objetivoActual.position - modeloTorreta.position;
        direccion.y = 0; // solo rotamos sobre el eje Y
        Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
        modeloTorreta.rotation = Quaternion.Lerp(
            modeloTorreta.rotation,
            rotacionDeseada,
            Time.deltaTime * velocidadRotacion
        );
    }

    private bool PuedeDisparar()
    {
        if (Time.time >= tiempoDisparo + 1f / cadenciaDisparo)
        {
            tiempoDisparo = Time.time;
            return true;
        }
        return false;
    }

    private void Disparar()
    {
        if (proyectilPrefab == null || puntoDisparo == null) return;

        GameObject nuevoProyectil = Instantiate(
            proyectilPrefab,
            puntoDisparo.position,
            modeloTorreta.rotation
        );

    }
}

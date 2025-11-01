using UnityEngine;

public class ObjectSpawner3D_DistanciaExacta : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    public GameObject prefab;                   // Prefab a instanciar
    public GameObject objetoReferencia;         // Objeto base desde el cual se calculará la posición
    public int cantidad = 10;                   // Cuántos objetos generar
    public float distancia = 5f;                // Distancia exacta al objeto

    [Header("Zona de generación (opcional)")]
    public SphereCollider zonaGeneracion;       // Collider opcional como guía

    [Header("Opciones")]
    public bool usarCollider = false;           // Si se usa el collider como referencia
    public bool rotacionAleatoria = true;       // Si los objetos tendrán rotación aleatoria

    void Start()
    {
        if (prefab == null || objetoReferencia == null)
        {
            Debug.LogWarning("Debes asignar un prefab y un objeto de referencia.");
            return;
        }

        GenerarObjetos();
    }

    public void GenerarObjetos()
    {
        Vector3 centro = objetoReferencia.transform.position;

        for (int i = 0; i < cantidad; i++)
        {
            Vector3 posicionSpawn;

            if (usarCollider && zonaGeneracion != null)
            {
                // Obtener centro y radio real del collider
                Vector3 colliderCenter = zonaGeneracion.transform.TransformPoint(zonaGeneracion.center);
                float radioReal = zonaGeneracion.radius * Mathf.Max(
                    zonaGeneracion.transform.lossyScale.x,
                    zonaGeneracion.transform.lossyScale.y,
                    zonaGeneracion.transform.lossyScale.z
                );

                // Generar un punto en la superficie del collider (no dentro)
                Vector3 direccion = Random.onUnitSphere;
                posicionSpawn = colliderCenter + direccion * radioReal;
            }
            else
            {
                // Generar un punto exactamente a la distancia indicada
                Vector3 direccion = Random.onUnitSphere; // Dirección aleatoria en 3D
                posicionSpawn = centro + direccion * distancia;
            }

            Quaternion rotacion = rotacionAleatoria ? Random.rotation : Quaternion.identity;
            Instantiate(prefab, posicionSpawn, rotacion);
        }

        Debug.Log($"Se generaron {cantidad} objetos de {prefab.name} a una distancia de {distancia} alrededor de {objetoReferencia.name}");
    }

#if UNITY_EDITOR
    // Dibuja la esfera en el editor para depuración visual
    void OnDrawGizmosSelected()
    {
        if (objetoReferencia != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(objetoReferencia.transform.position, distancia);
        }
    }
#endif
}

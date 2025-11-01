using System.Collections.Generic;
using UnityEngine;

public class GeneradorMinerales : MonoBehaviour
{
    [Header("Área de generación")]
    public float xMin = -10f;
    public float xMax = 10f;
    public float zMin = -10f;
    public float zMax = 10f;
    public float distanciaXpuntos = 5f;

    [Header("Mineral")]
    public GameObject mineral;
    public int cantidad = 10;

    [Header("Terreno (opcional)")]
    public Terrain terreno;

    [Header("Distancia mínima entre minerales")]
    public float distanciaMinima = 3f;

    private List<Vector3> possiblePoints = new List<Vector3>();
    private List<Vector3> spawnedAlready = new List<Vector3>();

    void Start()
    {
        GeneradorGridPoints();
        GeneradorRecursos();
    }

    void GeneradorGridPoints()
    {
        possiblePoints.Clear();

        for (float x = xMin; x <= xMax; x += distanciaXpuntos)
        {
            for (float z = zMin; z <= zMax; z += distanciaXpuntos)
            {
                float y = 0f;
                if (terreno != null)
                {
                    y = terreno.SampleHeight(new Vector3(x, 0, z));
                }

                Vector3 point = new Vector3(x, y, z);
                possiblePoints.Add(point);
            }
        }
    }

    bool PositionValidate(Vector3 punto)
    {
        foreach (var pos in spawnedAlready)
        {
            if (Vector3.Distance(punto, pos) < distanciaMinima)
                return false;
        }
        return true;
    }

    void GeneradorRecursos()
    {
        spawnedAlready.Clear();
        int generados = 0;
        int intentosMaximos = 500;
        int intentos = 0;

        while (generados < cantidad && intentos < intentosMaximos)
        {
            intentos++;
            Vector3 punto = possiblePoints[Random.Range(0, possiblePoints.Count)];

            if (PositionValidate(punto))
            {
                Instantiate(mineral, punto, Quaternion.identity);
                spawnedAlready.Add(punto);
                generados++;
            }
        }

        Debug.Log("Minerales generados: " + generados);
    }
    void OnDrawGizmosSelected()
{
    // Color del gizmo
    Gizmos.color = Color.yellow;

    // Calculamos el centro del área
    Vector3 centro = new Vector3((xMin + xMax) / 2f, 0f, (zMin + zMax) / 2f);

    // Calculamos el tamaño del área
    Vector3 tamaño = new Vector3(Mathf.Abs(xMax - xMin), 0.1f, Mathf.Abs(zMax - zMin));

    // Dibujamos el contorno del área
    Gizmos.DrawWireCube(centro, tamaño);
}

}


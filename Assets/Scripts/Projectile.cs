using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocidad = 20f;
    public float daño = 10f;
    public float vidaMax = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, vidaMax);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Aquí puedes acceder a un script del enemigo y aplicarle daño
            // other.GetComponent<Enemigo>().RecibirDaño(daño);
            Destroy(gameObject);
        }
    }
}

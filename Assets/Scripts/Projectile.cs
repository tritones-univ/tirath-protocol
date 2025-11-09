using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocidad = 20f;
    public float damage = 10f;
    public float vidaMax = 3f;
    public float hitCooldown = 0.5f;
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

        IDamageable target = other.GetComponent<IDamageable>();

        if (target != null && !target.isDead)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}

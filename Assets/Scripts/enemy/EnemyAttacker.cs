using System.Collections;
using UnityEngine;

public class EnemyAttacker : EnemyBase
{
    [Header("Objetivos")]
    public LayerMask playerLayer;
    public LayerMask structureLayer;
    public float structurePriorityDistance = 10f;

    protected override void FindTarget()
    {
        // Primero busca estructuras en rango
        Collider[] structures = Physics.OverlapSphere(transform.position, detectionRadius, structureLayer);
        Transform bestStructure = null;
        float bestDist = Mathf.Infinity;

        foreach (var s in structures)
        {
            float dist = Vector3.SqrMagnitude(s.transform.position - transform.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestStructure = s.transform;
            }
        }

        // Luego busca jugador
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        Transform playerTransform = players.Length > 0 ? players[0].transform : null;

        // Lógica de prioridad
        if (bestStructure != null && Mathf.Sqrt(bestDist) <= structurePriorityDistance)
        {
            target = bestStructure.GetComponent<IDamageable>();
        }
        else if (playerTransform != null)
        {
            target = playerTransform.GetComponent<IDamageable>();
        }
        else if (bestStructure != null)
        {
            target = bestStructure.GetComponent<IDamageable>();
        }
        else
        {
            target = null;
        }
    }

    protected override void Attack(IDamageable target)
    {
        if (target != null)
            target.TakeDamage(attackDamage);
    }

    protected override void Die()
    {
        PlayIdleAnim();
        PlayDeathAnim();
        if (agent != null) agent.isStopped = true;
        StartCoroutine(DeathDelay());
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(0.333f); // espera animación
        Destroy(gameObject);
    }

}

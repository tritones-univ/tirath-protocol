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
        IDamageable bestTarget = null;
        float closestDistSqr = Mathf.Infinity;

        Vector3 myPos = transform.position;

        // 1️⃣ Buscar estructuras en rango
        Collider[] structures = Physics.OverlapSphere(myPos, detectionRadius, structureLayer);
        foreach (var s in structures)
        {
            float distSqr = (s.transform.position - myPos).sqrMagnitude;
            if (distSqr < closestDistSqr)
            {
                closestDistSqr = distSqr;
                bestTarget = s.GetComponent<IDamageable>();
            }
        }

        // 2️⃣ Buscar jugador
        Collider[] players = Physics.OverlapSphere(myPos, detectionRadius, playerLayer);
        if (players.Length > 0)
        {
            Transform playerTransform = players[0].transform;
            float playerDistSqr = (playerTransform.position - myPos).sqrMagnitude;

            // Solo cambiar a jugador si no hay estructuras dentro de la distancia de prioridad
            if (closestDistSqr > structurePriorityDistance * structurePriorityDistance || bestTarget == null)
            {
                bestTarget = players[0].GetComponent<IDamageable>();
            }
        }

        target = bestTarget;
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

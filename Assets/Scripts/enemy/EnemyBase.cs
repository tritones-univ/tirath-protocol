using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : LivingEntity
{
    [Header("Movimiento")]
    protected NavMeshAgent agent;
    public float detectionRadius = 12f;
    public float meleeRange = 1.8f;

    [Header("Ataque")]
    public float attackDamage = 10f;
    public float attackRate = 1f;
    protected float lastAttackTime = 0f;

    protected IDamageable target;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (target == null || target.isDead)
        {
            FindTarget();
        }
        else
        {
            MoveTowardsTarget();
            TryAttack();
        }
    }

    protected abstract void FindTarget();

    protected virtual void MoveTowardsTarget()
    {
        if (target != null)
            agent.SetDestination(((MonoBehaviour)target).transform.position);
    }

    protected virtual void TryAttack()
    {
        if (target == null || target.isDead) return;

        float distance = Vector3.Distance(transform.position, ((MonoBehaviour)target).transform.position);
        if (distance <= meleeRange)
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime >= 1f / attackRate)
            {
                lastAttackTime = Time.time;
                Attack(target);
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }

    protected abstract void Attack(IDamageable target);
}

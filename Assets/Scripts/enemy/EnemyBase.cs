using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : LivingEntity
{
    [Header("Movimiento")]
    protected NavMeshAgent agent;
    public float detectionRadius = 12f;
    public float meleeRange = 1.8f;
    public float rotationSpeed = 10f; // velocidad de giro suave

    [Header("Ataque")]
    public float attackDamage = 10f;
    public float attackRate = 1f;
    protected float lastAttackTime = 0f;

    protected IDamageable target;

    [Header("Animaciones")]
    public Animator animator;
    [SerializeField] protected string walkAnim = "isWalking"; // Bool
    [SerializeField] protected string attackAnim = "Attack";  // Trigger
    [SerializeField] protected string dieAnim = "Die";        // Trigger

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();

        // Buscar animator si no fue asignado
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // 🔧 Corregir orientación inicial del modelo (mirar hacia Z+)
        AlignModelForward();
    }

    protected virtual void Update()
    {
        if (target == null || target.isDead)
        {
            FindTarget();
            PlayIdleAnim();
        }
        else
        {
            MoveTowardsTarget();
            TryAttack();
        }

        SmoothRotateTowardsMovement();
    }

    // Método abstracto → subclases definen cómo buscar su objetivo
    protected abstract void FindTarget();

    #region Movimiento y rotación
    protected virtual void MoveTowardsTarget()
    {
        if (target != null)
        {
            agent.SetDestination(((MonoBehaviour)target).transform.position);
            PlayWalkAnim();
        }
    }

    /// <summary>
    /// Gira suavemente el enemigo hacia la dirección de movimiento
    /// </summary>
    protected void SmoothRotateTowardsMovement()
    {
        Vector3 velocity = agent.velocity;
        velocity.y = 0; // ignorar altura

        if (velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Corrige la orientación del modelo al inicio para que mire hacia Z+
    /// </summary>
    protected void AlignModelForward()
    {
        // Busca un hijo con MeshRenderer o SkinnedMeshRenderer
        Transform model = GetComponentInChildren<SkinnedMeshRenderer>()?.transform;

        if (model == null)
            model = GetComponentInChildren<MeshRenderer>()?.transform;

        if (model != null)
        {
            // Si el modelo no está mirando al frente, lo reorientamos
            model.localRotation = Quaternion.identity;
        }
    }
    #endregion

    #region Animaciones
    protected void PlayWalkAnim()
    {
        if (animator == null) return;
        animator.SetBool(walkAnim, true);
    }

    protected void PlayAttackAnim()
    {
        if (animator == null) return;
        animator.SetTrigger(attackAnim);
    }

    protected void PlayIdleAnim()
    {
        if (animator == null) return;
        animator.SetBool(walkAnim, false);
    }

    protected void PlayDeathAnim()
    {
        if (animator == null) return;
        animator.SetTrigger(dieAnim);
        agent.isStopped = true;
        Destroy(gameObject, 2f);
    }
    #endregion

    #region Ataque
    protected virtual void TryAttack()
    {
        if (target == null || target.isDead) return;

        float distance = Vector3.Distance(transform.position, ((MonoBehaviour)target).transform.position);

        if (distance <= meleeRange)
        {
            agent.isStopped = true;
            PlayIdleAnim(); // Asegura que "isWalking" se desactive antes de atacar
            PlayAttackAnim();

            if (Time.time - lastAttackTime >= 1f / attackRate)
            {
                lastAttackTime = Time.time;
                Attack(target);
            }
        }
        else
        {
            agent.isStopped = false;
            PlayWalkAnim();
        }
    }

    // Subclases definen cómo atacar (daño, tipo, efectos, etc.)
    protected abstract void Attack(IDamageable target);
    #endregion

    #region Vida
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        // Aquí podrías reproducir una animación de recibir daño si quieres
    }

    protected override void Die()
    {
        PlayDeathAnim();
    }
    #endregion
}

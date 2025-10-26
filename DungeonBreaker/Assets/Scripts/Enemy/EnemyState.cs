using Unity.VisualScripting;
using UnityEngine;

#region SpiritIdleState
public class SpiritIdleState : IEnemyState
{
    private const int LeftDirection = -1;
    private const int RightDirection = 1;
    private EnemyMovement enemy;
    private Transform player;

    public SpiritIdleState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        Debug.Log($"<color=cyan>{enemy.name} entered SpiritIdleState</color>");
        
        Events.RequestMove(enemy, false);
    }

    public override void Execute()
    {
        Debug.Log($"<color=cyan>{enemy.name} executing SpiritIdleState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

       
        int dir = (player.position.x > enemy.transform.position.x) ? LeftDirection : RightDirection;
        Events.RequestDirection(enemy, dir);

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float attackRange = enemy.GetEnemyDate().attackRange;
        float detection = enemy.GetEnemyDate().detectionRange;
        var attackComp = enemy.GetEnemyAttack();

        if (enemy.GetEnemyDate().enemyType == EnemyType.Spirit)
        {
            if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
            {
                Events.RequestStateChange(enemy, enemy.attackState);
                return;
            }

            if (dist <= detection)
            {
                Events.RequestStateChange(enemy, enemy.chaseState);
                return;
            }
        }

        if (enemy.GetEnemyDate().enemyType == EnemyType.Boss)
        {
            if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
            {
                Events.RequestStateChange(enemy, enemy.attackState);
                return;
            }
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=cyan>{enemy.name} exited SpiritIdleState</color>");
        
        Events.RequestMove(enemy, true);
    }
}

#endregion

#region IdleState
public class IdleState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public IdleState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        Debug.Log($"<color=yellow>{enemy.name} entered IdleState</color>");
       
        Events.RequestMove(enemy, false);
    }

    public override void Execute()
    {
        Debug.Log($"<color=yellow>{enemy.name} executing IdleState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            
            Events.RequestStateChange(enemy, enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            
            Events.RequestStateChange(enemy, enemy.chaseState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=yellow>{enemy.name} exited IdleState</color>");
        
        Events.RequestMove(enemy, true);
    }
}

#endregion

#region PatrolState
public class PatrolState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;

    public PatrolState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        Debug.Log($"<color=green>{enemy.name} entered PatrolState</color>");
        Events.RequestMove(enemy, true);
    }

    public override void Execute()
    {
        Debug.Log($"<color=green>{enemy.name} executing PatrolState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
           
            Events.RequestStateChange(enemy, enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            
            Events.RequestStateChange(enemy, enemy.chaseState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=green>{enemy.name} exited PatrolState</color>");
        
    }
}

#endregion

#region ChaseState
public class ChaseState : IEnemyState
{
    private const int RightDirection = 1;
    private const int LeftDirection = -1;
    private EnemyMovement enemy;
    private Transform player;

    public ChaseState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        Debug.Log($"<color=magenta>{enemy.name} entered ChaseState</color>");
        Events.RequestMove(enemy, true);
    }

    public override void Execute()
    {
        Debug.Log($"<color=magenta>{enemy.name} executing ChaseState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            Events.RequestStateChange(enemy, enemy.patrolState);
            return;
        }

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        
        int dir = (player.position.x > enemy.transform.position.x) ? RightDirection : LeftDirection;
        Events.RequestDirection(enemy, dir);

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            Events.RequestStateChange(enemy, enemy.attackState);
            return;
        }

        if (dist > detection)
        {
            Events.RequestStateChange(enemy, enemy.patrolState);
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=magenta>{enemy.name} exited ChaseState</color>");
        
    }
}


#endregion

#region AttackState
public class AttackState : IEnemyState
{
    private EnemyMovement enemy;
    private EnemyAttack attackComp;
    private Transform player;
    private bool attackedThisEntry = false;
    private const float ResetTime = 0f;
    private float attackTime = ResetTime;
    private float postAttackDelay = 1f;

    public AttackState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        attackComp = enemy.GetComponent<EnemyAttack>();
        player = enemy.GetPlayer();
    }

    public override void Enter()
    {
        Debug.Log($"<color=red>{enemy.name} entered AttackState</color>");
        attackedThisEntry = false;
        attackTime = ResetTime;
    }

    public override void Execute()
    {
        Debug.Log($"<color=red>{enemy.name} executing AttackState</color>");

        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            ReturnToIdleOrPatrol();
            return;
        }

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float attackRange = enemy.GetEnemyDate().attackRange;
        float detection = enemy.GetEnemyDate().detectionRange;

        
        if (!attackedThisEntry && attackComp != null && attackComp.CanAttack() && dist <= attackRange)
        {
            Debug.Log($"<color=red>{enemy.name} ATTACK</color>");
            attackComp.TriggerAttack();
            attackedThisEntry = true;
            attackTime = Time.time;
            return;
        }

       
        if (attackedThisEntry)
        {
            if (Time.time - attackTime >= postAttackDelay)
            {
                ReturnToIdleOrPatrol();
                return;
            }
            return;
        }

        
        if (dist > detection)
        {
            ReturnToIdleOrPatrol();
            return;
        }
    }

    private void ReturnToIdleOrPatrol()
    {
        
        if (enemy.GetEnemyDate().enemyType == EnemyType.Spirit || enemy.GetEnemyDate().enemyType == EnemyType.Boss)
        {
            Events.RequestStateChange(enemy, enemy.spiritIdleState);
        }
        else
        {
            Events.RequestStateChange(enemy, enemy.patrolState);
        }
    }

    public override void Exit()
    {
        Debug.Log($"<color=red>{enemy.name} exited AttackState</color>");
    }
}

#endregion

#region DieState
public class DieState : IEnemyState
{
    private EnemyMovement enemy;

    public DieState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        Debug.Log($"<color=grey>{enemy.name} entered DieState</color>");

        
        Events.RequestMove(enemy, false);

        
        var attack = enemy.GetEnemyAttack();
        if (attack != null)
            attack.enabled = false;

        
        enemy.GetAnimator()?.SetTrigger("IsDead");

       
        Events.OnEnemyDeath?.Invoke(enemy.gameObject);
    }

    public override void Execute()
    {
        Debug.Log($"<color=grey>{enemy.name} executing DieState</color>");
       
    }

    public override void Exit()
    {
        Debug.Log($"<color=grey>{enemy.name} exited DieState</color>");
    }
}

#endregion

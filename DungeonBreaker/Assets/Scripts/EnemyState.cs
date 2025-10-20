using Unity.VisualScripting;
using UnityEngine;

#region SpiritIdleState
public class SpiritIdleState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;
    private float yTolerance =1f;

    public SpiritIdleState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    private bool IsPlayerOnSameHeight()
    {
        if (player == null) return false;
        float deltaY = Mathf.Abs(player.position.y - enemy.transform.position.y);
        return deltaY <= yTolerance;
    }

    public override void Enter()
    {
        enemy.StopMoving();
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        if (!IsPlayerOnSameHeight()) return;
        if (enemy.GetEnemyDate().enemyType == EnemyType.Boss)
        {
            if (player.position.x > enemy.transform.position.x)
                enemy.SetDirection(-1);
            else
                enemy.SetDirection(1);
        }
        else
        {
            if (player.position.x > enemy.transform.position.x)
                enemy.SetDirection(1);
            else
                enemy.SetDirection(-1);
        }
        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float attackRange = enemy.GetEnemyDate().attackRange;
        float detection = enemy.GetEnemyDate().detectionRange;
        var attackComp = enemy.GetEnemyAttack();

        if (enemy.GetEnemyDate().enemyType == EnemyType.Spirit)
        {
            if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
            {
                enemy.ChangeState(enemy.attackState);
                return;
            }

            if (dist <= detection)
            {
                enemy.ChangeState(enemy.chaseState);
                return;
            }
        }

        if (enemy.GetEnemyDate().enemyType == EnemyType.Boss)
        {
            if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
            {
                enemy.ChangeState(enemy.attackState);
                return;
            }
        }
    }

    public override void Exit()
    {
        enemy.ResumeMoving();
    }
}
#endregion

#region IdleState
public class IdleState : IEnemyState
{
    
    private EnemyMovement enemy;
    private Transform player;
    private float yTolerance = 0.5f;

    public IdleState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    private bool IsPlayerOnSameHeight()
    {
        if (player == null) return false;
        float deltaY = Mathf.Abs(player.position.y - enemy.transform.position.y);
        return deltaY <= yTolerance;
    }

    public override void Enter()
    {
        enemy.StopMoving();
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;

        if (!IsPlayerOnSameHeight()) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }
    }

    public override void Exit()
    {
        enemy.ResumeMoving();
    }
}
#endregion

#region PatrolState
public class PatrolState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;
    private float yTolerance = 0.5f;

    public PatrolState(EnemyMovement enemy)
    {
        Debug.Log("PatrolState: Execute running");
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    private bool IsPlayerOnSameHeight()
    {
        if (player == null) return false;
        float deltaY = Mathf.Abs(player.position.y - enemy.transform.position.y);
        return deltaY <= yTolerance;
    }

    public override void Enter()
    {
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null) return;


        if (!IsPlayerOnSameHeight()) return;


        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        if (dist <= detection)
        {
            enemy.ChangeState(enemy.chaseState);
            return;
        }

        // תזוזת פטרול מתבצעת ב-EnemyMovement
    }

    public override void Exit() { }
}
#endregion

#region ChaseState
public class ChaseState : IEnemyState
{
    private EnemyMovement enemy;
    private Transform player;
    private float yTolerance = 0.5f;

    public ChaseState(EnemyMovement enemy)
    {
        this.enemy = enemy;
        player = enemy.GetPlayer();
    }

    private bool IsPlayerOnSameHeight()
    {
        if (player == null) return false;
        float deltaY = Mathf.Abs(player.position.y - enemy.transform.position.y);
        return deltaY <= yTolerance;
    }

    public override void Enter()
    {
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }


        if (!IsPlayerOnSameHeight()) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float detection = enemy.GetEnemyDate().detectionRange;
        float attackRange = enemy.GetEnemyDate().attackRange;
        var attackComp = enemy.GetEnemyAttack();

        int dir = (player.position.x > enemy.transform.position.x) ? 1 : -1;
        enemy.SetDirection(dir);

        if (dist <= attackRange && (attackComp == null || attackComp.CanAttack()))
        {
            enemy.ChangeState(enemy.attackState);
            return;
        }

        if (dist > detection)
        {
            enemy.ChangeState(enemy.patrolState);
            return;
        }
    }


    public override void Exit() { }
}
#endregion

#region AttackState
public class AttackState : IEnemyState
{
    private EnemyMovement enemy;
    private EnemyAttack attackComp;
    private Transform player;
    private bool attackedThisEntry = false;
    private float attackTime = 0f;
    private float postAttackDelay = 1f;
    private float yTolerance = 1f;

    public AttackState(EnemyMovement enemy)
    {
        Debug.Log("AttackState: Execute running");
        this.enemy = enemy;
        attackComp = enemy.GetComponent<EnemyAttack>();
        player = enemy.GetPlayer();
    }

    private bool IsPlayerOnSameHeight()
    {
        if (player == null) return false;
        float deltaY = Mathf.Abs(player.position.y - enemy.transform.position.y);
        return deltaY <= yTolerance;
    }

    public override void Enter()
    {
        attackedThisEntry = false;
        attackTime = 0f;
    }

    public override void Execute()
    {
        if (player == null) player = enemy.FindPlayer();
        if (player == null)
        {
            ReturnToIdleOrPatrol();
            return;
        }

        if (!IsPlayerOnSameHeight()) return;

        float dist = Vector2.Distance(enemy.transform.position, player.position);
        float attackRange = enemy.GetEnemyDate().attackRange;
        float detection = enemy.GetEnemyDate().detectionRange;

        if (!attackedThisEntry && attackComp != null && attackComp.CanAttack() && dist <= attackRange)
        {
            Debug.Log("AttackState: Triggering attack");
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
            enemy.ChangeState(enemy.spiritIdleState);
        }
        else
        {
            enemy.ChangeState(enemy.patrolState);
        }
    }

    public override void Exit() { }
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
        enemy.StopMoving();
        var attack = enemy.GetEnemyAttack();
        if (attack != null) attack.enabled = false;
        enemy.GetAnimator()?.SetTrigger("IsDead");
    }

    public override void Execute() { }

    public override void Exit() { }
}
#endregion
